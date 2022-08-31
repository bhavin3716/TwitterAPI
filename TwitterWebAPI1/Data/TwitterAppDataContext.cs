using Microsoft.EntityFrameworkCore;
using TwitterWebAPI1.Model;

namespace TwitterWebAPI1.Data
{
    public class TwitterAppDataContext : DbContext
    {
        public TwitterAppDataContext(DbContextOptions<TwitterAppDataContext> options) : base(options)
        { }

        public DbSet<UserMaster> Users { get; set; }

        public DbSet<Tweets> Tweets { get; set; }
        public DbSet<TweetLikes> TweetsLikes { get; set; }
        public DbSet<TweetReplay> TweetsReplies { get; set; }
    }
}
