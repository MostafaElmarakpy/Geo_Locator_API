using GeoLocator.Application.Interfaces;
using GeoLocator.Infastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  MOVE ALL SERVICE REGISTRATIONS HERE 
builder.Services.AddHttpClient<IGeolocationRepository, GeolocationRepository>(client =>
{
    client.BaseAddress = new Uri("https://api.ipgeolocation.io/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ICountryService, CountryService>(client =>
{
    client.BaseAddress = new Uri("https://restcountries.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "Countries-API-Client");
});

builder.Services.AddSingleton<IBlockedCountryRepository, BlockedCountryRepository>();
builder.Services.AddSingleton<IBlockedAttemptsRepository, BlockedAttemptsRepository>();
builder.Services.AddHostedService<TemporaryBlockRepository>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
