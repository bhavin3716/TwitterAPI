using System.ComponentModel.DataAnnotations;

namespace TwitterWebAPI1.Model
{
    public class TweetReplay
    {
        [Key]
        public int ReplyId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Reply { get; set; }

        public int UserId { get; set; }

        public int TweetId { get; set; }
        public Tweets TweetsObj { get; set; }
        public UserMaster UserObj { get; set; }
    }
}
