using System;

namespace FunctionId.Shared
{
    public interface IUserData
    {
         string Id { get; set; }
         string UserName { get; set; }
         string Name { get; set; }
         string FirstName { get; set; }
         string Email { get; set; }
         bool IsActive { get; set; }
         DateTime ExpiresOn { get; set; }
         string PreferedLanguage { get; set; }
         string GenericJsonData { get; set; }
    }
}
