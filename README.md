# Driver Safety Score Service

ASP.NET Core driving event scoring API with strict 0.4 g acceleration/braking thresholds, distance-normalized configurable calculation weights, score clamping, boundary tests, and a PostgreSQL/Dapper monthly lowest-score query.

## Run

`dotnet test Tests/Tests.csproj`; `dotnet run`. POST /score with distanceKm and samples of seconds/speedKmh/limitKmh. Run schema.sql and set DATABASE_URL for the monthly query.
