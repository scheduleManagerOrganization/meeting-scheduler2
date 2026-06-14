using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UI_Forms.Models
{
    // 로그인 보낼 때 (요청)
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    // 로그인 성공 시 서버가 주는 데이터 (응답)
    public class LoginResponseData
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
    public class RegisterRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = "Asia/Seoul"; // API 명세에 따른 기본값
    }

    // 서버 헬스 체크 응답용 모델
    public class HealthResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mongodb")]
        public string Mongodb { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
    }
}
