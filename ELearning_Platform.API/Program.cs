using ELearning_Platform.API.Hubs;
using ELearning_Platform.API.Middleware;
using ELearning_Platform.Application.Extensions;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Infrastructure.Database;
using ELearning_Platform.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDataProtection();
builder.Services.AddInfrastructure(builder.Configuration); //for database and repository registration 
builder.Services.AddIdentity<Account, Roles>(setup =>
{
    setup.User.RequireUniqueEmail = true;
    setup.Password.RequireNonAlphanumeric = true;
    setup.Password.RequireUppercase = true;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<PlatformDb>();
builder.Services.AddApplication(); //MediatR and Validations 
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddCors(pr => pr.AddPolicy("corsPolicy", options =>
{
    options.
     AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
}));
builder.Services.AddScoped<SeederDb>();
var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<SeederDb>();
await seeder.GenerateRolesAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("corsPolicy");
app.MapHub<Notification>("hub/notifications");
app.UseAuthorization(); //Add to Avoid problem with Identity  
app.UseAuthentication();
app.MapControllers();
app.Run();


