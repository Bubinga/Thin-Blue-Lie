using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.ViewModels;

namespace ThinBlueLie.Models.View_Models
{
    public class EditReviewModel
    {
        public EditReviewSegment Old { get; set; }
        public EditReviewSegment New { get; set; }

        public class EditReviewSegment
        {
            public Timelineinfo Data { get; set; }
            public List<Media> Medias { get; set; }
            public List<DBOfficer> Officers { get; set; }
            public List<DBSubject> Subjects { get; set; }
            public Officers OfficerPerson { get; set; }
            public Subjects SubjectPerson { get; set; }
        }
    }
}
