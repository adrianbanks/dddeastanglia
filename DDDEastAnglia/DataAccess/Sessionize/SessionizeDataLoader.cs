using System;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace DDDEastAnglia.DataAccess.Sessionize
{
    public sealed class SessionizeDataLoader
    {
        public Conference Load(string conferenceId)
        {
            var client = new WebClient();
            var json = client.DownloadString($"https://sessionize.com/api/v2/{conferenceId}/view/all");
            return JsonConvert.DeserializeObject<Conference>(json);
        }

        public class Conference
        {
            public Speaker[] Speakers { get; set; }
            public Session[] Sessions { get; set; }
        }

        [DebuggerDisplay("{Title}")]
        public class Session
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public Guid[] Speakers { get; set; }
        }

        [DebuggerDisplay("{FullName}")]
        public class Speaker
        {
            public Guid Id { get; set; }
            public string FullName { get; set; }
            public string ProfilePicture { get; set; }
            public string Bio { get; set; }
            public string TagLine{ get; set; }
            public Link[] Links { get; set; }
            public int[] Sessions { get; set; }
        }

        [DebuggerDisplay("{Title} - {Url}")]
        public class Link
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
    }
}
