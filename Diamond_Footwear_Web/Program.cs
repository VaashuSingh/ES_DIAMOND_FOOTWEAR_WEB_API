using Diamond_Footwear_Services.DBContext;
using Diamond_Footwear_Services.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DiamondFootwearWebContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Constr")));


//string connectionString = "Server=YourServerName;Database=YourDatabaseName;User Id=YourUsername;Password=YourPassword;";
string connectionString = "Server = ESLAPI\\SQL2024 ; Database = Diamond_Footwear_Web; User Id = SA; Password = 1234; trusted_Connection = False; Encrypt = false; Integrated Security = false; MultipleActiveResultSets=true";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        try
        {
            // Connection Open
            connection.Open();
            Console.WriteLine("Connection Established Successfully!");

            // Perform your database operations here

            // Connection Close
            connection.Close();
            Console.WriteLine("Connection Closed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }


builder.Services.AddTransient<IRepository, Repository>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
