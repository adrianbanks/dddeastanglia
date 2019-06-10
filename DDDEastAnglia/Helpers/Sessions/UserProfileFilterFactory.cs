using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface IUserProfileFilterFactory
    {
        IUserProfileFilter Create(IConference conference);
    }

    public sealed class UserProfileFilterFactory : IUserProfileFilterFactory
    {
        private readonly ISessionRepositoryFactory sessionRepositoryFactory;

        public UserProfileFilterFactory(ISessionRepositoryFactory sessionRepositoryFactory)
        {
            if (sessionRepositoryFactory == null)
            {
                throw new ArgumentNullException(nameof(sessionRepositoryFactory));
            }

            this.sessionRepositoryFactory = sessionRepositoryFactory;
        }

        public IUserProfileFilter Create(IConference conference)
        {
            IUserProfileFilter userProfileFilter;

            if (conference.CanPublishAgenda())
            {
                userProfileFilter = new SelectedSpeakerProfileFilter(SelectedSessions.SpeakerIds);
            }
            else
            {
                userProfileFilter = new SubmittedSessionProfileFilter(sessionRepositoryFactory);
            }

            return userProfileFilter;
        }
    }
}
