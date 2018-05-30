namespace FunctionId.Logic.Model
{
    public interface IDataStore
    {
        void Add(UserDataInDb user);
        UserDataInDb GetUser(string userName, string email);
        UserDataInDb GetUserByEmail(string email);
        UserDataInDb GetUserByUserName(string userName);
        void UpdateUser(UserDataInDb userInDb);
        UserDataInDb GetUserByToken(string refreshToken);
        UserDataInDb GetUserByActivationToken(string activationToken);
        void DeleteUser(string testuser);
    }
}