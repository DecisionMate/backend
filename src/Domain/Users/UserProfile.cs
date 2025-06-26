using DecisionMate.Domain.Common;
using DecisionMate.Domain.Users.Events;
using DecisionMate.Domain.Users.ValueObjects;

namespace DecisionMate.Domain.Users;

public sealed class UserProfile : AggregateRoot
{
    public UserName UserName { get; set; }
    public Url? AvatarUrl { get; set; }

    private readonly HashSet<Guid> _friends = [];

    public IReadOnlySet<Guid> Friends
    {
        get => _friends;
        init => _friends = value.ToHashSet();
    }

    public void AddFriend(Guid friend) => _friends.Add(friend);
    public void RemoveFriend(Guid friend) => _friends.Remove(friend);

    public static (UserProfile, UserProfileCreatedEvent) Create(Guid id, UserName userName, Url? avatarUrl)
    {
        var profile = new UserProfile
        {
            Id = id,
            UserName = userName,
            AvatarUrl = avatarUrl
        };
        var @event = new UserProfileCreatedEvent(id, userName, avatarUrl);
        profile.AddEvent(@event);
        return (profile, @event);
    }

    public UserProfileUpdatedEvent Edit(UserName userName, Url? avatarUrl)
    {
        UserName = userName;
        AvatarUrl = avatarUrl;
        var @event = new UserProfileUpdatedEvent(Id, userName, avatarUrl);
        AddEvent(@event);
        return @event;
    }

    public UserProfileDeletedEvent Delete()
    {
        IsDeleted = true;
        var @event = new UserProfileDeletedEvent(Id);
        AddEvent(@event);
        return @event;
    }
}