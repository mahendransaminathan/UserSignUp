
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Models
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonProperty("email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [JsonProperty("password")]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        
        [JsonProperty("userType")]    
        public string UserType { get; set; } = string.Empty;

        public User(string id, string email, string password, string userType)
        {
            Id = id;
            Email = email;
            Password = password;
            UserType = userType;
        }
    }
}