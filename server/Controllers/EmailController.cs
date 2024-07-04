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
using server;
using System.Text.Json;

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
        string token = this.context.HttpContext.Session.GetString("access_token");
        if (token == null)
        {
            Response.Redirect("http://localhost:81/api/auth/login");
            return "Failure";
        }

        var oauth2 = new SaslMechanismOAuth2("will.woodward100", token);
        string jsonString;
    
        using (var emailClient = new ImapClient()) 
        {
            await emailClient.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            await emailClient.AuthenticateAsync(oauth2);

            var inbox = emailClient.Inbox;
            inbox.Open (FolderAccess.ReadOnly);

            List<Email> emailList = new List<Email>();

            Console.WriteLine ("Total messages: {0}", inbox.Count);
            Console.WriteLine ("Recent messages: {0}", inbox.Recent);

            // foreach (var summary in inbox.Fetch (0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure)) {
            //     if (summary.HtmlBody != null) {
            //         // this will download *just* the text/html part
            //         var html = inbox.GetBodyPart(summary.UniqueId, summary.HtmlBody);
            //         Console.WriteLine(html);
            //     }
            // }

            for (int i = inbox.Count - 1; i > inbox.Count - 11; i--) {
                var message = inbox.GetMessage(i);
                Email email = new Email();
                email.sender = message.From.ToString();
                email.subject = message.Subject;
                email.body = message.HtmlBody;
                email.date = message.Date.ToString();
                emailList.Add(email);
            }

            jsonString = JsonSerializer.Serialize(emailList);

            await emailClient.DisconnectAsync (true);
        };

        return jsonString;
    }
}