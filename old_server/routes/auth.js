const express = require("express");
const router = express.Router();
const jwt = require('jsonwebtoken');

router.get('/google', async (req, res) => {
    try {
        // Process Google's OAuth response
        if (req.query.error) return res.redirect('/');

        const code = req.query.code;

        const authURL = new URLSearchParams({
            client_id: '1022259876690-r9qd5va4upo28bacop20h86n29k6rhca.apps.googleusercontent.com',
            client_secret: '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',
            code,
            grant_type: 'authorization_code',
            redirect_uri: 'http://localhost:81/auth/google'
        })

        const response = await fetch('https://oauth2.googleapis.com/token', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: authURL.toString()
        });
        const token = await response.json()
        
        const jsonToken = jwt.decode(token.id_token);
        req.session.email = jsonToken.email;

        if (token.error) return res.redirect('/');

        // Store the access token
        req.session.accessToken = token.access_token;

        // Store the expiry date
        const date = new Date();
        date.setSeconds(date.getSeconds() + token.expires_in);
        req.session.expiryDate = date;

        return res.redirect('/'); 
    } catch (e) {
        res.json(e);
        return res.status(400).send();
    }
});

router.get('/verify', (req, res) => {
    if (req.session.accessToken && new Date(req.session.expiryDate) > new Date()) {
        return res.status(200).end();
    }

    return res.status(401).end();
});

module.exports = router;