using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Areas.Admin.Models.Eventbrite;
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
                        TicketId = attendee.ticket_id.ToString(), 
                        FullName = fullName, 
                        TwitterHandle = twitterHandle,
                        FirstTimer = firstTimer
                    };

                model.Add(nameBadge);
            }

            return View(model);
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
