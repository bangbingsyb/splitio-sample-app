using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Splitio.Services.Client.Interfaces;
using SplitSampleApp.Models;

namespace SplitSampleApp.Pages
{
    public class IndexModel : PageModel
    {
        private const string _coverPhotoUrl = "https://media-cdn.tripadvisor.com/media/photo-s/06/d0/6a/d0/meadows-of-lupine-wildflowers.jpg";

        private const string _newCoverPhotoUrl = "https://live.staticflickr.com/8076/29334716172_ef4842a5e5_c.jpg";

        private const string _surveyIntro = "Please rate the page with a score from 1 - 5.";

        private const string _surveyIntroIncentive = "Please rate the page with a score from 1 - 5 with the chance to win $1000.";

        private readonly ILogger _logger;

        private ISplitClient _splitClient;

        public string UserId { get; }

        public string ImageUrl { get; }

        public int ImageHeight { get; }

        public string SurveyIntro { get; }

        public IndexModel(ISplitClient splitClient, ILogger<IndexModel> logger)
        {
            _splitClient = splitClient ?? throw new ArgumentNullException(nameof(splitClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Generate user ID
            var rnd = new Random();
            int rndNum = rnd.Next(1, 1001);
            UserId = "user_" + rndNum.ToString();

            // Determine cover photo
            var treatment = _splitClient.GetTreatment(UserId, "new_cover_photo");
            _logger.LogInformation("The new cover photo feature is {treatment} for User {userId}", treatment, UserId);

            if (treatment == "on")
            {
                ImageUrl = _newCoverPhotoUrl;
            }
            else if (treatment == "off")
            {
                ImageUrl = _coverPhotoUrl;
            }
            else
            {
                ImageUrl = _coverPhotoUrl;
            }

            // Determine cover photo size
            var splitResult = _splitClient.GetTreatmentWithConfig(UserId, "large_cover_photo");
            var imageAttributes = JsonConvert.DeserializeObject<ImageAttributes>(splitResult.Config);
            ImageHeight = imageAttributes?.Height ?? 300;

            _logger.LogInformation("The large cover photo feature is {treatment} for User {userId}", splitResult.Treatment, UserId);
            _logger.LogInformation("The cover photo height is {imageHeight} for User {userId}", ImageHeight, UserId);

            // Determine survey introduction
            treatment = _splitClient.GetTreatment(UserId, "survey_incentive");
            _logger.LogInformation("The survey incentive feature is {treatment} for User {userId}", treatment, UserId);

            if (treatment == "on")
            {
                SurveyIntro = _surveyIntroIncentive;
            }
            else if (treatment == "off")
            {
                SurveyIntro = _surveyIntro;
            }
            else
            {
                SurveyIntro = _surveyIntro;
            }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostSubmit(Survey survey)
        {
            var score = Convert.ToDouble(survey.Score);
            var submitterUserId = survey.UserId;

            _splitClient.Track(submitterUserId, "user", "image_rating", score);
            _logger.LogInformation("The User {userId} rated the photo with a score {score}", submitterUserId, score);

            return Redirect("/");
        }
    }
}