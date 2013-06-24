using System.Collections.Generic;
using System.Diagnostics;

namespace DDDEastAnglia.Areas.Admin.Models.Twitter
{
    public class ProfileList
    {
        public List<Profile> profiles{get;set;}
    }

    [DebuggerDisplay("{screen_name}")]
    public class Profile
    {
        public string screen_name{get;set;}
        public string profile_image_url{get;set;}
    }
}