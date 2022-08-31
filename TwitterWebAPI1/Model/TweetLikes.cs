using System.ComponentModel.DataAnnotations;

namespace TwitterWebAPI1.Model
{
    public class TweetLikes
    {
        [Key]
        public int Id { get; set; }

        public bool Liked { get; set; } = false;

        public int TweetId { get; set; }

        public int UserId { get; set; }

        public Tweets TweetObj { get; set; }

        public UserMaster UserObj { get; set; }
    }
}
