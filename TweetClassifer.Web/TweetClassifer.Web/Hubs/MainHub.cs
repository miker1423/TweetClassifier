using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetClassifer.Web.Services;

namespace TweetClassifer.Web.Hubs
{
    public class MainHub : Hub
    {
        readonly TweetTableStorage _storageService;
        public MainHub(TweetTableStorage storageService)
            => _storageService = storageService;

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var tweets = await _storageService.GetUnClassified();
            await Clients.Client(Context.ConnectionId).SendAsync("Tweets", JsonConvert.SerializeObject(tweets));
        }
    }
}
