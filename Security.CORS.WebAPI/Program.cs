using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    //herhangi bir domainden tüm gelen istekleri kabul et anlamýna gelir
    //opt.AddDefaultPolicy(build =>
    //{
    //    build.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    //});

    //belirlediðimiz domainler izin verme
    opt.AddPolicy("AllowSites", builder =>
    {
        builder.WithOrigins("https://localhost:7276", "http://localhost:5086").AllowAnyHeader().AllowAnyMethod();
    });

    opt.AddPolicy("AllowSites2", builder =>
    {
        builder.WithOrigins("https://localhost:72761", "http://localhost:50861").WithHeaders(HeaderNames.ContentType, "x-custom-header");
    });

    opt.AddPolicy("AllowSites3", builder =>
    {
        //tüm subdomianleri kabul et izin ver
        builder.WithOrigins("https://*.example.com")
        .SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod();
    });

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.UseCors();
app.UseCors("AllowSites");
app.UseCors("AllowSites2");

app.Run();
