using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SessionController : Controller
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IVoteRepository voteRepository;
        private readonly SessionStatsThing sessionStatsThing;


        public SessionController(ISessionRepository sessionRepository, IVoteRepository voteRepository, SessionStatsThing sessionStatsThing)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException("sessionRepository");
            }

            if (voteRepository == null)
            {
                throw new ArgumentNullException("voteRepository");
            }
            
            if (sessionStatsThing == null)
            {
                throw new ArgumentNullException("sessionStatsThing");
            }
            
            this.sessionRepository = sessionRepository;
            this.voteRepository = voteRepository;
            this.sessionStatsThing = sessionStatsThing;
        }

        public ActionResult Index()
        {
            var votesGroupedBySessionId = voteRepository.GetAllVotes().GroupBy(v => v.SessionId).ToDictionary(g => g.Key, g => g.Count());
            var sessions = sessionRepository.GetAllSessions().ToList();

            foreach (var session in sessions)
            {
                int voteCount;
                votesGroupedBySessionId.TryGetValue(session.SessionId, out voteCount);
                session.Votes = voteCount;
            }

            var orderedSessions = sessions.OrderByDescending(s => s.Votes).ToList();
            return View(orderedSessions);
        }

        public ActionResult Details(int id)
        {
            var session = sessionRepository.Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        public ActionResult Edit(int id)
        {
            var session = sessionRepository.Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Session session)
        {
            if (ModelState.IsValid)
            {
                sessionRepository.UpdateSession(session);
                return RedirectToAction("Index");
            }

            return View(session);
        }

        public ActionResult Delete(int id = 0)
        {
            var session = sessionRepository.Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sessionRepository.DeleteSession(id);
            return RedirectToAction("Index");
        }

        public ActionResult SubmissionsStats()
        {
            var stats = sessionStatsThing.GetStats();
            return View(stats);
        }

        public class SessionStatsThing
        {
            private readonly ICalendarItemRepository calendarItemRepository;
            private readonly ISessionRepository sessionRepository;

            public SessionStatsThing(ICalendarItemRepository calendarItemRepository, ISessionRepository sessionRepository)
            {
                if (calendarItemRepository == null)
                {
                    throw new ArgumentNullException("calendarItemRepository");
                }

                if (sessionRepository == null)
                {
                    throw new ArgumentNullException("sessionRepository");
                }
                
                this.calendarItemRepository = calendarItemRepository;
                this.sessionRepository = sessionRepository;
            }

            public SessionStatsViewModel GetStats()
            {
                var dateTimeSubmissionModels = GetData();
                var submissionsPerDay = ToChartData(dateTimeSubmissionModels);
                var cumulativeSubmissions = WorkOutCumulativeSubmissions(dateTimeSubmissionModels);
                return new SessionStatsViewModel
                {
                    DayByDay = submissionsPerDay,
                    Cumulative = cumulativeSubmissions
                };
            }

            private IList<DateTimeSubmissionModel> GetData()
            {
                var sessionSbumission = calendarItemRepository.GetFromType(CalendarEntryType.SessionSubmission);

                var dateToCountDictionary = sessionRepository.GetAllSessions()
                                                .Where(s => s.SubmittedAt != null)
                                                .GroupBy(s => s.SubmittedAt)
                                                .ToDictionary(g => g.Key, g => g.Count());

                var submissionStartDate = sessionSbumission.StartDate.Date;
                var submissionEndDate = sessionSbumission.EndDate.Value.Date;
                var dateTimeSubmissionModels = new List<DateTimeSubmissionModel>();

                for (var day = submissionStartDate.Date; day <= submissionEndDate.Date; day = day.AddDays(1))
                {
                    int count;
                    dateToCountDictionary.TryGetValue(day, out count);
                    var model = new DateTimeSubmissionModel
                        {
                            Date = day,
                            SubmissionCount = count
                        };
                    dateTimeSubmissionModels.Add(model);
                }

                return dateTimeSubmissionModels;
            }

            private long[][] WorkOutCumulativeSubmissions(IEnumerable<DateTimeSubmissionModel> votesPerDay)
            {
                int totalVotes = 0;
                var cumulativeVotesPerDay = new List<DateTimeSubmissionModel>();

                foreach (var dateTimeVoteModel in votesPerDay)
                {
                    totalVotes += dateTimeVoteModel.SubmissionCount;
                    var model = new DateTimeSubmissionModel { Date = dateTimeVoteModel.Date, SubmissionCount = totalVotes };
                    cumulativeVotesPerDay.Add(model);
                }

                var cumulativeVotesPerDayData = ToChartData(cumulativeVotesPerDay);
                return cumulativeVotesPerDayData;
            }

            private long[][] ToChartData(IList<DateTimeSubmissionModel> voteData)
            {
                long[][] chartData = new long[voteData.Count][];

                foreach (var item in voteData.Select((v, i) => new { Index = i, Vote = v }))
                {
                    var vote = item.Vote;
                    long javascriptTimestamp = vote.Date.GetJavascriptTimestamp();
                    chartData[item.Index] = new[] { javascriptTimestamp, vote.SubmissionCount };
                }

                return chartData;
            }

            private class DateTimeSubmissionModel
            {
                public DateTime Date { get; set; }
                public int SubmissionCount { get; set; }
            }
        }
    }
}
