using System;
using System.Net;
using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Admin
{
    [TestFixture]
    public class UserControllerTests
    {
        private IUserProfileRepository userProfileRepository;

        [Test]
        public void Details_GetsTheCorrectUserDetails()
        {
            var userId = new Guid("e9d651a8-d638-4aba-b584-b9b5772d3b2f");
            var controller = CreateController();

            controller.Details(userId);

            userProfileRepository.Received().GetUserProfileById(userId);
        }

        [Test]
        public void Details_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var controller = CreateController();

            var actionResult = controller.Details(Guid.NewGuid());

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Edit_GetsTheCorrectUserDetails()
        {
            var userId = new Guid("e9d651a8-d638-4aba-b584-b9b5772d3b2f");
            var controller = CreateController();

            controller.Edit(userId);

            userProfileRepository.Received().GetUserProfileById(userId);
        }

        [Test]
        public void Edit_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var controller = CreateController();

            var actionResult = controller.Edit(Guid.NewGuid());

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Edit_SavesTheUserProfileCorrectly()
        {
            var controller = CreateController();
            var userProfile = new UserProfile { UserName = "fred", Name = "Fred Bloggs", EmailAddress = "fred@example.com" };

            controller.Edit(userProfile);

            userProfileRepository.Received().UpdateUserProfile(userProfile);
        }

        [Test]
        public void Edit_DoesNotSaveTheUserProfile_WhenTheSubmittedDataIsInvalid()
        {
            var controller = CreateController();
            controller.CreateModelStateError();

            controller.Edit(new UserProfile());

            userProfileRepository.DidNotReceive().UpdateUserProfile(Arg.Any<UserProfile>());
        }

        [Test]
        public void Delete_GetsTheCorrectUserDetails()
        {
            var userId = new Guid("e9d651a8-d638-4aba-b584-b9b5772d3b2f");
            var controller = CreateController();

            controller.Delete(userId);

            userProfileRepository.Received().GetUserProfileById(userId);
        }

        [Test]
        public void Delete_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var controller = CreateController();

            var actionResult = controller.Delete(Guid.NewGuid());

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void DeleteConfirmed_DeletesTheCorrectUser()
        {
            var userId = new Guid("e9d651a8-d638-4aba-b584-b9b5772d3b2f");
            var controller = CreateController();

            controller.DeleteConfirmed(userId);

            userProfileRepository.Received().DeleteUserProfile(userId);
        }

        private UserController CreateController()
        {
            userProfileRepository = Substitute.For<IUserProfileRepository>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            return new UserController(userProfileRepository, sessionRepository);
        }
    }
}
