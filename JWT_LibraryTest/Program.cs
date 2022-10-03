using JWTAuthLibrary;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJWTAuth(opt =>
{
    opt.IssuerSigningSecret = "xxxx";
    opt.OnValidateUserInfo = (jsonBody, p) =>
    {
        DefaultUserLogin login = JsonSerializer.Deserialize<DefaultUserLogin>(jsonBody,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        // TODO: verify the password is valid.
        UserInfo user = new UserInfo(login.Username, login);
        return Task.FromResult(user);
    };

    opt.OnValidateRoleInfo = (validUser, p) =>
    {
        return Task.FromResult<IEnumerable<string>>(new string[] { validUser.Name });
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseJWTAuth();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}