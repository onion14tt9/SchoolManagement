namespace SchoolManagement.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalExceptionHandler(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
