using System.Text.Json;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.DecisionGames;
using DecisionMate.Domain.DecisionGames.ValueObjects;
using DecisionMate.Domain.Users;
using DecisionMate.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DecisionMate.Infrastructure.DecisionGames;

public sealed class DecisionGameConfiguration : IEntityTypeConfiguration<DecisionGame>
{
    public void Configure(EntityTypeBuilder<DecisionGame> builder)
    {
        builder.ToTable("decision_games");

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId);

        builder.OwnsMany(x => x.Results, b => b.ToJson());

        builder.Property(x => x.Settings)
            .HasJsonConversion();

        builder.Property(x => x.Code)
            .HasConversion(
                x => x == null ? null : x.Value.Value,
                x => x == null ? null : new JoinCode(x)
            )
            .HasMaxLength(JoinCode.CodeLength);

        builder.OwnsMany(x => x.Players, ConfigurePlayer);
    }

    private static void ConfigurePlayer(OwnedNavigationBuilder<DecisionGame, DecisionGame.Player> builder)
    {
        builder.ToTable("players");

        builder.Property(x => x.Choices)
            .HasConversion(
                x => JsonSerializer.Serialize(x, null as JsonSerializerOptions),
                x => new DecisionGame.Player.ChoicesArray(
                    JsonSerializer.Deserialize<int[]>(x, null as JsonSerializerOptions)
                    ?? Array.Empty<int>()
                )
            );
        
        builder.HasOne<UserProfile>()
            .WithOne()
            .HasForeignKey<DecisionGame.Player>(x => x.Id);
    }
}