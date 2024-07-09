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
using System.ComponentModel.DataAnnotations;

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
    public async Task<string> readEmailsAsync(string folder, int index)
    {
        string token = this.context.HttpContext.Session.GetString("access_token");
        if (token == null)
        {
            Response.Redirect("http://localhost:81/api/auth/login");
            return "Failure";
        }

        int pageSize = 10;

        var oauth2 = new SaslMechanismOAuth2("will.woodward100", token);
        string jsonString;
    
        using (var emailClient = new ImapClient()) 
        {
            await emailClient.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            await emailClient.AuthenticateAsync(oauth2);

            var inbox = emailClient.GetFolder(folder);
            inbox.Open(FolderAccess.ReadOnly);

            List<EmailPreview> emailList = new List<EmailPreview>();

            Console.WriteLine ("Total messages: {0}", inbox.Count);
            Console.WriteLine ("Recent messages: {0}", inbox.Recent);
            int lastIndex = inbox.Count - 1;

            // There is a bug where if it is pressed on the last row, it will return the last email
            int lowerIndex = lastIndex - ((index * pageSize) + pageSize - 1);
            if (lowerIndex < 0) lowerIndex = 0;

            int upperIndex = lastIndex - (index * pageSize);
            if (upperIndex < 0) upperIndex = 0;

            // Getting the mail summary
            foreach (var summary in (await inbox.FetchAsync(lowerIndex, upperIndex, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope)).Reverse())
            {
                EmailPreview envelope = new EmailPreview();
                envelope.sender = summary.Envelope.From.ToString();
                envelope.subject = summary.Envelope.Subject;
                envelope.date = summary.Date.ToString();
                envelope.id = summary.UniqueId.ToString();
                emailList.Add(envelope);
            }

            jsonString = JsonSerializer.Serialize(emailList);

            await emailClient.DisconnectAsync (true);
        };

        return jsonString;
    }

    [HttpGet("folders")]
    public async Task<string> getFolders()
    {
        string token = this.context.HttpContext.Session.GetString("access_token");
        if (token == null)
        {
            Response.Redirect("http://localhost:81/api/auth/login");
        }

        var oauth2 = new SaslMechanismOAuth2("will.woodward100", token);
        List<string> jsonString = new List<string>();
    
        using (var emailClient = new ImapClient()) 
        {
            await emailClient.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            await emailClient.AuthenticateAsync(oauth2);

            var inbox = emailClient.Inbox;
            inbox.Open (FolderAccess.ReadOnly);

            // Get the first personal namespace and list the toplevel folders under it.
            var personal = emailClient.GetFolder(emailClient.PersonalNamespaces[0]);

            foreach (var folder in personal.GetSubfolders(false))
            {
                if (folder.Name == "[Gmail]")
                {
                    foreach (var subfolder in folder.GetSubfolders(false)) {
                        jsonString.Add("[Gmail]/" + subfolder.Name);
                    }
                } else
                {
                    jsonString.Add(folder.Name);
                }
            }

            await emailClient.DisconnectAsync (true);
        };

        string resp = JsonSerializer.Serialize(jsonString);
        return resp;
    }

    [HttpGet("email")]
    public async Task<string> getEmail(string folder, int emailID)
    {
        string token = this.context.HttpContext.Session.GetString("access_token");
        if (token == null)
        {
            Response.Redirect("http://localhost:81/api/auth/login");
            return "Failure";
        }

        int pageSize = 10;

        var oauth2 = new SaslMechanismOAuth2("will.woodward100", token);
        string jsonString;
    
        using (var emailClient = new ImapClient()) 
        {
            await emailClient.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            await emailClient.AuthenticateAsync(oauth2);

            var inbox = emailClient.GetFolder(folder);
            inbox.Open(FolderAccess.ReadOnly);

            var message = inbox.GetMessage(emailID - 1);
            Email email = new Email();
            email.sender = message.From.ToString();
            email.subject = message.Subject;
            email.body = message.HtmlBody;
            email.date = message.Date.ToString();

            jsonString = JsonSerializer.Serialize(email);

            await emailClient.DisconnectAsync(true);
        };

        return jsonString;
    }
}