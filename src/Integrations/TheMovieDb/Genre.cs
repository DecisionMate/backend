using JetBrains.Annotations;

namespace DecisionMate.Integrations.TheMovieDb;

[PublicAPI]
public sealed record Genre(int Id, string Name);