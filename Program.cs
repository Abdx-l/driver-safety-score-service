using Dapper;using Npgsql;
var builder=WebApplication.CreateBuilder(args);var app=builder.Build();
app.MapPost("/score",(Trip trip)=>{try{return Results.Ok(Scoring.Calculate(trip));}catch(ArgumentException e){return Results.BadRequest(new{error=e.Message});}});
app.MapGet("/drivers/lowest",async(DateOnly month)=>{await using var db=new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE_URL"));return await db.QueryAsync("SELECT driver_id,avg(score) score FROM trips WHERE occurred>=@start AND occurred<@end GROUP BY driver_id ORDER BY score,driver_id LIMIT 10",new{start=month.ToDateTime(TimeOnly.MinValue),end=month.AddMonths(1).ToDateTime(TimeOnly.MinValue)});});app.Run();
public record Sample(double Seconds,double SpeedKmh,double LimitKmh);
public record Trip(double DistanceKm,Sample[] Samples);
public record Score(double Value,int Braking,int Acceleration,int Speeding);
public static class Scoring{
 public static Score Calculate(Trip trip,double brakingWeight=4,double accelerationWeight=3,double speedingWeight=2){
 if(!double.IsFinite(trip.DistanceKm)||trip.DistanceKm<=0||trip.Samples.Length<2)throw new ArgumentException("positive distance and two samples required");
 int braking=0,acceleration=0,speeding=0;
 for(int i=0;i<trip.Samples.Length;i++){var s=trip.Samples[i];if(!double.IsFinite(s.SpeedKmh)||!double.IsFinite(s.Seconds)||s.SpeedKmh<0||!double.IsFinite(s.LimitKmh)||s.LimitKmh<0)throw new ArgumentException("invalid sample");if(s.SpeedKmh>s.LimitKmh)speeding++;
 if(i==0)continue;var previous=trip.Samples[i-1];var dt=s.Seconds-previous.Seconds;if(dt<=0)throw new ArgumentException("timestamps must increase");var g=(s.SpeedKmh-previous.SpeedKmh)/3.6/dt/9.80665;if(g<-.4)braking++;if(g>.4)acceleration++;}
 return new(Math.Clamp(100-(braking*brakingWeight+acceleration*accelerationWeight+speeding*speedingWeight)*100/trip.DistanceKm,0,100),braking,acceleration,speeding);
 }}
