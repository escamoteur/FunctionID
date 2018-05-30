namespace FunctionId.SharedInterfaces
{
    public interface IDataStore
    {
        void Add(IUserInDb user);
        IUserInDb GetUser(string userName, string email);
        IUserInDb GetUserByEmail(string email);
        IUserInDb GetUserByUserName(string userName);
        void UpdateUser(IUserInDb userInDb);
        IUserInDb GetUserByToken(string refreshToken);
        IUserInDb GetUserByActivationToken(string activationToken);
        void DeleteUser(string userName);
    }
}