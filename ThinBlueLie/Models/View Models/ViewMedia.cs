﻿using Microsoft.AspNetCore.Http;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using Syncfusion.Blazor.Inputs.Internal;
using DataAccessLibrary.Enums;

namespace ThinBlueLieB.Models
{
    public class ViewMedia
    {
        public int IdMedia { get; set; }
        public int IdTimelineinfo { get; set; }
        public int? SubmittedBy { get; set; }
        public int ListIndex { get; set; } //becomes Rank
        [Required]
        public MediaEnums.MediaTypeEnum? MediaType { get; set; }
        [Url]
        public string SourcePath { get; set; } //For linked image 
        public UploadFiles Source { get; set; } //For uploaded image file         
        [Required]
        public byte Gore { get; set; }
        [Required(ErrorMessage = "Select where the Media is from")]
        public MediaEnums.SourceFromEnum? SourceFrom { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Blurb { get; set; }
        [MaxLength(100, ErrorMessage = "Please type less than 100 characters")]
        public string Credit { get; set; }
    }
}