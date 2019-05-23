using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using DDDEastAnglia.Mvc.Attributes;
using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly ISpeakerRepository speakerRepository;
        private readonly SessionRepositoryFactory sessionRepositoryFactory;
        private readonly ISessionSorter sessionSorter;

        public SessionController(IConferenceLoader conferenceLoader, ISpeakerRepository speakerRepository, SessionRepositoryFactory sessionRepositoryFactory, ISessionSorter sorter)
        {
            this.conferenceLoader = conferenceLoader;
            this.speakerRepository = speakerRepository;
            this.sessionRepositoryFactory = sessionRepositoryFactory;
            sessionSorter = sorter;
        }

        [AllowAnonymous]
        [AllowCrossSiteJson]
        public ActionResult Index()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanShowSessions())
            {
                return HttpNotFound();
            }

            var speakersLookup = speakerRepository.GetAllSpeakerProfiles().ToDictionary(p => p.UserName, p => p);
            var sessions = sessionRepositoryFactory.Create().GetAllSessions();

            var allSessions = new List<SessionDisplayModel>();
            var showSpeaker = conference.CanShowSpeakers();

            foreach (var session in sessions)
            {
                var profile = speakersLookup[session.SpeakerUserName];
                var displayModel = CreateDisplayModel(session, profile, showSpeaker);
                allSessions.Add(displayModel);
            }

            sessionSorter.SortSessions(conference, allSessions);

            return View(new SessionIndexModel
                        {
                            Sessions = allSessions,
                            IsOpenForSubmission = conference.CanSubmit(),
                            IsOpenForVoting = conference.CanVote()
                        });
        }

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            var session = sessionRepositoryFactory.Create().Get(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            var conference = conferenceLoader.LoadConference();
            var showSpeaker = conference.CanShowSpeakers();

            var userProfile = speakerRepository.GetProfileByUserName(session.SpeakerUserName);
            var displayModel = CreateDisplayModel(session, userProfile, showSpeaker);

            return View(displayModel);
        }

        public ActionResult Create()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanSubmit())
            {
                return RedirectToAction("Index");
            }

            if (User == null)
            {
                return RedirectToAction("Index");
            }

            var userProfile = speakerRepository.GetProfileByUserName(User.Identity.Name);

            if (userProfile == null)
            {
                return RedirectToAction("Index");
            }

            return View(new Session {SpeakerUserName = userProfile.UserName, ConferenceId = conference.Id});
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Votes")] Session session)
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanSubmit())
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)
            {
                session.SubmittedAt = DateTimeOffset.UtcNow;
                var addedSession = sessionRepositoryFactory.Create().AddSession(session);
                return RedirectToAction("Details", new {id = addedSession.SessionId});
            }

            return View(session);
        }

        [UserNameFilter("userName")]
        public ActionResult Edit(string userName, int id = 0)
        {
            Session session = sessionRepositoryFactory.Create().Get(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            if (UserDoesNotOwnSession(userName, session))
            {
                return new HttpUnauthorizedResult();
            }

            return View(session);
        }

        [HttpPost]
        [UserNameFilter("userName")]
        public ActionResult Edit(string userName, [Bind(Exclude = "Votes")] Session session)
        {
            var loadedSession = sessionRepositoryFactory.Create().Get(session.SessionId);

            if (UserDoesNotOwnSession(userName, loadedSession))
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)
            {
                sessionRepositoryFactory.Create().UpdateSession(session);
                return RedirectToAction("Index");
            }

            return View(session);
        }

        [UserNameFilter("userName")]
        public ActionResult Delete(string userName, int id = 0)
        {
            Session session = sessionRepositoryFactory.Create().Get(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            if (UserDoesNotOwnSession(userName, session))
            {
                return new HttpUnauthorizedResult();
            }

            var conference = conferenceLoader.LoadConference();
            var showSpeaker = conference.CanShowSpeakers();

            var userProfile = speakerRepository.GetProfileByUserName(session.SpeakerUserName);
            var displayModel = CreateDisplayModel(session, userProfile, showSpeaker);

            return View(displayModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [UserNameFilter("userName")]
        public ActionResult DeleteConfirmed(string userName, int id)
        {
            Session session = sessionRepositoryFactory.Create().Get(id);

            if (UserDoesNotOwnSession(userName, session))
            {
                return new HttpUnauthorizedResult();
            }

            sessionRepositoryFactory.Create().DeleteSession(id);
            return RedirectToAction("Index");
        }

        private SessionDisplayModel CreateDisplayModel(Session session, SpeakerProfile profile, bool showSpeaker)
        {
            var isUsersSession = Request.IsAuthenticated && session.SpeakerUserName == User.Identity.Name;

            var displayModel = new SessionDisplayModel
                {
                    SessionId = session.SessionId,
                    SessionTitle = session.Title,
                    SessionAbstract = session.Abstract,

                    Speakers = new List<SessionSpeakerModel>()
                    {
                        new SessionSpeakerModel
                        {
                            SpeakerId = profile.UserId,
                            SpeakerName = profile.Name,
                            SpeakerUserName = session.SpeakerUserName,
                            SpeakerGravatarUrl = profile.GravatarUrl(),
                        }
                    },

                    IsUsersSession = isUsersSession,
                    ShowSpeaker = showSpeaker
                };
            return displayModel;
        }

        private bool UserDoesNotOwnSession(string userName, Session session)
        {
            return session.SpeakerUserName != userName;
        }
    }
}
