using lab1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// This part of code is for testing the database connection
TestDbConnection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

void TestDbConnection()
{
    string connectionString = "Server=tcp:bohush-server.database.windows.net,1433;Initial Catalog=bohush;Persist Security Info=False;User ID=bohush;Password=1qaz!QAZ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT 1", conn);
            var result = command.ExecuteScalar();
            Console.WriteLine("Підключення успішне: " + result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка підключення: " + ex.Message);
        }
    }
}