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
[Route("/api/email")]
public class EmailController : ControllerBase
{
    private readonly IHttpContextAccessor context;

    public EmailController(IHttpContextAccessor context)
    {
        this.context = context;
    }

    [HttpGet("emails")]
    public async Task<string> readEmailsAsync()
    {
        var oauth2 = new SaslMechanismOAuth2 ("will.woodward100", this.context.HttpContext.Session.GetString("access_token"));
    
        using (var emailClient = new ImapClient ()) 
        {
            await emailClient.ConnectAsync ("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            await emailClient.AuthenticateAsync (oauth2);

            var inbox = emailClient.Inbox;
            inbox.Open (FolderAccess.ReadOnly);

            Console.WriteLine ("Total messages: {0}", inbox.Count);
            Console.WriteLine ("Recent messages: {0}", inbox.Recent);

            for (int i = inbox.Count - 1; i > inbox.Count - 11; i--) {
                var message = inbox.GetMessage (i);
                Console.WriteLine ("Subject: {0}", message.Date);
            }

            await emailClient.DisconnectAsync (true);
        };

        return "Success";
    }
}