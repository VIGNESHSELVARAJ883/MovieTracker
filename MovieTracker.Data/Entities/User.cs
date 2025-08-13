using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.Data.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<UserWatchlist> Watchlist { get; set; }
        public ICollection<UserToWatch> ToWatchList { get; set; }
        public ICollection<UserFavourite> Favourites { get; set; }
        public ICollection<Collection> Collections { get; set; }
        public ICollection<UserActivity> Activities { get; set; }
        public ICollection<UserReview> Reviews { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
