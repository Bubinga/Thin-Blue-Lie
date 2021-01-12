using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.RichTextEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Helper;
using ThinBlueLie.Helper.Extensions;
using ThinBlueLie.Models;
using static DataAccessLibrary.Enums.MediaEnums;
using static ThinBlueLie.Components.SimilarPeople;
using static ThinBlueLie.Helper.Extensions.EnumExtensions;
using static ThinBlueLie.Models.SubmitBase;
using static ThinBlueLie.Searches.SearchClasses;

namespace ThinBlueLie.Bases
{
    public class InformationBase : ComponentBase
    {
        public SubmitModel model = new SubmitModel()
        {
            Timelineinfos = new ViewTimelineinfo(),
            Medias = new List<ViewMedia> { 
                new ViewMedia { MediaType = MediaTypeEnum.Image, Blurb="Placeholder Image", Rank = 0 },
               new ViewMedia { MediaType = MediaTypeEnum.Video, Blurb="Placeholder Video",Rank = 1 },
                  new ViewMedia { MediaType = MediaTypeEnum.News, Blurb="Placeholder News", Rank = 2, SourceFrom = SourceFromEnum.Link }
            },
            Officers = new List<ViewOfficer> { new ViewOfficer { Rank = 0 } },
            Subjects = new List<ViewSubject> { new ViewSubject { Rank = 0 } }
        };

