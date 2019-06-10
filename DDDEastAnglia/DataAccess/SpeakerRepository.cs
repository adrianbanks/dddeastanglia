using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public class SpeakerRepository : ISpeakerRepository
    {
        private readonly ISessionRepositoryFactory sessionRepositoryFactory;
        private readonly IUserProfileRepository userProfileRepository;

        public SpeakerRepository(ISessionRepositoryFactory sessionRepositoryFactory, IUserProfileRepository userProfileRepository)
        {
            if (sessionRepositoryFactory == null)
            {
                throw new ArgumentNullException("sessionRepositoryFactory");
            }

            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            this.sessionRepositoryFactory = sessionRepositoryFactory;
            this.userProfileRepository = userProfileRepository;
        }

        public IEnumerable<SpeakerProfile> GetAllSpeakerProfiles()
        {
            var allSessions = sessionRepositoryFactory.Create().GetAllSessions();
            var sessionCountGroupedBySpeaker = allSessions.GroupBy(s => s.SpeakerUserName)
                                                          .Where(g => g.Any())
                                                          .ToDictionary(g => g.Key, g => g.Count());

            var usersWithSessions = new List<SpeakerProfile>();
            var allUserProfiles = userProfileRepository.GetAllUserProfiles();

            foreach (UserProfile userProfile in allUserProfiles)
            {
                int sessionCount;

                if (sessionCountGroupedBySpeaker.TryGetValue(userProfile.UserName, out sessionCount)
                        && sessionCount > 0)
                {
                    var speakerProfile = createSpeakerProfile(userProfile, sessionCount);
                    usersWithSessions.Add(speakerProfile);
                }
            }

            return usersWithSessions;
        }

        public SpeakerProfile GetProfileByUserName(string userName)
        {
//            var allSessions = sessionRepositoryFactory.Create().GetAllSessions();
//            allSessions.Where(s => )


//            return createSpeakerProfile(userProfile, sessionCount);
            return null;
        }

        private SpeakerProfile createSpeakerProfile(UserProfile userProfile, int numberOfSubmittedSessions)
        {
            return new SpeakerProfile
            {
                UserId = userProfile.Id,
                UserName = userProfile.UserName,
                Name = userProfile.Name,
                EmailAddress = userProfile.EmailAddress,
                NewSpeaker = userProfile.NewSpeaker,
                NumberOfSubmittedSessions = numberOfSubmittedSessions
            };
        }
    }
}
