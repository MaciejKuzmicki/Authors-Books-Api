using ApiWebClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClient();

// Configure the authentication services with a default scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies"; // Set Google as the default scheme
    options.DefaultChallengeScheme = "Google";
}).AddCookie("Cookies")
.AddGoogle(options =>
{
    options.ClientId = "";
    options.ClientSecret = "";
    options.CallbackPath = "/signin-google";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // This should be before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
