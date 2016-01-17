using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Der_Bayer_und_der_Würschtlmann
{
    /// <summary>
    ///     Summarizes little, static generic helpers.
    ///         - RSS Handling
    ///         - etc.
    /// </summary>
    class TSUtils
    {
        /// <summary>
        ///     Parses async a given url for rss / wordpress feed objects
        /// </summary>
        /// <param name="url">URL to the blog</param>
        /// <returns>Async task</returns>
        public static async Task<List<FeedItem>> getFeedsAsync(string url)
        {
            SyndicationClient client = new SyndicationClient();
            List<FeedItem> feedData = new List<FeedItem>();
            Uri feedUri = new Uri(url);
            var feed = await client.RetrieveFeedAsync(feedUri);

            foreach (SyndicationItem item in feed.Items)
            {
                FeedItem feedItem = new FeedItem();

                feedItem.Title = item.Title.Text;
                feedItem.Content = item.Summary.Text;
                feedItem.Link = item.Links[0].Uri;
                feedItem.PubDate = item.PublishedDate.DateTime;
                feedItem.Author = item.Authors[0].Name;

                List<string> categories = new List<string>(item.Categories.Count);
                foreach (SyndicationCategory category in item.Categories)
                {
                    categories.Add(category.Term);
                }
                feedItem.Categories = String.Join(", ", categories);

                var pattern = "(https?)\\S*(png|jpg|jpeg)";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                var match = regex.Match(item.NodeValue);

                if (match.Success)
                {
                    var imageUrl = match.Value;
                    
                    // Resize the image to width = 80 before downloading by calling a WP-service
                    imageUrl = imageUrl.Insert(imageUrl.Length, "?w=80");
                    feedItem.ArticleImageLink = new Uri(imageUrl);
                }

                feedData.Add(feedItem);
            }

            return feedData;
        }

        /// <summary>
        ///     Shortens the way to debug print a string
        /// </summary>
        /// <param name="message">Message to print</param>
        public static void Print(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }

    /// <summary>
    ///     Model for a RSS feed item.
    ///     Contains data of an feed article
    /// </summary>
    public class FeedItem
    {
        /// <summary>
        ///     Title of the article
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Html content of the article
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Author name of the article
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     Comma seperated list of assigned categories
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        ///     URI to the article
        /// </summary>
        public Uri Link { get; set; }

        /// <summary>
        ///     URI to the first article's images.
        ///     Aka the header image
        /// </summary>
        public Uri ArticleImageLink { get; set; }

        /// <summary>
        ///     Publishing date of the article
        /// </summary>
        public DateTime PubDate { get; set; }
    }
}
