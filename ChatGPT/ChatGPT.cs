using ChatGPT.Models;
using System.Net.Http.Json;

namespace ChatGPT
{
    public class ChatGPT
    {
        private readonly string _endpoint = @"https://api.openai.com/v1/chat/completions";
        private readonly string _apiKey;

        public ChatGPT(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetResponseAsync(string content)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var message = new List<Message> { new Message() { Role = "user", Content = content } };
            var requestData = new Request()
            {
                ModelId = "gpt-3.5-turbo",
                Messages = message
            };

            using var response = await httpClient.PostAsJsonAsync(_endpoint, requestData);

            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

            var choices = responseData?.Choices ?? new List<Choice>();
            if (choices.Count == 0)
            {
                return string.Empty;
            }
            var choice = choices[0];
            var responseMessage = choice.Message;

            return responseMessage.Content.Trim();
        }
    }
}
