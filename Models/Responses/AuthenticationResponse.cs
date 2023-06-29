using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace asp.net_core_api_template.Models.Responses
{
    public class AuthenticationResponse
    {
        [Required]
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }


        [Required]
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [Required]
        [JsonProperty("user")]
        public UserResponse User { get; set; }
    }
}
