using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.Data.Entities
{
    public class UserWatchlist
    {
        public int WatchlistId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string Note { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
