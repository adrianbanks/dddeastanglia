using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.DataAccess
{
    [TestFixture]
    public sealed class SpeakerRepositoryTests
    {
        [Test]
        public void GetAllSpeakerProfiles_DoesNotReturnAUserWhoHasNotSubmittedASession()
        {
            var sessionRepositoryFactory = new SessionRepositoryFactoryBuilder().Build();
            var userProfileRepository = new UserProfileRepositoryBuilder().WithUser("fred").Build();
            var speakerRepository = new SpeakerRepository(sessionRepositoryFactory, userProfileRepository);

            var allSpeakers = speakerRepository.GetAllSpeakerProfiles();

            Assert.That(allSpeakers, Is.Empty);
        }

        [Test]
        public void GetAllSpeakerProfiles_ReturnsAUserWhoHasSubmittedASession()
        {
            var sessionRepositoryFactory = new SessionRepositoryFactoryBuilder().WithSessionSubmittedBy("fred").Build();
            var userProfileRepository = new UserProfileRepositoryBuilder().WithUser("fred").Build();
            var speakerRepository = new SpeakerRepository(sessionRepositoryFactory, userProfileRepository);

            var allSpeakers = speakerRepository.GetAllSpeakerProfiles();

            Assert.That(allSpeakers.Select(s => s.UserName), Is.EqualTo(new[] {"fred"}));
        }

        [Test]
        public void GetAllSpeakerProfiles_ReturnsOnlyUsersWhoHaveSubmittedSessions()
        {
            var sessionRepositoryFactory = new SessionRepositoryFactoryBuilder().WithSessionSubmittedBy("fred").Build();
            var userProfileRepository = new UserProfileRepositoryBuilder().WithUser("fred").WithUser("bob").Build();
            var speakerRepository = new SpeakerRepository(sessionRepositoryFactory, userProfileRepository);

            var allSpeakers = speakerRepository.GetAllSpeakerProfiles();

            Assert.That(allSpeakers.Select(s => s.UserName), Is.EqualTo(new[] {"fred"}));
        }

        private class SessionRepositoryFactoryBuilder
        {
            private readonly List<Session> sessions = new List<Session>();

            public SessionRepositoryFactoryBuilder WithSessionSubmittedBy(string username)
            {
                sessions.Add(new Session {SpeakerUserName = username});
                return this;
            }

            public ISessionRepositoryFactory Build()
            {
                var sessionRepository = Substitute.For<ISessionRepository>();
                sessionRepository.GetAllSessions().Returns(sessions);

                var sessionRepositoryFactory = Substitute.For<ISessionRepositoryFactory>();
                sessionRepositoryFactory.Create().Returns(sessionRepository);

                return sessionRepositoryFactory;
            }
        }

        private class UserProfileRepositoryBuilder
        {
            private readonly List<UserProfile> userProfiles = new List<UserProfile>();

            public UserProfileRepositoryBuilder WithUser(string username)
            {
                userProfiles.Add(new UserProfile {UserName = username});
                return this;
            }

            public IUserProfileRepository Build()
            {
                var userProfileRepository = Substitute.For<IUserProfileRepository>();
                userProfileRepository.GetAllUserProfiles().Returns(userProfiles);
                return userProfileRepository;
            }
        }
    }
}