        public List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Bold },
            new ToolbarItemModel() { Command = ToolbarCommand.Italic },
            new ToolbarItemModel() { Command = ToolbarCommand.Underline },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Formats },
            new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
            new ToolbarItemModel() { Command = ToolbarCommand.OrderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.UnorderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.Outdent },
            new ToolbarItemModel() { Command = ToolbarCommand.Indent },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
         // new ToolbarItemModel() { Command = ToolbarCommand.Image }, TODO add support for context inline images
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.ClearFormat },
            new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo }
         };

        public IEnumerable<ListItem> States = GetEnumDisplayNames<TimelineinfoEnums.StateEnum>();

        public DateTime Today { get; set; } =  DateTime.Today;
        public static DateTime MinDate = new DateTime(1776, 7, 4);
        public DateTime? DateValue { get; set; } = DateTime.Today;

        internal void SetSameAsSubject(SimilarPeopleModel person)
        {
            model.Subjects[person.PersonRank].SameAsId = person.IdPerson;
            model.Subjects[person.PersonRank].Age = person.Age == 0 ? null : person.Age;
            model.Subjects[person.PersonRank].Name = person.Name;
            model.Subjects[person.PersonRank].Sex = person.Sex;
            model.Subjects[person.PersonRank].Race = person.Race;
            SimilarSubjects[person.PersonRank] = new List<SimilarPersonGeneral>();
        }
        internal void SetSameAsOfficer(SimilarPeopleModel person)
        {
            model.Officers[person.PersonRank].SameAsId = person.IdPerson;
            model.Officers[person.PersonRank].Age = person.Age == 0? null : person.Age;
            model.Officers[person.PersonRank].Name = person.Name;
            model.Officers[person.PersonRank].Sex = person.Sex;
            model.Officers[person.PersonRank].Race = person.Race;
            SimilarOfficers[person.PersonRank] = new List<SimilarPersonGeneral>();
        }
        internal void AddMedia(MediaTypeEnum mediaType)
        {
            //if the given media is below the maximum total
            if (model.Medias.Where(m => m.MediaType == mediaType).Count() < SubmitModel.MaximumMedia)
            {
                if (mediaType != MediaTypeEnum.News)
                {
                    var newMediaItem = new ViewMedia { Rank = model.Medias.Count, MediaType = mediaType };
                    model.Medias.Add(newMediaItem);
                }
                else
                {
                    var newMediaItem = new ViewMedia { Rank = model.Medias.Count, MediaType = mediaType, SourceFrom = SourceFromEnum.Link };
                    model.Medias.Add(newMediaItem);
                }
             
            }           
        }

        internal void AddSubject()
        {
            if (model.Subjects.Count < SubmitModel.MaximumSubjects)
            {
                var newSubjectItem = new ViewSubject { Rank = model.Subjects.Count };
                model.Subjects.Add(newSubjectItem);
                SimilarSubjects.Add(new List<SimilarPersonGeneral> { });
            }          
        }
        internal void AddOfficer()
        {
            if (model.Officers.Count < SubmitModel.MaximumOfficers)
            {
                var newOfficerItem = new ViewOfficer { Rank = model.Officers.Count };
                model.Officers.Add(newOfficerItem);
                SimilarOfficers.Add(new List<SimilarPersonGeneral> { });
            }           
        }

        public List<List<SimilarPersonGeneral>> SimilarSubjects { get; set; } = new List<List<SimilarPersonGeneral>>();
        internal async void SuggestSubjects(SimilarPersonCallback personCallback)
        {
            SearchesSubmit searchesSubmit = new SearchesSubmit();
            //TODO don't do suggest if name is too short or something
            var similarSubject = await searchesSubmit.SearchSubject(personCallback.Name);
            SimilarSubjects[personCallback.Index] = similarSubject;
            this.StateHasChanged();
        }

        public List<List<SimilarPersonGeneral>> SimilarOfficers { get; set; } = new List<List<SimilarPersonGeneral>>();
        internal async void SuggestOfficers(SimilarPersonCallback personCallback)
        {
            SearchesSubmit searchesSubmit = new SearchesSubmit();
            //TODO don't do suggest if name is too short or something
            var similarOfficer = await searchesSubmit.SearchOfficer(personCallback.Name);
            SimilarOfficers[personCallback.Index] = similarOfficer;
            this.StateHasChanged();
        }

        internal void MoveMediaUp(int Index)
        {
            if (Index != 0)
            {
                var item = model.Medias[Index]; //Get Card
                model.Medias.RemoveAt(Index); //Remove Card
                model.Medias.Insert(Index - 1, item); //Put Card in place above where it was
                model.Medias[Index].Rank = Index; //Set Rank to fit it's position
                model.Medias[Index - 1].Rank = Index - 1; //do above to item above
            }
        }

        internal void MoveMediaDown(int Index)
        {
            if (Index != model.Medias.Count - 1)
            {
                var item = model.Medias[Index]; //Get Card
                model.Medias.RemoveAt(Index); //Remove Card
                model.Medias.Insert(Index + 1, item); //Put Card in place below where it was
                model.Medias[Index].Rank = Index; //Set Rank to fit it's position
                model.Medias[Index + 1].Rank = Index + 1; //do above to item below
            }
        }

        internal void MoveMediaTop(int Index)
        {
            if (Index != 0)
            {
                var item = model.Medias[Index]; //Get Card
                model.Medias.RemoveAt(Index); //Remove Card
                model.Medias.Insert(0, item); //Put Card at Top
                model.Medias[Index].Rank = 0; //Set Rank to fit it's position
                for (int i = 0; i < model.Medias.Count; i++)
                {
                    model.Medias[i].Rank = i;
                }
            }
        }

        internal void DeleteMedia(int Index)
        {
            model.Medias.RemoveAt(Index);
            for (int i = 0; i < model.Medias.Count; i++)
            {
                model.Medias[i].Rank = i;
            }
        }

        internal void DeleteSubject(int Index)
        {
            model.Subjects.RemoveAt(Index);
            SimilarSubjects.RemoveAt(Index);
            //Resetting List Indexes
            for (int i = 0; i < model.Subjects.Count; i++)
            {
                model.Subjects[i].Rank = i;
            }

        }

        internal void DeleteOfficer(int Index)
        {
            model.Officers.RemoveAt(Index);
            SimilarOfficers.RemoveAt(Index);
            //Resetting List Indexes
            for (int i = 0; i < model.Officers.Count; i++)
            {
                model.Officers[i].Rank = i;
            }
        }

    }
}
