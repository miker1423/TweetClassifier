using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using TweetClassifer.Web.Services;

namespace TweetClassifer.Web.Hubs
{
    public class MainHub : Hub
    {
        readonly Random random = new Random();
        readonly TweetTableStorage _storageService;
        public MainHub(TweetTableStorage storageService)
            => _storageService = storageService;

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var tweets = await _storageService.GetUnClassified();
            var filtered = tweets.Skip(random.Next(0, tweets.Count / 2));
            await Clients.Client(Context.ConnectionId).SendAsync("Tweets", JsonConvert.SerializeObject(filtered));
        }
    }
}
