using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace SplitSampleApp.Test
{
    internal class TestClient
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;
        private const string endpoint = "https://localhost:44354/";

        public TestClient(ILogger logger, HttpClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task Run()
        {
            var response = await _client.SendAsync(CreateGetRequest());
            var html = await response.Content.ReadAsStringAsync();

            var htmlResult = HtmlParser.Parse(html);

            _logger.LogInformation("UserId: {userId}", htmlResult.UserId);
            _logger.LogInformation("ImageUrl: {imageUrl}", htmlResult.ImageUrl);
            _logger.LogInformation("SurveyIntro: {surveyIntro}", htmlResult.SurveyIntro);
            _logger.LogInformation("RequestVerificationToken: {requestVerificationToken}", htmlResult.RequestVerificationToken);

            if (SurveyParticipation(htmlResult.SurveyIntro))
            {
                await Task.Delay(200);
                var score = GetScore(htmlResult.ImageUrl);
                var request = CreatePostRequest(htmlResult.UserId, score, htmlResult.RequestVerificationToken);
                response = await _client.SendAsync(request);
                _logger.LogInformation("UserId {userId} submitted a score {score}.", htmlResult.UserId, score);
            }
            else
            {
                _logger.LogInformation("UserId {userId} did not participate the survey.", htmlResult.UserId);
            }
        }

        private static bool SurveyParticipation(string surveyIntro)
        {
            var participation = false;

            if (surveyIntro.Contains("win", StringComparison.OrdinalIgnoreCase))
            {
                participation = true;
            }
            else
            {
                var rnd = new Random();
                var rndNum = rnd.Next(0, 100);
                if (rndNum < 50)
                {
                    participation = true;
                }
            }

            return participation;
        }

        private static int GetScore(string imageUrl)
        {
            var score = 1;
            var rnd = new Random();

            if (imageUrl.Contains("tripadvisor", StringComparison.OrdinalIgnoreCase))
            {
                score = rnd.Next(1, 4); // 1 - 3
            }
            else if (imageUrl.Contains("flickr", StringComparison.OrdinalIgnoreCase))
            {
                score = rnd.Next(3, 6); // 3 - 5
            }

            return score;
        }

        private static HttpRequestMessage CreateGetRequest()
        {
            return new HttpRequestMessage(HttpMethod.Get, endpoint);
        }

        private static HttpRequestMessage CreatePostRequest(string userId, int score, string requestVerificationToken)
        {

            var queryString = new Dictionary<string, string>()
            {
                { "handler", "Submit" }
            };

            var requestUri = QueryHelpers.AddQueryString(endpoint, queryString);

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            request.Content = new MultipartFormDataContent
            {
                {new StringContent(userId),"UserId"},
                {new StringContent(score.ToString()),"Score"},
                {new StringContent(requestVerificationToken),"__RequestVerificationToken"},
            };

            return request;
        }
    }
}
