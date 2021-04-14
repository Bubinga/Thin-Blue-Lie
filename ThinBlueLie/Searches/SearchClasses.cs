using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;

namespace ThinBlueLie.Searches
{
    public class SearchClasses
    {
        public class FirstLoadEditHistory
        {
            public int IdEditHistory { get; set; }
            public int? IdTimelineinfo { get; set; }
            public short? Vote { get; set; }
            public byte IsNewEvent { get; set; }
            public int SubmittedBy { get; set; }
        }
        public class FirstLoadOfficer
        {
            public int IdOfficer { get; set; }
            public string Name { get; set; }
        }
        public class FirstLoadSubject
        {
            public int IdSubject { get; set; }
            public string Name { get; set; }
        }

        public class NameScore
        {
            public int Id { get; set; }
            //public string Name { get; set; }
            public double Score { get; set; }
            //public int MiddleNameCount { get; set; }
        }
        public class SimilarPerson : CommonPerson
        {
            public class SimilarPersonEvents
            {
                public int IdTimelineinfo { get; set; }
                public DateTime Date { get; set; }
                public byte State { get; set; }
                public string City { get; set; }
            }
            public List<SimilarPersonEvents> Events { get; set; }

        }
        public class SimilarOfficer : SimilarPerson
        {
            public int IdOfficer { get; set; }
        }
        public class SimilarSubject : SimilarPerson
        {
            public int IdSubject { get; set; }
        }

        public class SimilarPersonGeneral : SimilarPerson
        {
            public int IdPerson { get; set; }
        }

    }
}
