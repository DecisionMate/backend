using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Options;
using DecisionMate.Domain.Categories.Options.ValueObjects;
using DecisionMate.Domain.Categories.ValueObjects;
using DecisionMate.Domain.Common;
using DecisionMate.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DecisionMate.Infrastructure.Categories;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, x => new CategoryName(x))
            .HasMaxLength(CategoryName.MaxLength);

        builder.Property(x => x.Description)
            .HasConversion(
                x => x.HasValue ? x.Value.Value : null,
                x => CategoryDescription.Create(x)
            )
            .HasMaxLength(CategoryDescription.MaxLength);

        builder.Property(x => x.ImageUrl)
            .HasUrlToStringConversion()
            .HasMaxLength(Url.MaxLength);

        builder.OwnsMany(x => x.Options, ConfigureOption);
    }

    private static void ConfigureOption(OwnedNavigationBuilder<Category, Option> builder)
    {
        builder.ToTable("options");

        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, x => new OptionName(x))
            .HasMaxLength(OptionName.MaxLength);

        builder.Property(x => x.Description)
            .HasConversion(x => x.Value, x => new OptionDescription(x))
            .HasMaxLength(OptionDescription.MaxLength);

        builder.Property(x => x.ImageUrl)
            .HasUrlToStringConversion()
            .HasMaxLength(255);

        builder.Property(x => x.Metadata)
            .HasJsonConversion();
    }
}