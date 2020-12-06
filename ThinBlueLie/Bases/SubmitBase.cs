using Dapper;
using DataAccessLibrary.DataModels.EditModels;
using DataAccessLibrary.Enums;
using DataAccessLibrary.UserModels;
using Ganss.XSS;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using Syncfusion.Blazor.RichTextEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Bases;
using ThinBlueLie.Helper;
using ThinBlueLie.Helper.Extensions;

namespace ThinBlueLie.Models
{
    public partial class SubmitBase : InformationBase
    {

        [Inject] NavigationManager NavManager { get; set; }
        [Inject] SignInManager<ApplicationUser> signInManager { get; set; }
        [Inject] UserManager<ApplicationUser> userManager { get; set; }

        public AuthenticationState userState;
        protected async Task HandleValidSubmitAsync()
        {
            //display loading gif in modal while doing the processing
            //TODO add locked option in form for authed users with perms

            model.Timelineinfos.Locked = 0;
            string? userId;
            if (signInManager.IsSignedIn(userState.User))
                userId = userManager.GetUserId(userState.User);
            else
                userId = null;

            EditHistory editHistory = new EditHistory();
            //TODO if user is has perms auto verify
            int IdTimelineinfo;
            var sanitizer = new HtmlSanitizer();
            using (var connection = new MySqlConnection(ConfigHelper.GetConnectionString()))
            {
                connection.Open();

                int EditHistoryId;
                string createNewEditHistory = @"INSERT INTO edithistory (`Confirmed`, `SubmittedBy`) 
                                                        VALUES ('2', @userId);
                                            SELECT LAST_INSERT_ID();
                                            SET @v1 = (SELECT Max(e.IdTimelineinfo +1) FROM edithistory e);
                                            SELECT @v1;";
                using (var multi = await connection.QueryMultipleAsync(createNewEditHistory, new { userId }))
                {
                    EditHistoryId = editHistory.IdEditHistory = multi.Read<int>().FirstOrDefault();
                    IdTimelineinfo = multi.Read<int>().FirstOrDefault();
                }


                var timelineSave = $@"INSERT INTO edits (`IdTimelineinfo`, `IdEditHistory`,`Date`, `State`, `City`, `Context`, `Locked`)
                                       VALUES ('{IdTimelineinfo}', '{EditHistoryId}', @Date, @State, @City, @context, @Locked);";

                //Insert data into edits table
                connection.QuerySingle<int>(timelineSave, new
                {
                    model.Timelineinfos.Date,
                    model.Timelineinfos.State,
                    model.Timelineinfos.City,
                    context = sanitizer.Sanitize(model.Timelineinfos.Context),
                    model.Timelineinfos.Locked,
                    submittedby = userId, //nullable
                });               


                foreach (var media in model.Medias)
                {
                    string path;
                    //if there is a file for the media save it to disk
                    if (media.Source != null & media.SourceFrom == MediaEnums.SourceFromEnum.Device)
                    {
                        var rand = StringExtensions.RandomString(20); //random string of 20 characters
                        path = ConfigHelper.GetUploadsDirectory() + rand + ".jpg";
                        FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.Write);
                        media.Source.Stream.WriteTo(filestream);
                        filestream.Close();
                        media.Source.Stream.Close();
                    }
                    else
                    {
                        path = media.SourcePath;
                    }
                    string mediaSql = $@"INSERT INTO editmedia (`IdEditHistory`, `IdTimelineinfo`, `MediaType`, `SourcePath`, `Gore`, `SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Rank`)
                                    VALUES ('{EditHistoryId}', @IdTimelineinfo, @MediaType, @SourcePath, @Gore, @SourceFrom, @Blurb, @Credit, '{userId}', '{media.ListIndex}');";
                    await connection.ExecuteAsync(mediaSql, media);
                    editHistory.EditMedia = 1;
                }

                //Subject Table
                foreach (var subject in model.Subjects)
                {
                    int IdSubject = 0;
                    if (subject.SameAsId == null)
                    {
                        //Create new subject
                        var subjectSql = $@"INSERT INTO edits_subject 
                                            (`IdEditHistory`, `Name`, `Race`, `Sex`, `Action`)
                                            VALUES ('{EditHistoryId}', @Name, @Race, @Sex, '0');
                                          SELECT LAST_INSERT_ID();";
                        //Add to subjects table and return id
                        IdSubject = connection.QuerySingle<int>(subjectSql, subject);
                        editHistory.Subjects = 1;
                    }
                        //Add to junction table    
                        var junctionSql = "INSERT INTO edits_timelineinfo_subject (`IdEditHistory`, `IdTimelineinfo`, `IdSubject`, `Armed`, `Age`)" +
                                         "VALUES (@EditHistoryId, @IdTimelineinfo, @IdSubject, @Armed, @Age);";
                        connection.Execute(junctionSql, new
                        {
                            EditHistoryId,
                            IdTimelineinfo,
                            IdSubject = subject.SameAsId == null? IdSubject : subject.SameAsId,
                            Armed = Convert.ToByte(subject.Armed),
                            subject.Age
                        });
                    editHistory.Timelineinfo_Subject = 1;
                }
                //Officer Table
                foreach (var officer in model.Officers)
                {
                    int IdOfficer = 0;
                    if (officer.SameAsId == null)
                    {
                        //Create new officer
                        var officerSql = "INSERT INTO edits_officers (`IdEditHistory`, `Name`, `Race`, `Sex`, `Action`)" +
                                     "VALUES ('{EditHistoryId}', @Name, @Race, @Sex, '0');" +
                                     "SELECT LAST_INSERT_ID();";
                        //Add to officers table and return id
                        IdOfficer = connection.QuerySingle<int>(officerSql, officer);
                        editHistory.Officers = 1;
                    }
                        //Add to junction table
                        var junctionSql = "INSERT INTO edits_timelineinfo_officer (`IdEditHistory`, `IdTimelineinfo`, `IdOfficer`, `Age`, `Misconduct`, `Weapon`)" +
                                        "VALUES (@EditHistoryId, @idtimelineinfo, @idofficer, @age, @misconduct, @weapon);";
                        connection.Execute(junctionSql, new
                        {
                            EditHistoryId,
                            idtimelineinfo = IdTimelineinfo,
                            idofficer = officer.SameAsId == null? IdOfficer : officer.SameAsId,
                            age = officer.Age,
                            misconduct = officer.Misconduct.Sum(),
                            weapon = officer.Weapon?.Sum(),
                        });
                    editHistory.Timelineinfo_Officer = 1;
                }
                string updateEditHistory = @$"UPDATE edithistory SET 
                                               `IdTimelineinfo` = @IdTimelineinfo,
                                               `Confirmed` = '0',`Edits` = @Edits, `EditMedia` = @EditMedia,
                                                `Officers` = @Officers, Subjects` = @Subjects, `Timelineinfo_Officer` = @Timelineinfo_Officer, 
                                                `Timelineinfo_Subject` = @Timelineinfo_Subject 
                                         WHERE (`IdEditHistory` = '{EditHistoryId}');";
                await connection.ExecuteAsync(updateEditHistory, editHistory);
            }
           


            //TODO move all into querymultiple
            if (true) //TODO check if successful submit
            {
                NavManager.NavigateTo("/Index");
            }
        }

    }
}
