using System;
using Microsoft.AspNetCore.Mvc;

using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Net.Imap;
using MailKitSimplified.Receiver;

using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Google.Apis.Util;
using MailKit.Security;
using Google.Apis.Auth.OAuth2.Web;

[ApiController]
[Route("/api/account")]
public class EmailController : ControllerBase
{
    [HttpGet("login")]
    public string readEmailsAsync()
    {
        using (var client = new ImapClient (new ProtocolLogger ("imap.log"))) {
            client.Connect ("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

            client.Authenticate ("willwoodward100", "ILoveP3nny!");
        }
        return "Done";
        // const string GMailAccount = "willwoodward100@gmail.com";

        // var clientSecrets = new ClientSecrets {
        //     ClientId = "1022259876690-r9qd5va4upo28bacop20h86n29k6rhca.apps.googleusercontent.com",
        //     ClientSecret = "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"
        // };

        // var codeFlow = new GoogleAuthorizationCodeFlow (new GoogleAuthorizationCodeFlow.Initializer {
        //     DataStore = new FileDataStore ("CredentialCacheFolder", false),
        //     Scopes = new [] { "https://mail.google.com/" },
        //     ClientSecrets = clientSecrets
        // });

        // // Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
        // var codeReceiver = new LocalServerCodeReceiver ();
        // var authCode = new AuthorizationCodeWebApp(codeFlow, "http://localhost:5108/api/account/login", "http://localhost:5108/api/account/login");

        // var credential = await authCode.AuthorizeAsync (GMailAccount, CancellationToken.None);

        // if (credential.token.IsExpired (SystemClock.Default))
        //     await credential.RefreshTokenAsync (CancellationToken.None);

        // var oauth2 = new SaslMechanismOAuth2 (credential.UserId, credential.Token.AccessToken);

        // using (var client = new ImapClient ()) {
        //     await client.ConnectAsync ("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
        //     await client.AuthenticateAsync (oauth2);
        //     await client.DisconnectAsync (true);
        // }
    }
}