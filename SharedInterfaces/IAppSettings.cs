namespace FunctionId.SharedInterfaces
{
    public interface IAppSettings
    {
        bool DebugMode { get; set; }
        bool IgnoreSubscriptionValidCheck { get; set; }
        bool IgnoreUserActiveCheck { get; set; }
        string JwtKey { get; set; }
        double TokenLiveTimeInMinutes { get; set; }
    }
}