namespace Sonar.UserProfile.Web.Filters;

public static class HttpExtensions
{
    public static Guid GetIdFromItems(HttpContext httpContext)
    {
        var userIdItem = httpContext.Items["SenderUserId"];

        if (userIdItem is null)
        {
            throw new Exception("Incorrect user id item");
        }

        return (Guid)userIdItem;
    }
}