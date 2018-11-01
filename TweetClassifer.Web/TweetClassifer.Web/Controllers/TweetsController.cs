using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetClassifer.Shared.Models;

namespace TweetClassifer.Web.Controllers
{
    [Route("api/[controller]")]
    public class TweetsController : Controller
    {
        [HttpPost]
        public async IActionResult Post([FromBody]TweetVM tweet)
        {
            return Ok();
        }
    }
}
