using DecisionMate.Domain.Common;
using DecisionMate.Domain.Users;
using DecisionMate.Domain.Users.ValueObjects;
using DecisionMate.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DecisionMate.Infrastructure.UserProfiles;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("profiles");

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(o => o.UserName)
            .HasColumnName("username")
            .HasMaxLength(UserName.MaxLength)
            .HasConversion(x => x.Value, x => new UserName(x));

        builder.Property(x => x.AvatarUrl)
            .HasUrlToStringConversion()
            .HasMaxLength(Url.MaxLength);

        builder.HasMany(x => x.Friends)
            .WithMany()
            .UsingEntity(join => join.ToTable("friends"));
    }
}