using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet("[action]")]
        public async Task<IActionResult> Classified()
        {
            var classified = await _storageService.GetClassified();
            return Json(new { classified = classified.Count });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Unclassified()
        {
            var uncassified = await _storageService.GetUnClassified();
            return Json(new { Unclassified = uncassified.Count });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Total() 
            => Json(new { Total = await _storageService.TotalCount() });

        [HttpGet("[action]")]
        public async Task<IActionResult> Stats()
            => Json(await _storageService.GetStats());

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]string id)
        {
            await _storageService.Remove(id);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Csv()
        {
            var classified = await _storageService.GetClassified();
            var file = await CsvGenerator.GetCSV(classified);
            return File(file, "application/octet-stream", "tweets.csv");
        }
    }
}
