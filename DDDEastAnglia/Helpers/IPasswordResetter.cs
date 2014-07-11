namespace DDDEastAnglia.Helpers
{
    public interface IPasswordResetter
    {
        bool ResetPassword(string passwordResetToken, string newPassword);
    }
}
