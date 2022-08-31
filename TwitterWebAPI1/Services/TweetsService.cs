using TwitterWebAPI1.Model;
using TwitterWebAPI1.Data;
using Microsoft.EntityFrameworkCore;

namespace TwitterWebAPI1.Services
{
    public class TweetsService : ITweetsService
    {
        private readonly TwitterAppDataContext _twitterAppDataContext;

        public TweetsService(TwitterAppDataContext TweetserDataContext)
        {
            _twitterAppDataContext = TweetserDataContext;
        }
        public async Task<GenericResponse<int>> AddTweet(Tweets Tweets)
        {
            GenericResponse<int> response = new GenericResponse<int>();

            _twitterAppDataContext.Add(Tweets);
            await _twitterAppDataContext.SaveChangesAsync();

            response.Success = true;
            response.Message = "New tweet added successfully";
            return response;
        }

        public async Task<GenericResponse<int>> DeleteTweet(int id)
        {
            GenericResponse<int> response = new GenericResponse<int>();

            try
            {
                var Tweets = await _twitterAppDataContext.Tweets.FirstOrDefaultAsync(t => t.TweetId == id);
                if (Tweets == null)
                {
                    response.Success = false;
                    response.Message = "Tweets not found.!";
                    return response;
                }
                _twitterAppDataContext.Tweets.Remove(Tweets);
                await _twitterAppDataContext.SaveChangesAsync();

                response.Success = true;
                response.Message = "Tweets deleted successfully..";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<GenericResponse<List<Tweets>>> GetAllTweets()
        {
            var response = new GenericResponse<List<Tweets>>();
            var Tweets = await _twitterAppDataContext.Tweets.ToListAsync();
            response.Data = Tweets;
            response.Success = true;
            return response;
        }

        public async Task<GenericResponse<List<Tweets>>> GetAllTweetsOfUser(string userName)
        {
            var response = new GenericResponse<List<Tweets>>();
            var userDetail = await _twitterAppDataContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (userDetail == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            else
            {
                var Tweets = await _twitterAppDataContext.Tweets.Where(t => t.UserId == userDetail.Id).ToListAsync();
                response.Data = Tweets;
                response.Success = true;
                return response;
            }
        }

        public async Task<GenericResponse<Tweets>> GetTweetById(int id)
        {
            var response = new GenericResponse<Tweets>();
            var Tweets = await _twitterAppDataContext.Tweets.FirstOrDefaultAsync(x => x.TweetId == id);
            response.Data = Tweets;
            response.Success = true;
            return response;
        }

        public async Task<GenericResponse<int>> LikeTweet(int id, bool flag, int userId)
        {
            GenericResponse<int> response = new GenericResponse<int>();
            var tweet = await _twitterAppDataContext.Tweets.FirstOrDefaultAsync(x => x.TweetId == id);
            if (tweet == null)
            {
                response.Success = false;
                response.Message = "Tweets not found";
                return response;
            }
            else
            {
                var like = await _twitterAppDataContext.TweetsLikes.FirstOrDefaultAsync(l => l.TweetId == tweet.TweetId && l.UserId == userId);
                if (like == null)
                {
                    _twitterAppDataContext.TweetsLikes.Add(new TweetLikes { Liked = flag, TweetId = id, UserId = userId });
                }
                else
                {
                    like.Liked = flag;
                    _twitterAppDataContext.TweetsLikes.Update(like);
                }
                await _twitterAppDataContext.SaveChangesAsync();

                response.Success = true;
                return response;
            }
        }

        public async Task<GenericResponse<int>> ReplyToTweet(int id, string reply, int userId)
        {
            GenericResponse<int> response = new GenericResponse<int>();
            var tweet = await _twitterAppDataContext.Tweets.FirstOrDefaultAsync(x => x.TweetId == id);
            if (tweet == null)
            {
                response.Success = false;
                response.Message = "Tweets not found";
                return response;
            }
            else
            {
                _twitterAppDataContext.TweetsReplies.Add(new TweetReplay { Reply = reply, TweetId = id, UserId = userId });

                await _twitterAppDataContext.SaveChangesAsync();

                response.Success = true;
                return response;
            }
        }

        public async Task<GenericResponse<int>> UpdateTweet(int id, string TweetsDescription)
        {
            GenericResponse<int> response = new GenericResponse<int>();

            try
            {
                var Tweets = await _twitterAppDataContext.Tweets.FirstOrDefaultAsync(t => t.TweetId == id);
                if (Tweets == null)
                {
                    response.Success = false;
                    response.Message = "Tweet not found";
                    return response;
                }

                Tweets.Description = TweetsDescription;
                _twitterAppDataContext.Tweets.Update(Tweets);
                await _twitterAppDataContext.SaveChangesAsync();

                response.Success = true;
                response.Message = "Tweets updated successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
