using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UI_Forms
{
    public static class ApiService
    {
        // 1. HTTP 클라이언트 단일 인스턴스 (앱 전체에서 공유)
        private static readonly HttpClient _httpClient = new HttpClient();

        // 2. 테스트 로그에서 확인된 실제 작동하는 Base URL
        private const string BaseUrl = "https://meeting-scheduler-csharp.onrender.com";

        // 3. 전역 상태 변수 (로그인 후 저장)
        public static string JwtToken { get; private set; } = string.Empty;
        public static string CurrentUserId { get; private set; } = string.Empty;
        public static string CurrentUserName { get; private set; } = string.Empty; // 이름 속성 추가

        // JSON 직렬화 설정 (낙타표기법 등 서버 명세에 맞춤)
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// 로그인 성공 시 발급받은 토큰과 유저 ID를 저장하고 헤더에 세팅합니다.
        /// </summary>
        public static void SetAuthData(string token, string userId, string userName)
        {
            JwtToken = token;
            CurrentUserId = userId;
            CurrentUserName = userName; // 이름 저장

            // 이후 모든 HTTP 요청 헤더에 JWT 토큰을 자동으로 포함
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// 공통 POST 요청 메서드
        /// </summary>
        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                string url = $"{BaseUrl}{endpoint}";
                string jsonContent = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                // 성공 여부와 상관없이 JSON 객체로 파싱하여 반환 (에러 메시지도 객체에 담겨 오기 때문)
                return JsonSerializer.Deserialize<TResponse>(responseBody, _jsonOptions);
            }
            catch (Exception ex)
            {
                // 네트워크 오류 등 예외 발생 시 처리
                throw new Exception($"API POST 요청 중 오류 발생: {ex.Message}");
            }
        }

        /// <summary>
        /// 공통 GET 요청 메서드
        /// </summary>
        public static async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            try
            {
                string url = $"{BaseUrl}{endpoint}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<TResponse>(responseBody, _jsonOptions);
            }
            catch (Exception ex)
            {
                throw new Exception($"API GET 요청 중 오류 발생: {ex.Message}");
            }
        }
    }
}