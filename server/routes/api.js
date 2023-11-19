const express = require("express");
const router = express.Router();
const { ImapFlow } = require('imapflow');
const quotedPrintable = require('quoted-printable');
const utf8 = require('utf8');

// Global variables
const IMAPHost = 'imap.gmail.com';

class Email {
    constructor(sender, subject, date, body) {
        this.sender = sender;
        this.subject = subject;
        this.date = date;
        this.body = body;
    }
}

router.get('/emails', async (req, res) => {
    try {
        const client = new ImapFlow({
            host: IMAPHost,
            port: 993,
            secure: true,
            logger: false,
            auth: {
                user: req.session.email,
                accessToken: req.session.accessToken
            }
        });
    
        let emails = [];
        await client.connect();

        let lock = await client.getMailboxLock('INBOX');
        try {
            await client.fetchOne(client.mailbox.exists, { source: true });
            for await (let message of client.fetch('1:10', { envelope: true, source: true, bodyStructure: true, headers: true })) {
                let sliced_source = message.source.toString()
                // Sometimes AMP emails don't load https://amp.dev/documentation/guides-and-tutorials/learn/email_fundamentals

                // Find the body part with plain text
                let searchLength = `Content-Type: text/plain; charset=\"UTF-8\"\r\n\r\n`.length;
                let searchText = RegExp(`Content-Type: text/plain; charset=\"UTF-8\"\r\n\r\n`);

                // Checks to see if plain text isn't found
                let index1 = sliced_source.search(searchText);
                if (index1 == -1) {
                    let searchLength1 = `<!doctype html>`.length;
                    let searchText1 = RegExp(`<!doctype html>`, 'i');
                    // Should decode here, throws invalid continuation byte error
                    let sliced_source_text1 = sliced_source.toString();
                    let index3 = sliced_source_text1.search(searchText1);

                    index3 += searchLength1;

                    let index2 = sliced_source_text1.search(RegExp(`</html`));
                    index2 += `</html`.length;

                    sliced_source = utf8.decode(quotedPrintable.decode(sliced_source_text1.slice(index3, index2)));

                    // AMP emails have a carriage return after the <!doctype> definition
                    if (sliced_source[0].search(/\r/) === 0) {
                        sliced_source = sliced_source.slice(2);
                    }

                    // Cannot render AMP email format yet
                    if (sliced_source.startsWith('<html ⚡4email')) {
                        sliced_source = 'This is in AMP format, which is currently not supported.';
                    }

                    emails.push(new Email(message.envelope.from[0].name, message.envelope.subject, message.envelope.date, sliced_source));
                } else {
                    index1 += searchLength;

                    let index2 = message.bodyStructure.childNodes.find(({type}) => type = 'text/plain').size + index1 - 2;
                    sliced_source = sliced_source.slice(index1, index2);
    
                    const email = new Email(message.envelope.from[0].name, message.envelope.subject, message.envelope.date, sliced_source);
                    emails.push(email);
                }
            }
        } finally {
            lock.release();
            res.json(emails);
        }
        await client.logout();

    } catch (error) {
        res.status(401).send();
    }
});

router.get('/login', (req, res) => {
    // Generate Google OAuth link
    const authURL = new URLSearchParams({
        scope: 'https://mail.google.com/ email',
        include_granted_scopes: true,
        response_type: 'code',
        redirect_uri: 'http://localhost:5173/auth/google',
        access_type: 'offline',
        client_id: '1022259876690-r9qd5va4upo28bacop20h86n29k6rhca.apps.googleusercontent.com'
    });

    res.redirect('https://accounts.google.com/o/oauth2/v2/auth?' + authURL.toString());
});

module.exports = router;