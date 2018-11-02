using Microsoft.AspNetCore.SignalR;
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


    }
}
