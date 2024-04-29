using FluentValidation.AspNetCore;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolManagement.Configuration;
using SchoolManagement.DataContext;
using SchoolManagement.Helpers;
using SchoolManagement.Repositories;
using SchoolManagement.Repositories.Impl;
using SchoolManagement.Services;
using SchoolManagement.Services.Impl;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


//AUTH
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("AppSettings:SecretKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//Add DB Context
builder.Services.AddDbContext<SchoolManageDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolManageConnectionString")), ServiceLifetime.Scoped);
//Add Fluent Validatation
builder.Services.AddControllers()
            .AddFluentValidation(v =>
            {
                v.ImplicitlyValidateChildProperties = true;
                v.ImplicitlyValidateRootCollectionElements = true;
                v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });
//Add Transaction 
/*builder.Services.AddScoped<IDbContextTransaction>(provider =>
{
    var dbContext = provider.GetRequiredService<SchoolManageDbContext>();
    return dbContext.Database.BeginTransaction();
});*/

//Add Mapper
builder.Services.AddAutoMapper(typeof(Program));

//Add Scope
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IClassService, ClassService>();
builder.Services.AddTransient<IUserProfileService, UserProfileService>();
builder.Services.AddTransient<IUserCourseService, UserCourseService>();
builder.Services.AddTransient<IUserClassService, UserClassService>();
builder.Services.AddTransient<ISlotService, SlotService>();
builder.Services.AddTransient<IScheduleService, ScheduleService>();
builder.Services.AddTransient<IClassScheduleSlotService, ClassScheduleSlotService>();
builder.Services.AddTransient<AuthHelper>();
builder.Services.AddScoped<ExcelHelper>();
builder.Services.AddScoped<MailHelper>();

//Debugging
builder.Services.AddScoped<IUrlHelper>((x =>
{
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
}));


builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


//Add context accessor
builder.Services.AddHttpContextAccessor();

//Configure Hangfire
builder.Services.AddHangfire(config => config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("SchoolManageConnectionString")));
builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Use Authen
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
//Logging Middleware
app.UseMiddleware<LoggingMiddleware>();

//Add Global ExceptionHandler
app.AddGlobalExceptionHandler();

//Use Hangfire dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    DashboardTitle = "School Management Dashboard",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter()
        {
            Pass = "12345",
            User = "thanhnt"
        }
    }
});
app.UseHangfireServer();
app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<IAuthService>(x => x.SendChangePasswordRemindingMail(), Cron.Daily);

app.Run();
