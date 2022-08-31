using TwitterWebAPI1.Model;

namespace TwitterWebAPI1.Services
{
    public interface ITweetsService
    {
        Task<GenericResponse<List<Tweets>>> GetAllTweets();
        Task<GenericResponse<Tweets>> GetTweetById(int id);
        Task<GenericResponse<int>> AddTweet(Tweets Tweets);
        Task<GenericResponse<int>> UpdateTweet(int id, string TweetsDescription);
        Task<GenericResponse<int>> DeleteTweet(int id);
        Task<GenericResponse<int>> ReplyToTweet(int id, string reply, int userId);
        Task<GenericResponse<int>> LikeTweet(int id, bool flag, int userId);
        Task<GenericResponse<List<Tweets>>> GetAllTweetsOfUser(string userName);
    }
}
