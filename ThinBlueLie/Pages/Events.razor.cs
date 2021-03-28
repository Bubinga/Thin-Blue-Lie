using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ThinBlueLie.Components;
using ThinBlueLie.Models;
using DataAccessLibrary.DataModels;
using ThinBlueLie.ViewModels;
using ThinBlueLie.Helper.Extensions;
using static ThinBlueLie.Helper.ConfigHelper;
using HtmlAgilityPack;
using ThinBlueLie.Models.ViewModels;
using DataAccessLibrary.UserModels;
using DataAccessLibrary.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ThinBlueLie.Pages
{
    public partial class Events
    {
        public ViewEvent Event { get; set; }

        public List<FirstLoadEvents> DatesEvents { get; set; }

        [Parameter] public DateTime? Date { get; set; }

        [Parameter] public string Title { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthState { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }
        [Inject] IDataAccess Data { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] UserManager<ApplicationUser> UserManager { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        public AuthenticationState userState;
        public ApplicationUser User;
        public bool EventExists = false;
        bool MultipleEvents;
        string DbTitle;
        bool NoEvents;
        bool FakeEvent;
        protected override async Task OnInitializedAsync()
        {
            EventExists = MultipleEvents = NoEvents = FakeEvent = false;
            if (Date == null)
            {
                Date = DateTime.Today;
            }

            userState = await AuthState;
            User = await UserManager.GetUserAsync(userState.User);
        }

        protected override async Task OnParametersSetAsync()
        {
            EventExists = MultipleEvents = NoEvents = FakeEvent = false;
            if (Date == null)
                return;

            if (Date > DateTime.Today.Date || Date <= InformationBase.MinDate)
                Date = DateTime.Today;

            if (Title != null)
            {
                await ChangeDay();
            }
            else
            {
                DatesEvents = new List<FirstLoadEvents>(); //clearing previous information
                await GetDaysEvents();
                if (DatesEvents.Count == 1)
                {
                    if (Title == null)
                    {
                        NavManager.NavigateTo(NavManager.BaseUri + "Events/" + Date?.ToString("yyyy-MM-dd") + "/" + UriExtensions.CreateTitleUrl(DatesEvents.FirstOrDefault().Title)); //recall onparameters set async
                        return;
                    }
                }
                else if (DatesEvents.Count > 1)
                {
                    MultipleEvents = true;
                }
            }

            if (Event?.Data?.City == null || DatesEvents?.Count == 0)
            {
                NoEvents = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender && EventExists)
            {
                await JsRuntime.InvokeVoidAsync("InitializeSwiper");
            }
        }

        async Task GetDaysEvents()
        {
            List<FirstLoadEvents> events = new();
            string getEventsFromDate = "SELECT t.Title, t.Context, t.IdTimelineinfo FROM timelineinfo t WHERE t.Date = @date";
            (await Data.LoadDataNoLog<FirstLoadEvents?, dynamic>(getEventsFromDate, new{date = Date?.ToString("yyyy-MM-dd")},GetConnectionString())).ForEach(e => events.Add(e));
            int eventCount = 0;
            if (events.Any())
            {
                foreach (var evnt in events)
                {
                    string getFirstMedia = "Select *,(true) as Processed from media m where m.IdTimelineinfo = @id order by m.rank Limit 1";
                    evnt.Media = new ViewMedia();
                    evnt.Media = await Data.LoadDataSingle<ViewMedia, dynamic>(getFirstMedia, new {id = evnt.IdTimelineinfo});
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(evnt.Context);
                    var htmlBody = htmlDoc.DocumentNode;
                    HtmlNode text = htmlBody.FirstChild;
                    evnt.Context = string.Join(". ", text.InnerText.Split(". ").Take(2)); //Get just the first two sentences
                    eventCount++;
                }
            }
            else
            {
                NoEvents = true;
            }

            Serilog.Log.Information("Fetched {eventCount} events on day {date}", eventCount, Date?.ToShortDateString());
            DatesEvents.AddRange(events);
            await ViewMedia.GetDataMany(events.Select(m => m.Media).ToList());
        }

        //Get new information
        private async Task ChangeDay()
        {
            //get new information from DB
            DbTitle = string.Join("[^a-zA-Z]*", Title.Split('-'));
            var title = $"[^a-zA-Z]*{DbTitle}"; //use Title to get information
            var query = @"SELECT t.*, Max(e.Timestamp) As EventUpdated, Min(e.Timestamp) as Timestamp
                    From timelineinfo t
                    Left Join edithistory e on t.IdTimelineinfo = e.IdTimelineinfo
                    where t.date = @date and t.Title Regexp @title and Confirmed = 1;";
            var timelineinfo = await Data.LoadDataSingle<Timelineinfo, dynamic>(query, new {date = Date?.ToString("yyyy-MM-dd"), title});
            if (timelineinfo.IdTimelineinfo != 0) //If title matches title on that day
            {
                var mediaQuery = "SELECT *,(true) as Processed From media m where m.IdTimelineinfo = @id Order By m.Rank;";
                string misconductQuery = "SELECT * from misconducts WHERE IdTimelineinfo = @id";
                var officerQuery = "SELECT distinct o.* FROM misconducts m " +
                                        "Join officers o on m.IdOfficer = o.IdOfficer " +
                                        "WHERE m.IdTimelineinfo = @id;";
                var subjectQuery = "SELECT distinct o.* FROM misconducts m " +
                                        "Join subjects o on m.IdSubject = o.IdSubject " +
                                        "WHERE m.IdTimelineinfo = @id;";
                //get media, officers, and subjects using timelineinfo id
                List<ViewMedia> media = await Data.LoadData<ViewMedia, dynamic>(mediaQuery, new { id = timelineinfo.IdTimelineinfo });
                List<Misconducts> misconducts = await Data.LoadData<Misconducts, dynamic>(misconductQuery, new { id = timelineinfo.IdTimelineinfo });
                List<Officer> officers = await Data.LoadData<Officer, dynamic>(officerQuery, new { id = timelineinfo.IdTimelineinfo });
                List<Subject> subjects = await Data.LoadData<Subject, dynamic>(subjectQuery, new { id = timelineinfo.IdTimelineinfo });
                await ViewMedia.GetDataMany(media);
                officers.SetAgeFromDOB(timelineinfo.Date);
                subjects.SetAgeFromDOB(timelineinfo.Date);
                Event = new ViewEvent {
                    Data = timelineinfo,
                    Medias = media,
                    Misconducts = misconducts,
                    Officers = Mapper.Map<List<Officer>, List<ViewOfficer>>(officers), 
                    Subjects = Mapper.Map<List<Subject>, List<ViewSubject>>(subjects) 
                };
                EventExists = true;
                Serilog.Log.Information("Sucessfully fetched '{EventTitle}' event", timelineinfo.Title);
            }
            else
            {
                FakeEvent = true;
                Serilog.Log.Information("Could not fetch '{EventTitle}' event", Title);
            }

            this.StateHasChanged();
        }
    }
}