using Microsoft.AspNetCore.Mvc;

namespace SplitSampleApp.Models
{
    public class Survey
    {
        [BindProperty]
        public int Score { get; set; }

        [BindProperty]
        public string UserId { get; set; }
    }
}
