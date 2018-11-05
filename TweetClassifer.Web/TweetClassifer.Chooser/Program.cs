using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using Tweetinvi;
using Tweetinvi.Models;
using System.Collections.Generic;
using TweetClassifer.Shared.Models;

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

            var tweetQueue = new Queue<TweetVM>();

            var thread = new Thread((state) =>
            {
                var tweetList = new List<TweetVM>();
                while (true)
                {
                    if(tweetQueue.Count != 0)
                    {
                        tweetList.Add(tweetQueue.Dequeue());
                        Console.Write($"{tweetList.Count} ");
                    }

                    if(tweetList.Count > 9)
                    {
                        Console.WriteLine("Send");
                        var json = JsonConvert.SerializeObject(tweetList);
                        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
                        httpClient.PostAsync("/api/tweets", jsonContent);

                        tweetList.Clear();
                    }
                }

            });
            thread.Start();

            Auth.SetUserCredentials(args[1], args[2], args[3], args[4]);

            var user = User.GetAuthenticatedUser();

            var regex = new Regex(@"\p{Cs}");

            var stream = Stream.CreateFilteredStream();
            stream.AddTweetLanguageFilter(LanguageFilter.English);
            stream.AddTrack("the The be to of and a Be To Of And A");

            Console.CancelKeyPress += (sender, cnArgs) =>
            {
                httpClient.Dispose();
                stream.StopStream();
                stream = null;
            };

            stream.MatchingTweetReceived += (sender, twArgs) =>
            {
                var tweet = twArgs.Tweet;
                if (!tweet.IsRetweet && !regex.IsMatch(tweet.Text))
                {
                    tweetQueue.Enqueue(tweet.ToVM());
                }
            };
            await stream.StartStreamMatchingAnyConditionAsync();

            Console.ReadLine();
        }
    }
}
