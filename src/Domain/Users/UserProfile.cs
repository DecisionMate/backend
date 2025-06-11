using DecisionMate.Domain.Common;
using Geneirodan.Abstractions.Domain;

namespace DecisionMate.Domain.Users;

public sealed class UserProfile : Entity<Guid>
{
    public Types.Username Username { get; set; }
    public Url? AvatarUrl { get; set; }
    
    private readonly HashSet<Guid> _friends = [];
    public IReadOnlyCollection<Guid> Friends => _friends;
    public void AddFriend(Guid friend) => _friends.Add(friend);
    public void RemoveFriend(Guid friend) => _friends.Remove(friend);
    
    public static class Types
    {
        public record struct Username(string Value)
        {
            public const int MinLength = 2;
            public const int MaxLength = 32;
        }
    }
}