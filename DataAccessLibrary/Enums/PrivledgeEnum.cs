﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.Enums
{
    public class PrivledgeEnum
    {
        public enum Privledges
        {
            [Display(Name = "Create, Edit, and Flag Events")]
            CreateEditFlag = 1,
            [Display(Name = "Edit Officers and Subjects")]
            EditPerson = 50,
            [Display(Name = "Edit Notable Events")]
            EditNotable = 200,
            [Display(Name = "Review all Pending Edits")]
            ReviewAll = 300,
            [Display(Name = "Edits Auto-Verified")]
            AutoVerified = 500,
            [Display(Name = "Edit Locked Posts")]
            EditLocked = 650,
            [Display(Name = "Mark posts as Locked")]
            MarkLocked = 750,
            [Display(Name = "Self Verify Edits")]
            SelfVerifyEdits = 1000,
            [Display(Name = "Delete Events")]
            DeleteEvents = 2000,
            [Display(Name = "Full Permissions")]
            Moderator = 5000
        }
    }
}
