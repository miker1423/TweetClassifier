using System;
using System.Collections.Generic;
using System.Text;
using TweetClassifer.Shared.Models;
using Tweetinvi.Models;

namespace TweetClassifer.Chooser
{
    public static class TweetExtensions
    {
        public static TweetVM ToVM(this ITweet tweet)
            => new TweetVM
            {
                Text = tweet.Text,
                URL = "https://twitter.com/" + tweet.CreatedBy.ScreenName,
                Gender = Gender.Other
            };
    }
}
