namespace DDDEastAnglia.Helpers
{
    public interface IResetPasswordTokenGenerator
    {
        string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow);
    }
}

