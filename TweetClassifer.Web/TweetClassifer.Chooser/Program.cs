using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;

namespace TweetClassifer.Chooser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(args[0])
            };

            Auth.SetUserCredentials(args[1], args[2], args[3], args[4]);

            var user = User.GetAuthenticatedUser();

            var regex = new Regex(@"\p{Cs}");

            var stream = Stream.CreateFilteredStream();
            stream.AddTweetLanguageFilter(Tweetinvi.Models.LanguageFilter.English);
            stream.AddTrack("the The");

            Console.CancelKeyPress += (sender, cnArgs) =>
            {
                httpClient.Dispose();
                stream.StopStream();
                stream = null;
            };

            stream.MatchingTweetReceived += (sender, twArgs) =>
            {
                var tweet = twArgs.Tweet;
                if (!tweet.IsRetweet && regex.IsMatch(tweet.Text))
                {
                    var json = JsonConvert.SerializeObject(tweet.ToVM());
                    var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
                    httpClient.PostAsync("/api/tweets", jsonContent);
                }
            };
            await stream.StartStreamMatchingAnyConditionAsync();

            Console.ReadLine();
        }
    }
}
