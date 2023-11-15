using Microsoft.AspNetCore.Mvc;

namespace SplitSampleApp.Models
{
    public class ImageAttributes
    {
        [BindProperty]
        public int Height { get; set; }
    }
}
