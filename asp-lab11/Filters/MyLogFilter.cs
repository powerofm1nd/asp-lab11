using Microsoft.AspNetCore.Mvc.Filters;

namespace asp_lab11.Filters;

public class MyLogFilter : Attribute, IAsyncActionFilter
{
    private readonly string _pathToLogFile;
    private readonly object _lock = new object();
    public MyLogFilter(string pathToLogFile)
    {
        _pathToLogFile = pathToLogFile;
    }
    
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var message = $"Controller: {context.RouteData.Values["controller"]} Action: {context.RouteData.Values["action"]} Time: {DateTime.Now.ToString("HH:mm:ss")} \n";
        lock(_lock) { File.AppendAllText(_pathToLogFile, message); }
        return next();
    }
}