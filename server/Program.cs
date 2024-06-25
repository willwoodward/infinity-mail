var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// builder.Services.AddAuthentication (options => {
//     // This forces challenge results to be handled by Google OpenID Handler, so there's no
//     // need to add an AccountController that emits challenges for Login.
//     options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    
//     // This forces forbid results to be handled by Google OpenID Handler, which checks if
//     // extra scopes are required and does automatic incremental auth.
//     options.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    
//     // Default scheme that will handle everything else.
//     // Once a user is authenticated, the OAuth2 token info is stored in cookies.
//     options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
// })
// .AddCookie (options => {
//     options.ExpireTimeSpan = TimeSpan.FromMinutes (5);
// })
// .AddGoogleOpenIdConnect (options => {
//     var secrets = GoogleClientSecrets.FromFile ("client_secret.json").Secrets;
//     options.ClientId = secrets.ClientId;
//     options.ClientSecret = secrets.ClientSecret;
// });

var app = builder.Build();

app.UseHttpsRedirection ();
app.UseStaticFiles ();
	
app.UseRouting ();

app.UseAuthentication ();
app.UseAuthorization ();

app.MapControllers();

app.Run();
