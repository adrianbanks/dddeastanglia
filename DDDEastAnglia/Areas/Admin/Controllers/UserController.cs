using System;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly ISessionRepositoryFactory sessionRepositoryFactory;

        public UserController(IUserProfileRepository userProfileRepository, ISessionRepositoryFactory sessionRepositoryFactory)
        {
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            if (sessionRepositoryFactory == null)
            {
                throw new ArgumentNullException("sessionRepositoryFactory");
            }

            this.userProfileRepository = userProfileRepository;
            this.sessionRepositoryFactory = sessionRepositoryFactory;
        }

        public ActionResult Index()
        {
            var users = userProfileRepository.GetAllUserProfiles()
                                             .Select(CreateUserModel)
                                             .OrderBy(u => u.UserName).ToList();

            var sessionCountsPerUser = sessionRepositoryFactory.Create()
                                                               .GetAllSessions()
                                                               .GroupBy(s => s.SpeakerUserName)
                                                               .ToDictionary(g => g.Key, g => g.Count());

            foreach (var user in users)
            {
                int sessionCount;
                sessionCountsPerUser.TryGetValue(user.UserName, out sessionCount);
                user.SubmittedSessionCount = sessionCount;
            }

            return View(users);
        }

        public ActionResult Details(Guid id)
        {
            var userProfile = userProfileRepository.GetUserProfileById(id);

            if (userProfile == null)
            {
                return HttpNotFound();
            }

            return View(userProfile);
        }

        public ActionResult Edit(Guid id)
        {
            var userProfile = userProfileRepository.GetUserProfileById(id);
            return userProfile == null ? (ActionResult) HttpNotFound() : View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                userProfileRepository.UpdateUserProfile(userProfile);
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        public ActionResult Delete(Guid id)
        {
            var userProfile = userProfileRepository.GetUserProfileById(id);
            return userProfile == null ? (ActionResult) HttpNotFound() : View(userProfile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            userProfileRepository.DeleteUserProfile(id);
            return RedirectToAction("Index");
        }

        private static UserModel CreateUserModel(UserProfile profile)
        {
            return new UserModel
                {
                    Id = profile.Id,
                    UserId = profile.UserId,
                    UserName = profile.UserName,
                    Name = profile.Name,
                    EmailAddress = profile.EmailAddress,
                    MobilePhone = profile.MobilePhone,
                    WebsiteUrl = profile.WebsiteUrl,
                    TwitterHandle = profile.TwitterHandle,
                    Bio = profile.Bio,
                    NewSpeaker = profile.NewSpeaker,
                    GravatarUrl = profile.GravatarUrl()
                };
        }
    }
}
