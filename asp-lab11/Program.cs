var builder = WebApplication.CreateBuilder(args);
string logFilePath = "unique_users_count.txt";

builder.Services.AddControllersWithViews();

//Підключення фільтру для підрахунку унікальних сессій
builder.Services.AddControllers(options =>
{
    options.Filters.Add( new UniqueUserCounterFilter(logFilePath));
});

//Налаштування сессій
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();