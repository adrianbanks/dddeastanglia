using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DDDEastAnglia.Models
{
    [DebuggerDisplay("{Title}")]
    public class Session
    {
        [Key]
        public int SessionId { get; set; }

        [Required]
        [Display(Name = "title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "abstract")]
        public string Abstract { get; set; }

        [Display(Name = "submitted by")]
        public string SpeakerUserName { get; set; }

        public int Votes { get; set; }

        public int ConferenceId { get; set; }

        public DateTimeOffset? SubmittedAt { get; set; }
    }
}
