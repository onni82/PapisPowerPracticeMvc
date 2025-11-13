using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PapisPowerPracticeMvc.Controllers
{
    public class ChatController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ChatController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromForm] string message)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                
                var jwtToken = HttpContext.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
                }
                
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var content = new StringContent($"\"{message}\"", System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{baseUrl}/api/ChatBot/chat", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    // Check if response is JSON
                    if (responseContent.StartsWith("{") || responseContent.StartsWith("["))
                    {
                        var data = JsonSerializer.Deserialize<ChatResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        return Json(new { success = true, response = data?.Response ?? "No response" });
                    }
                    else
                    {
                        // Return the raw response if it's not JSON
                        return Json(new { success = true, response = responseContent });
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, error = $"API returned {response.StatusCode}: {errorContent}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }

    public class ChatResponse
    {
        public string Response { get; set; } = string.Empty;
    }
}