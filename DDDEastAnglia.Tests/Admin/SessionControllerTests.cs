﻿using System.Net;
using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Admin
{
    [TestFixture]
    public sealed class SessionControllerTests
    {
        [Test]
        public void Details_GetsTheCorrectSessionDetails()
        {
            const int sessionId = 123;
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            sessionRepositoryFactory.Create().Get(sessionId).Returns(new Session { SessionId = sessionId });
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);

            var actionResult = controller.Details(sessionId);

            var model = actionResult.GetViewModel<Session>();
            Assert.That(model.SessionId, Is.EqualTo(sessionId));
        }

        [Test]
        public void Details_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);

            var actionResult = controller.Details(123);

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Edit_GetsTheCorrectSessionDetails()
        {
            const int sessionId = 123;
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            sessionRepositoryFactory.Create().Get(sessionId).Returns(new Session { SessionId = sessionId });
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);

            var actionResult = controller.Edit(sessionId);

            var model = actionResult.GetViewModel<Session>();
            Assert.That(model.SessionId, Is.EqualTo(sessionId));
        }

        [Test]
        public void Edit_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);

            var actionResult = controller.Edit(123);

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Edit_SavesTheUserProfileCorrectly()
        {
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            sessionRepositoryFactory.Create().Returns(sessionRepository);
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);
            var session = new Session { Title = "A session", Abstract = "My session is about...", SpeakerUserName = "fred" };

            controller.Edit(session);

            sessionRepository.Received().UpdateSession(session);
        }

        [Test]
        public void Edit_DoesNotSaveTheUserProfile_WhenTheSubmittedDataIsInvalid()
        {
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            sessionRepositoryFactory.Create().Returns(sessionRepository);
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);
            controller.CreateModelStateError();

            controller.Edit(new Session());

            sessionRepository.DidNotReceive().UpdateSession(Arg.Any<Session>());
        }

        [Test]
        public void Delete_GetsTheCorrectUserDetails()
        {
            const int sessionId = 123;
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            sessionRepositoryFactory.Create().Get(sessionId).Returns(new Session { SessionId = sessionId });
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);

            var actionResult = controller.Delete(sessionId);

            var model = actionResult.GetViewModel<Session>();
            Assert.That(model.SessionId, Is.EqualTo(sessionId));
        }

        [Test]
        public void Delete_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);

            var actionResult = controller.Delete(123);

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void DeleteConfirmed_DeletesTheCorrectUser()
        {
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            sessionRepositoryFactory.Create().Returns(sessionRepository);
            var voteRepository = Substitute.For<IVoteRepository>();
            var controller = new SessionController(sessionRepositoryFactory, voteRepository);

            controller.DeleteConfirmed(123);

            sessionRepository.Received().DeleteSession(123);
        }
    }
}
