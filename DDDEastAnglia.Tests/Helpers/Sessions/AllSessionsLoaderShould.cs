using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Sessions
{
    [TestFixture]
    public sealed class AllSessionsLoaderShould
    {
        [Test]
        public void ThrowAnException_WhenConstructedWithANullContext()
        {
            Assert.Throws<ArgumentNullException>(() => new AllSessionsLoader(null));
        }

        [Test]
        public void ThrowAnException_WhenGivenANullProfile()
        {
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            var sessionsLoader = new AllSessionsLoader(sessionRepositoryFactory);
            Assert.Throws<ArgumentNullException>(() => sessionsLoader.LoadSessions(null));
        }

        [Test]
        public void OnlyReturnSessionsForTheSpecifiedSpeaker()
        {
            var sessionRepository = Substitute.For<ISessionRepository>();
            var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
            sessionRepositoryFactory.Create().Returns(sessionRepository);
            var sessionsLoader = new AllSessionsLoader(sessionRepositoryFactory);

            sessionsLoader.LoadSessions(new UserProfile {UserName = "bob"});

            sessionRepository.Received().GetSessionsSubmittedBy("bob");
        }
    }
}
