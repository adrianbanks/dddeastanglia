using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess.Sessionize
{
    public sealed class SessionizeUserRepository : IUserProfileRepository
    {
        private readonly SessionizeDataLoader sessionizeDataLoader;
        private readonly ConferenceLoader conferenceLoader;

        public SessionizeUserRepository(SessionizeDataLoader sessionizeDataLoader, ConferenceLoader conferenceLoader)
        {
            this.sessionizeDataLoader = sessionizeDataLoader;
            this.conferenceLoader = conferenceLoader;
        }

        public IEnumerable<UserProfile> GetAllUserProfiles()
        {
            var conference = conferenceLoader.LoadConference();
            var sessionizeInfo = conference.SessionizeInfo;
            var data = sessionizeDataLoader.Load(sessionizeInfo.ConferenceId);

            return data.Speakers.Select(s => new UserProfile
            {
                Name = s.FullName,
                Bio = s.Bio,
                Id = s.Id,
                TwitterHandle = s.Links.FirstOrDefault(l => l.Title == "Twitter")?.Url,
                WebsiteUrl = s.Links.FirstOrDefault(l => l.Title == "Blog")?.Url
            });
        }

        public UserProfile GetUserProfileById(Guid id)
        {
            return GetAllUserProfiles()
                .FirstOrDefault(p => p.Id == id);
        }

        public UserProfile GetUserProfileByUserName(string userName)
        {
            return GetAllUserProfiles()
                .FirstOrDefault(p => p.UserName == userName);
        }

        public UserProfile GetUserProfileByEmailAddress(string emailAddress)
        {
            throw new NotSupportedException();
        }

        public UserProfile AddUserProfile(UserProfile userProfile)
        {
            throw new NotSupportedException();
        }

        public void UpdateUserProfile(UserProfile profile)
        {
            throw new NotSupportedException();
        }

        public void DeleteUserProfile(Guid id)
        {
            throw new NotSupportedException();
        }
    }
}
