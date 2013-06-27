using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Areas.Admin.Models.Eventbrite;
using DDDEastAnglia.Helpers;
using Newtonsoft.Json;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class NameBadgeController : Controller
    {
        public ActionResult Index()
        {
            string userKey = ConfigurationManager.AppSettings["EventbriteUserKey"];
            string appKey = ConfigurationManager.AppSettings["EventbriteAppKey"];
            long eventId = long.Parse(ConfigurationManager.AppSettings["EventbriteEventId"]);
            var eventbriteUrl = new EventbriteUrl(userKey, appKey, eventId);

            var json = new WebClient().DownloadString(eventbriteUrl.Url);
            var attendeeList = JsonConvert.DeserializeObject<AttendeeList>(json);

            var model = new List<NameBadge>();

            foreach (AttendeeContainer attendeeContainer in attendeeList.attendees)
            {
                var attendee = attendeeContainer.attendee;
                string fullName = GetFullName(attendee);
                var firstTimer = IsFirstTimeAtDDD(attendee);
                var twitterHandle = GetTwitterHandle(attendee);

                var nameBadge = new NameBadge
                    {
                        Id = attendee.id.ToString(),
                        TicketId = attendee.ticket_id.ToString(), 
                        FullName = fullName, 
                        TwitterHandle = twitterHandle,
                        FirstTimer = firstTimer
                    };

                model.Add(nameBadge);
            }

            var usersWithTwitterHandles = model.Where(b => !string.IsNullOrWhiteSpace(b.TwitterHandle))
                                               .ToDictionary(b => b.TwitterHandle.ToUpper(), b => b);
            var twitterHandles = usersWithTwitterHandles.Values.Select(b => b.TwitterHandle).ToList();
            var profiles = new TwitterAPI().GetProfiles(twitterHandles);

            foreach (var profile in profiles)
            {
                NameBadge badge;

                if (usersWithTwitterHandles.TryGetValue(profile.screen_name.ToUpper(), out badge))
                {
                    badge.TwitterImageUrl = profile.profile_image_url;
                }
            }

            this.Session["BadgeInfo"] = model;

            return View(model);
        }

        public FileContentResult Badge(int id)
        {
            var badges = (List<NameBadge>) this.Session["BadgeInfo"];

            if (badges != null)
            {
                var badgeMap = badges.ToDictionary(b => b.Id, b => b);
                NameBadge badge;

                if (badgeMap.TryGetValue(id.ToString(), out badge))
                {
                    var badgeImage = new Bitmap(189, 112);

                    using (var graphics = Graphics.FromImage(badgeImage))
                    {
                        graphics.FillRectangle(Brushes.White, 0, 0, 189, 112);
                        graphics.DrawRectangle(new Pen(Color.Black), 0, 0, 188, 111);
                        graphics.DrawString(badge.FullName, new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Black), 10, 10);
                    }

                    var stream = new MemoryStream();
                    badgeImage.Save(stream, ImageFormat.Jpeg);
                    var bytes = stream.ToArray();

                    return File(bytes, "image/jpeg");
                }
            }

            return null;
        }

        private static string GetFullName(Attendee attendee)
        {
            string firstName = attendee.first_name;
            string lastName = attendee.last_name;

            if (firstName.EndsWith(lastName))
            {
                firstName = firstName.Substring(0, firstName.IndexOf(lastName));
            }

            return string.Format("{0} {1}", firstName, lastName);
        }

        private static bool IsFirstTimeAtDDD(Attendee attendee)
        {
            long firstTimeAtDDDQuestionId = long.Parse(ConfigurationManager.AppSettings["EventbriteFirstTimeAtDDDQuestionId"]);
            var firstTimeAtDDDItem = attendee.answers.SingleOrDefault(a => a.answer.question_id == firstTimeAtDDDQuestionId);
            bool firstTimer = string.Equals(firstTimeAtDDDItem.answer.answer_text, "Yes", StringComparison.InvariantCultureIgnoreCase);
            return firstTimer;
        }

        private static string GetTwitterHandle(Attendee attendee)
        {
            long twitterHandleQuestionId = long.Parse(ConfigurationManager.AppSettings["EventbriteTwitterHandleQuestionId"]);
            var twitterHandleItem = attendee.answers.SingleOrDefault(a => a.answer.question_id == twitterHandleQuestionId);
            string twitterHandle = string.Empty;

            if (twitterHandleItem != null)
            {
                twitterHandle = twitterHandleItem.answer.answer_text;

                if (twitterHandle.StartsWith("@"))
                {
                    twitterHandle = twitterHandle.Substring(1);
                }
            }
     
            return twitterHandle;
        }
    }
}
