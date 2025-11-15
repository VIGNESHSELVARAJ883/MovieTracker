namespace MovieTracker.Data.Entities
{
    public class RefreshToken
    {
        public Guid TokenId { get; set; } // PK
        public int UserId { get; set; }   // FK
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Navigation property
        public User User { get; set; } = null!;
    }
}
