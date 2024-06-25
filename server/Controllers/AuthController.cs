using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

[ApiController]
[Route("/api/auth")]
public class AuthController: ControllerBase
{
    private readonly IConfiguration _configuration;

    [HttpGet("login")]
    public object login() 
    {
        // Construct Google API endpoint with mail permissions
        string url = "https://accounts.google.com/o/oauth2/v2/auth";
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("scope", "https://mail.google.com/");
        param.Add("include_granted_scopes", "true");
        param.Add("response_type", "code");
        param.Add("redirect_uri", "http://localhost:5108/api/auth/google");
        param.Add("access_type", "offline");
        param.Add("client_id", _configuration["GoogleAuth:client_id"]);

        string uri = QueryHelpers.AddQueryString(url, param);
        return Redirect(uri);
    }

    [HttpGet("google")]
    public async Task<string> google(string code) 
    {
        // Get authorization code
        // Do error handling

        // Get authorization token
        string url = "https://oauth2.googleapis.com/token";
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("redirect_uri", "http://localhost:5108/api/auth/google");
        param.Add("client_id", _configuration["GoogleAuth:client_id"]);
        param.Add("client_secret", _configuration["GoogleAuth:client_secret"]);
        param.Add("code", code);
        param.Add("grant_type", "authorization_code");

        var httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.PostAsync(url, new FormUrlEncodedContent(param));
        string data = await response.Content.ReadAsStringAsync();
        dynamic json  = JsonConvert.DeserializeObject(data);
        data = json.access_token;

        return data;
    }
}