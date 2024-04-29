using SchoolManagement.DataContext;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Configuration
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, SchoolManageDbContext dbContext)
        {
            // Log and save action to the database
            LogActionToDatabase(context, dbContext);

            await _next(context);
        }

        private void LogActionToDatabase(HttpContext context, SchoolManageDbContext dbContext)
        {
            // Retrieve relevant information from the HttpContext
            var action = GetAction(context.Request.Method);
            var url = context.Request.Path.Value;
            var ipAddress = context.Connection.RemoteIpAddress.ToString();
            var timestamp = DateTime.Now;
            var userId = context.User.Identity.Name; // Assuming user id is stored in the User.Identity.Name property

            // Save the action to the database
            dbContext.Logs.Add(new LogEntry { Action = action, IpAddress = ipAddress, Timestamp = timestamp, UserId = userId, Url = url });
            dbContext.SaveChanges();

            // Log the action to your preferred logging provider
            _logger.LogInformation($"Action '{action}' from IP '{ipAddress}' by User ID '{userId}' logged at '{timestamp}' for URL '{url}'.");
        }

        private string GetAction(string method)
        {
            switch (method)
            {
                case "POST":
                    return "Create";
                case "PUT":
                    return "Update";
                case "DELETE":
                    return "Delete";
                case "GET":
                default:
                    return "View";
            }
        }
    }
}
