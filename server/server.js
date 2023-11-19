const express = require('express');
const session = require('express-session');
const bodyParser = require('body-parser');

const app = express();

app.use(bodyParser.json());

app.use(session({
    secret: 'change-this-in-production',
    resave: false,
    saveUninitialized: false,
    cookie: {
        sameSite: true
    }
}));

// Loading routes
const api = require('./routes/api.js');
const authenticator = require('./routes/auth.js');
app.use('/api', api);
app.use('/auth', authenticator);

app.listen(8090);

module.exports = app;