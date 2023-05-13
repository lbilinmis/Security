using Microsoft.Data.SqlClient;
using Security.SecretProtection.WebUI.Settings;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("SqlConnection"));
string connectionString = builder.Configuration["ConnectionStrings:SqlConnection"];
var builderSql = new SqlConnectionStringBuilder(connectionString);

builderSql.UserID = builder.Configuration["UserId:SqlUserId"];
builderSql.Password = builder.Configuration["Password:SqlPassword"];

string connectionStr = builderSql.ConnectionString;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
