using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetClassifer.Shared.Models;
using TweetClassifer.Web.Services;

namespace TweetClassifer.Web.Controllers
{
    [Route("api/[controller]")]
    public class TweetsController : Controller
    {
        readonly TweetTableStorage _storageService;
        public TweetsController(TweetTableStorage storageService)
            => _storageService = storageService;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]IList<TweetVM> tweets)
        {
            await _storageService.InsertRange(tweets);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string id, [FromQuery]Gender gender)
        {
            var entry = await _storageService.GetTweet(id);
            entry.Tweet.Gender = gender;
            entry.IsClassified = true;
            await _storageService.UpdateTweet(entry);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]string id)
        {
            await _storageService.Remove(id);
            return Ok();
        }
    }
}
