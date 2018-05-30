using System;
using System.Linq;
using FunctionId.Logic.Model;
using FunctionId.SharedInterfaces;
using Moq;
using Xunit;
using Microsoft.Azure.WebJobs.Host;

namespace LogicTests
{
    public class AuthenticationManagerUnitTests
    {

        

        [Fact]
        public void CreateUser()
        {
            var mockLogger = new Mock<TraceWriter>();
            mockLogger.Setup(x => x.Info(It.IsAny<string>(),null));

            var dataStore = new Mock<IDataStore>();    
            var appSettings = new Mock<IAppSettings>();


            AuthenticationManager auth = new AuthenticationManager(dataStore.Object,mockLogger.Object,appSettings.Object);

            auth.AddUser()

        }
    }
}
