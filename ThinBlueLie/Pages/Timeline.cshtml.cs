﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThinBlue;
using MySql.Data.MySqlClient;
using DataAccessLibrary;
using Microsoft.EntityFrameworkCore;

namespace ThinBlueLie.Pages
{
    public class TimelineModel
    {
        public TimelineModel()
        {
        }
        [BindProperty]
        public Flagged Flags { get; set; }

        public IList<Timelineinfo> Timelineinfos { get; set; }      
    }
}