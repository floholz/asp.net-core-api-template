using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace asp.net_core_api_template.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }


        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
