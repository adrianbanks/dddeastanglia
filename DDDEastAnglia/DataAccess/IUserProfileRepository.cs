using System;
using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IUserProfileRepository
    {
        IEnumerable<UserProfile> GetAllUserProfiles();
        UserProfile GetUserProfileById(Guid id);
        UserProfile GetUserProfileByUserName(string userName);
        UserProfile GetUserProfileByEmailAddress(string emailAddress);
        UserProfile AddUserProfile(UserProfile userProfile);
        void UpdateUserProfile(UserProfile profile);
        void DeleteUserProfile(Guid id);
    }
}
