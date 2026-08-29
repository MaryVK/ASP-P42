namespace ASP_P42.Data.Middleware.AuthSession
{
    // придерживание традиций ASP - оглашение расширения с Use...
    public static class AuthSessionExtension
    {
        public static IApplicationBuilder UseAuthSession(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthSessionMiddleware>();
        }
    }
}
