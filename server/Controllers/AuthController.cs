using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

[ApiController]
[Route("/api/auth")]
public class AuthController: ControllerBase
{
    private readonly IHttpContextAccessor context;

    public AuthController(IHttpContextAccessor context)
    {
        this.context = context;
    }

    [HttpGet("login")]
    public object login() 
    {
        // Construct Google API endpoint with mail permissions
        string url = "https://accounts.google.com/o/oauth2/v2/auth";
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("scope", "https://mail.google.com/");
        param.Add("include_granted_scopes", "true");
        param.Add("response_type", "code");
        param.Add("redirect_uri", "http://localhost:81/api/auth/google");
        param.Add("access_type", "offline");
        param.Add("client_id", "1022259876690-r9qd5va4upo28bacop20h86n29k6rhca.apps.googleusercontent.com");

        string uri = QueryHelpers.AddQueryString(url, param);
        return Redirect(uri);
    }

    [HttpGet("google")]
    public async Task google(string code) 
    {
        // Get authorization code
        // Do error handling

        // Get authorization token
        string url = "https://oauth2.googleapis.com/token";
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("redirect_uri", "http://localhost:81/api/auth/google");
        param.Add("client_id", "1022259876690-r9qd5va4upo28bacop20h86n29k6rhca.apps.googleusercontent.com");
        param.Add("client_secret", "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");
        param.Add("code", code);
        param.Add("grant_type", "authorization_code");

        var httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.PostAsync(url, new FormUrlEncodedContent(param));
        string data = await response.Content.ReadAsStringAsync();
        dynamic json  = JsonConvert.DeserializeObject(data);
        data = json.access_token;

        this.context.HttpContext.Session.SetString("access_token", data);

        Response.Redirect("http://localhost:81");
    }

    [HttpGet("verify")]
    public void verify()
    {
        string token = this.context.HttpContext.Session.GetString("access_token");
        if (token != null)
        {
            Response.StatusCode = 200;
            return;
        }

        Response.StatusCode = 401;
        return;
    }
}