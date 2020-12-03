using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.RichTextEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;
using ThinBlueLieB.Helper.Extensions;
using ThinBlueLieB.Models;
using static DataAccessLibrary.Enums.MediaEnums;
using static ThinBlueLieB.Components.SimilarPeople;
using static ThinBlueLieB.Helper.Extensions.EnumExtensions;
using static ThinBlueLieB.Models.SubmitBase;
using static ThinBlueLieB.Searches.SearchClasses;

namespace ThinBlueLieB.Bases
{
    public class InformationBase : ComponentBase
    {
        public SubmitModel model = new SubmitModel()
        {
            Timelineinfos = new ViewTimelineinfo(),
            Medias = new List<ViewMedia> { 
                new ViewMedia { MediaType = MediaTypeEnum.Image, Blurb="Placeholder Image", ListIndex = 0 },
               new ViewMedia { MediaType = MediaTypeEnum.Video, Blurb="Placeholder Video",ListIndex = 1 },
                  new ViewMedia { MediaType = MediaTypeEnum.News, Blurb="Placeholder News", ListIndex = 2, SourceFrom = SourceFromEnum.Link }
            },
            Officers = new List<ViewOfficer> { new ViewOfficer { ListIndex = 0 } },
            Subjects = new List<ViewSubject> { new ViewSubject { ListIndex = 0 } }
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
        public DateTime MinDate { get; set; } = new DateTime(1776, 6, 4);
        public DateTime? DateValue { get; set; } = DateTime.Today;

        internal void SetSameAsSubject(SimilarPeopleModel person)
        {
            model.Subjects[person.PersonListIndex].SameAsId = person.IdPerson;
            model.Subjects[person.PersonListIndex].Age = person.Age == 0 ? null : person.Age;
            model.Subjects[person.PersonListIndex].Name = person.Name;
            model.Subjects[person.PersonListIndex].Sex = person.Sex;
            model.Subjects[person.PersonListIndex].Race = person.Race;
            SimilarSubjects[person.PersonListIndex] = new List<SimilarPersonGeneral>();
        }
        internal void SetSameAsOfficer(SimilarPeopleModel person)
        {
            model.Officers[person.PersonListIndex].SameAsId = person.IdPerson;
            model.Officers[person.PersonListIndex].Age = person.Age == 0? null : person.Age;
            model.Officers[person.PersonListIndex].Name = person.Name;
            model.Officers[person.PersonListIndex].Sex = person.Sex;
            model.Officers[person.PersonListIndex].Race = person.Race;
            SimilarOfficers[person.PersonListIndex] = new List<SimilarPersonGeneral>();
        }
        const int MaximumMedia = 20;
        internal void AddMedia(MediaTypeEnum mediaType)
        {
            //if the given media is below the maximum total
            if (model.Medias.Where(m => m.MediaType == mediaType).Count() < MaximumMedia)
            {
                if (mediaType != MediaTypeEnum.News)
                {
                    var newMediaItem = new ViewMedia { ListIndex = model.Medias.Count, MediaType = mediaType };
                    model.Medias.Add(newMediaItem);
                }
                else
                {
                    var newMediaItem = new ViewMedia { ListIndex = model.Medias.Count, MediaType = mediaType, SourceFrom = SourceFromEnum.Link };
                    model.Medias.Add(newMediaItem);
                }
             
            }           
        }

        const int MaximumSubject = 10;
        internal void AddSubject()
        {
            if (model.Subjects.Count < MaximumSubject)
            {
                var newSubjectItem = new ViewSubject { ListIndex = model.Subjects.Count };
                model.Subjects.Add(newSubjectItem);
                SimilarSubjects.Add(new List<SimilarPersonGeneral> { });
            }          
        }
        const int MaximumOfficer = 10;
        internal void AddOfficer()
        {
            if (model.Officers.Count < MaximumOfficer)
            {
                var newOfficerItem = new ViewOfficer { ListIndex = model.Officers.Count };
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
                model.Medias[Index].ListIndex = Index; //Set ListIndex to fit it's position
                model.Medias[Index - 1].ListIndex = Index - 1; //do above to item above
            }
        }

        internal void MoveMediaDown(int Index)
        {
            if (Index != model.Medias.Count - 1)
            {
                var item = model.Medias[Index]; //Get Card
                model.Medias.RemoveAt(Index); //Remove Card
                model.Medias.Insert(Index + 1, item); //Put Card in place below where it was
                model.Medias[Index].ListIndex = Index; //Set ListIndex to fit it's position
                model.Medias[Index + 1].ListIndex = Index + 1; //do above to item below
            }
        }

        internal void MoveMediaTop(int Index)
        {
            if (Index != 0)
            {
                var item = model.Medias[Index]; //Get Card
                model.Medias.RemoveAt(Index); //Remove Card
                model.Medias.Insert(0, item); //Put Card at Top
                model.Medias[Index].ListIndex = 0; //Set ListIndex to fit it's position
                for (int i = 0; i < model.Medias.Count; i++)
                {
                    model.Medias[i].ListIndex = i;
                }
            }
        }

        internal void DeleteMedia(int Index)
        {
            model.Medias.RemoveAt(Index);
            for (int i = 0; i < model.Medias.Count; i++)
            {
                model.Medias[i].ListIndex = i;
            }
        }

        internal void DeleteSubject(int Index)
        {
            model.Subjects.RemoveAt(Index);
            SimilarSubjects.RemoveAt(Index);
            //Resetting List Indexes
            for (int i = 0; i < model.Subjects.Count; i++)
            {
                model.Subjects[i].ListIndex = i;
            }

        }

        internal void DeleteOfficer(int Index)
        {
            model.Officers.RemoveAt(Index);
            SimilarOfficers.RemoveAt(Index);
            //Resetting List Indexes
            for (int i = 0; i < model.Officers.Count; i++)
            {
                model.Officers[i].ListIndex = i;
            }
        }

    }
}
