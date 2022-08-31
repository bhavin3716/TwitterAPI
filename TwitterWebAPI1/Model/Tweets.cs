using System.ComponentModel.DataAnnotations;

namespace TwitterWebAPI1.Model
{
    public class Tweets
    {
        [Key]
        public int TweetId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Description { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int UserId { get; set; }

    }
}
