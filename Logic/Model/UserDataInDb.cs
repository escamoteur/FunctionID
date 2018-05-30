using System;
using FunctionId.SharedInterfaces;

namespace FunctionId.Logic.Model
{

    public class UserDataInDb :  IUserInDb
    {
        // Unique ID that is part of the returned access tokens
		public string Id { get; set; }

    
		public string Email { get; set; }
		public string UserName { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string PreferedLanguage { get; set; }
        public string GenericJsonData { get; set; }

        public bool IsActive { get; set; }


        public string HashedAndSaltedPassword { get; set; }
        public string SaltString { get; set; }

        // these one time tokens are set when an reset password / activation email is send out 
        // and is cleared as soon as the associated link was clicked.
        // if another reset / activate was requested before the earlier one these tokens will be over written 
        public string RequestsPasswordResetToken { get; set; }
        public string LastActivationToken { get; set; }

        // Long living refresh token that is created when logging in with a user name / password
        // It can be used to request a new access token
        // It's safer to store this on a device than the real user credentials
        public string RefreshToken { get; set; }

        // only used if you want to use a subscription based user model
        public DateTime ExpiresOn { get; set; }

        public bool IsExpired => ExpiresOn > DateTime.UtcNow;


    }
}
