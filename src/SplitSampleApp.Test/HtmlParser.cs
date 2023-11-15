using HtmlAgilityPack;

namespace SplitSampleApp.Test
{
    internal static class HtmlParser
    {
        public static HtmlParsedResult Parse(string html)
        {
            var imageUrl = string.Empty;
            var requestVerificationToken = string.Empty;
            var surveyIntro = string.Empty;
            var userId = string.Empty;

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var image = document.DocumentNode.SelectSingleNode("//body/img");
            if (image != null)
            {
                imageUrl = image.GetAttributeValue("src", string.Empty);
            }

            var h2 = document.DocumentNode.SelectSingleNode("//body/h2");
            if (h2 != null)
            {
                surveyIntro = h2.InnerText;
            }

            var inputs = document.DocumentNode.SelectNodes("//body/form/input");
            if (inputs != null)
            {
                foreach (var input in inputs)
                {
                    if (input != null)
                    {
                        if (input.GetAttributeValue("name", string.Empty) == "UserId")
                        {
                            userId = input.GetAttributeValue("value", string.Empty);
                        }

                        if (input.GetAttributeValue("name", string.Empty) == "__RequestVerificationToken")
                        {
                            requestVerificationToken = input.GetAttributeValue("value", string.Empty);
                        }
                    }
                }
            }

            return new HtmlParsedResult()
            {
                ImageUrl = imageUrl,
                RequestVerificationToken = requestVerificationToken,
                SurveyIntro = surveyIntro,
                UserId = userId,
            };
        }
    }
}
