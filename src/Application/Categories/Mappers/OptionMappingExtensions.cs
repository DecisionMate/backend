using DecisionMate.Application.Categories.Contracts;
using DecisionMate.Domain.Categories.Options;
using Riok.Mapperly.Abstractions;
using static Riok.Mapperly.Abstractions.RequiredMappingStrategy;

namespace DecisionMate.Application.Categories.Mappers;

[Mapper]
public static partial class OptionMappingExtensions
{
    [MapperRequiredMapping(Source)]
    public static partial Option Map(this OptionContract contract, int id);

    [MapperRequiredMapping(Target)]
    public static partial OptionModel Map(this Option option);
}