using DecisionMate.Domain.Common;
using DecisionMate.Domain.Users;
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

        builder.Property(o => o.Username)
            .HasColumnName("username")
            .HasMaxLength(UserProfile.Types.Username.MaxLength)
            .HasConversion(x => x.Value, x => new UserProfile.Types.Username(x));

        builder.Property(x => x.AvatarUrl)
            .HasUrlToStringConversion()
            .HasMaxLength(Url.MaxLength);

        builder.HasMany<UserProfile>()
            .WithMany()
            .UsingEntity(join => join.ToTable("friends"));
    }
}