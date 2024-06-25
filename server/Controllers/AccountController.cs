// using System.Security.Claims;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.Google;
// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("/api/account")]
// public class AccountController : ControllerBase
// {
//     [HttpGet("login")]
//     public IActionResult Login()
//     {
//         var props = new AuthenticationProperties { RedirectUri = "signin-google" };
//         return Challenge(props, GoogleDefaults.AuthenticationScheme);
//     }
//     [HttpGet("signin-google")]
//     public async Task<IActionResult> GoogleLogin()
//     {
//       var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//       if (response.Principal == null) return BadRequest();

//       var name = response.Principal.FindFirstValue(ClaimTypes.Name);
//       var givenName = response.Principal.FindFirstValue(ClaimTypes.GivenName);
//       var email = response.Principal.FindFirstValue(ClaimTypes.Email);
//       //Do something with the claims
//       // var user = await UserService.FindOrCreate(new { name, givenName, email});
//       Console.WriteLine(email);

//       return Ok();
//     }
// }