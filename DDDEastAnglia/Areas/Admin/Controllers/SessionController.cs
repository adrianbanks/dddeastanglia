using System;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SessionController : Controller
    {
        private readonly SessionRepositoryFactory sessionRepositoryFactory;
        private readonly IVoteRepository voteRepository;

        public SessionController(SessionRepositoryFactory sessionRepositoryFactory, IVoteRepository voteRepository)
        {
            if (sessionRepositoryFactory == null)
            {
                throw new ArgumentNullException("sessionRepositoryFactory");
            }

            if (voteRepository == null)
            {
                throw new ArgumentNullException("voteRepository");
            }

            this.sessionRepositoryFactory = sessionRepositoryFactory;
            this.sessionRepositoryFactory = sessionRepositoryFactory;
            this.voteRepository = voteRepository;
        }

        public ActionResult Index()
        {
            var votesGroupedBySessionId = voteRepository.GetAllVotes().GroupBy(v => v.SessionId).ToDictionary(g => g.Key, g => g.Count());
            var sessions = sessionRepositoryFactory.Create().GetAllSessions().ToList();

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
            var session = sessionRepositoryFactory.Create().Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        public ActionResult Edit(int id)
        {
            var session = sessionRepositoryFactory.Create().Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Session session)
        {
            if (ModelState.IsValid)
            {
                sessionRepositoryFactory.Create().UpdateSession(session);
                return RedirectToAction("Index");
            }

            return View(session);
        }

        public ActionResult Delete(int id = 0)
        {
            var session = sessionRepositoryFactory.Create().Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sessionRepositoryFactory.Create().DeleteSession(id);
            return RedirectToAction("Index");
        }
    }
}
