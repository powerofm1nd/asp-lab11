using Microsoft.AspNetCore.Mvc.Filters;

public class UniqueUserCounterFilter : IAsyncActionFilter
{
    private static readonly HashSet<string> uniqueSessions = new();
    private static readonly object lockObject = new();
    private readonly string logFilePath;

    public UniqueUserCounterFilter(string logFilePath)
    {
        this.logFilePath = logFilePath;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Session.SetString("Initialized", "true");

        var sessionId = context.HttpContext.Session.Id;

        lock (lockObject)
        {
            uniqueSessions.Add(sessionId);
        }

        lock (lockObject)
        {
            File.WriteAllText(logFilePath, $"Кількість унікальних користувачів (по сесіях): {uniqueSessions.Count}");
        }

        await next();
    }
}