using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionId.Logic
{
    class AppSettings
    {
        public bool IgnoreUserActiveCheck { get; set; }  // Allows for testing to ignore the activation check
        public bool IgnoreSubscriptionValidCheck { get; set; } // Allows for testing to ignore subscription time
        public string JwtKey { get; set; }
        public bool DebugMode { get; set; }
        public double TokenLiveTimeInMinutes { get; set; }
    }
}
