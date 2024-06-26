var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});

var app = builder.Build();

app.UseHttpsRedirection ();
app.UseStaticFiles ();
app.UseSession();
	
app.UseRouting ();

app.UseAuthentication ();
app.UseAuthorization ();

app.MapControllers();

app.Run();
