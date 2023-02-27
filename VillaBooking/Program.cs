using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VillaBooking.Data;
using VillaBooking.Helper;
using VillaBooking.Repository;
using VillaBooking.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=identifier.sqlite;");
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable=true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy("corsname", builder =>
{
    builder.WithOrigins("http://localhost:5280")
        .AllowAnyHeader().AllowAnyMethod();
}));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("corsname");
app.UseAuthorization();
app.MapControllers();
app.Run();