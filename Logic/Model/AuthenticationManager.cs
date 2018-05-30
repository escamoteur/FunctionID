using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FunctionId.Logic.Helpers;
using FunctionId.Logic.Model;
using FunctionId.Shared;
using FunctionId.SharedInterfaces;
using Jose;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionId.Logic.Model
{
    public class AuthenticationManager
    {
        private readonly IDataStore dataStore;
        private readonly TraceWriter log;
        private readonly IAppSettings settings;

        public AuthenticationManager(IDataStore dataStore, TraceWriter logger, IAppSettings settings)
        {
            this.dataStore = dataStore;
            this.log = logger;
            this.settings = settings;
        }


        public bool AddUser(UserDataInDb user, string passWord)
        {
            try
            {
                    user.Id = Guid.NewGuid().ToString();
                    user.IsActive = false;

                    var hashAndSalt = HashHelper.GenerateSaltedSHA1(passWord);

                    user.HashedAndSaltedPassword = hashAndSalt.hashedAndSalted;
                    user.SaltString = hashAndSalt.salt;

                    dataStore.Add(user);
                    
                    log.Info("New user Added: {@user}", user.ToString());
                    return true;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return false;
            }
        }


        // We allow to use either userName or email for logging in. We always try email first.
        public AuthenticationStatus LoginUser(string userName, string email, string pwd)
        {

            var userInDb = email != null ? dataStore.GetUserByEmail(email) : null;
            if (userInDb == null)
            {
                userInDb = dataStore.GetUserByUserName(userName);
            }

                var authenticationStatus = new AuthenticationStatus() { JwtToken = null, Status = AuthenticationStatus.UserStatus.UserInValid};

            if (userInDb == null || !HashHelper.VerifyAgainstSaltedHash(userInDb.HashedAndSaltedPassword, userInDb.SaltString, pwd))
            {
                authenticationStatus.Status = AuthenticationStatus.UserStatus.UserInValid;
                return authenticationStatus;
            }
                
            //Only issue a new refresh token if none is available
            if (string.IsNullOrWhiteSpace(userInDb.RefreshToken))
            {
                userInDb.RefreshToken = Guid.NewGuid().ToString();

                authenticationStatus.RefreshToken = userInDb.RefreshToken;

                dataStore.UpdateUser(userInDb);
            }
            else
            {
                authenticationStatus.RefreshToken = userInDb.RefreshToken;
            }
                
            return CompleteAuthenticationStatus(userInDb, authenticationStatus);
        }

        private AuthenticationStatus CompleteAuthenticationStatus(IUserInDb userInDb, AuthenticationStatus authenticationStatus)
        {
            // We return an refresh token even if the user is not activated or the subscription has expired.
            // In this cases we will not return a new access token (JWT token)

            //First check if active
            if (!userInDb.IsActive && !settings.IgnoreUserActiveCheck)
            {
                authenticationStatus.Status = AuthenticationStatus.UserStatus.UserInactive;
                return authenticationStatus;
            }

            //if active user should have a valid subscription
            if (!userInDb.IsExpired && !settings.IgnoreSubscriptionValidCheck)
            {
                authenticationStatus.Status = AuthenticationStatus.UserStatus.UserPaymentExpired;
                authenticationStatus.EndOfSubscription = userInDb.ExpiresOn;
                return authenticationStatus;
            }

            //everything fine
            authenticationStatus.Status = AuthenticationStatus.UserStatus.UserInValid;
            var accessToken = new JwtToken()
            {
                token = Guid.NewGuid(),
                sub = userInDb.Id,
                exp = DateTime.UtcNow.AddMinutes(settings.TokenLiveTimeInMinutes).ToBinary()
            };
            authenticationStatus.JwtToken =
                Jose.JWT.Encode(accessToken, Encoding.ASCII.GetBytes(settings.JwtKey), JwsAlgorithm.HS256);
            authenticationStatus.EndOfSubscription = userInDb.ExpiresOn;

            return authenticationStatus;
        }


        public AuthenticationStatus RefreshLogin(string refreshToken)
        {

            var userInDb = dataStore.GetUserByToken(refreshToken);

            var authenticationStatus = new AuthenticationStatus() { JwtToken = null, Status = AuthenticationStatus.UserStatus.UserInValid };

            if (userInDb == null)
            {
                return authenticationStatus;
            }
            return CompleteAuthenticationStatus(userInDb, authenticationStatus);
        }



        public AuthenticationStatus UpdateUserSubscription(string refreshToken, DateTime newExpiryDate)
        {
            var userInDb = dataStore.GetUserByToken(refreshToken);

            userInDb.ExpiresOn = newExpiryDate;
            dataStore.UpdateUser(userInDb);

            return new AuthenticationStatus()
            {
                Status = AuthenticationStatus.UserStatus.UserValid,
                EndOfSubscription = newExpiryDate
            };
        }

        // This shoudl be implemented to only work in TestMode
        public bool ActivateUser(string userName, string password)
        {
            return false;
        }


        public bool ActivateUser(string activationToken)
        {
 
            var userInDB = dataStore.GetUserByActivationToken(activationToken);

            if (userInDB == null)
            {
                return false;
            }

            userInDB.IsActive = true;
            userInDB.LastActivationToken = null;

            dataStore.UpdateUser(userInDB);

            return true;
        }


        public UserDataInDb UpdatePassword(object token, object password)
        {
            throw new NotImplementedException();
        }

        public bool UpdateLanguage(string credentialsUserName, object lang)
        {
            throw new NotImplementedException();
        }

        // will use sendgrid to completely changes
        public async Task<bool> SendActivationEmail(string userName)
        {
            //var theRealm = Realm.GetInstance(_realmConfiguration);

            //var activationManager = new EmailManager("EmailSettings.json", AppConfig);

            //var userToActivate = theRealm.All<UserData>()
            //                .FirstOrDefault(user => user.UserName == userName);


            //if (userToActivate != null)
            //{
            //    // Generate activation token
            //    theRealm.Write(() => userToActivate.LastActivationToken = Guid.NewGuid().ToString());

            //    await activationManager.SendActivationEmail(userToActivate);
            //    return true;
            //}

            return false;


        }

        // will use sendgrid to completely changes
        public async Task<bool> SendResetPasswordEmail(string userName)
        {
            //var theRealm = Realm.GetInstance(_realmConfiguration);

            //var activationManager = new EmailManager("EmailSettings.json",AppConfig);

            //var userWithResetRequest = theRealm.All<UserData>()
            //    .FirstOrDefault(user => user.UserName == userName);


            //if (userWithResetRequest != null)
            //{
            //    await activationManager.SendActivationEmail(userWithResetRequest.Email, userWithResetRequest.Id, userWithResetRequest.Language);
            //    return true;
            //}

            return false;
        }

        public void DeleteTestUser()
        {
            try
            {
                  dataStore.DeleteUser("TestUser");
            }
            catch (Exception e)
            {
                Debugger.Break();
                
            }
        }
    }
}
