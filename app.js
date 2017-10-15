const express = require('express')
const request = require('request');
const bodyParser = require('body-parser');
require('dotenv').config()

const app = express()
const PORT = process.env.PORT || 3000;


app.use(bodyParser.urlencoded({ extended: false }));

app.get('/', function (req, res) {
  res.send('Hello tipwallet!')
})

app.post('/api/tipwallet/info', function (req, res) {
    res.send('tipwallet info!')
})

app.post('/api/tipwallet/deposite', function (req, res) {

    console.log(req.body);

    if (req.body.token === process.env.SLACK_VERIFICATION_TOKEN)  {

        request.post(
            req.bodyresponse_url,
            { json: { text: 'test deposite' } },
            function (error, response, body) {
                if (!error && response.statusCode == 200) {
                    console.log(body)
                }
            }
        );

        res.send('');
        return;
    }

    res.status(500).send('invalid token')
})

app.post('/api/tipwallet/withdraw', function (req, res) {
    res.send('tipwallet withdraw!')
})

app.post('/api/tipwallet/sendtip', function (req, res) {
    res.send('tipwallet sendtip!')
})

app.listen(PORT, function () {
    console.log(`Server is listening on port ${PORT}`)
})