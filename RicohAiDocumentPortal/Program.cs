using RicohAiDocumentPortal.Models;
using RicohAiDocumentPortal.Services;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
    .AddSessionStateTempDataProvider();

builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection("GeminiSettings"));

builder.Services.AddScoped<IDocumentAnalysisService, DocumentAnalysisService>();
builder.Services.AddScoped<IDocumentChatService, DocumentChatService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(8);
});

// Register Supabase client
var supabaseUrl = builder.Configuration["Supabase:Url"]!;
var supabaseKey = builder.Configuration["Supabase:Key"]!;

builder.Services.AddSingleton(_ =>
    new Supabase.Client(supabaseUrl, supabaseKey, new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = false
    })
);

builder.Services.AddScoped<SupabaseService>();

var app = builder.Build();

// Initialize Supabase
var supabase = app.Services.GetRequiredService<Supabase.Client>();
await supabase.InitializeAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapGet("/", async context =>
{
    var isAuthenticated = context.Session.GetString("IsAuthenticated");

    if (isAuthenticated == "true")
    {
        context.Response.Redirect("/Home");
    }
    else
    {
        context.Response.Redirect("/Account/Printer");
    }

    await Task.CompletedTask;
});
app.MapRazorPages();

app.Run();
