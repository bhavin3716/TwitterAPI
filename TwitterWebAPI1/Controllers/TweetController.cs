using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitterWebAPI1.Model;
using TwitterWebAPI1.Services;

namespace TwitterWebAPI1.Controllers
{
    [Route("api/v1.0/tweets/")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetsService _tweetService;

        public TweetController(ITweetsService tweetsService)
        {
            _tweetService = tweetsService;
        }

        // GET: api/Tweets
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Tweets>>> GetTweets()
        {
            return Ok(await _tweetService.GetAllTweets());
        }

        [HttpGet]
        [Route("{userName}")]
        public async Task<IActionResult> GetAllTweetsforUser(string userName)
        {
            var response = await _tweetService.GetAllTweetsOfUser(userName);
            if(response.Data == null)
            {
                response.Success = false;
                response.Message = "Record Not Found!";
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("{useName}/add")]
        public async Task<IActionResult> Add(Tweets Tweets)
        {
            var response = await _tweetService.AddTweet(Tweets);
            if ((bool)!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
