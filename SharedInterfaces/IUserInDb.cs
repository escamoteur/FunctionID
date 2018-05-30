using FunctionId.Shared;

namespace FunctionId.SharedInterfaces
{
    public interface IUserInDb : IUserData
    {
        string HashedAndSaltedPassword { get; set; }
        string SaltString { get; set; }
        string RequestsPasswordResetToken { get; set; }
        string LastActivationToken { get; set; }
        string RefreshToken { get; set; }
    }
}
