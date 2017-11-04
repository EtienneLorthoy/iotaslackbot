const express = require('express');
const iota = require('iota.lib.js');
const request = require('request');
const bodyParser = require('body-parser');
const Agenda = require('agenda');
const iotaManager = require("./src/core/iota/iotaManager.js");
require('dotenv').config()

const app = express()
const PORT = process.env.PORT || 3000;

var MongoClient = require('mongodb').MongoClient;
var mongoConnectionString = process.env.MONGO_CONNECTION_STRING;

app.use(bodyParser.urlencoded({ extended: false }));

app.get('/', iotaManager.getNodeInfo)

app.post('/api/tipwallet/info', function (req, res) {
    
    console.error("starting info");

    MongoClient.connect(mongoConnectionString, function (err, db) {
        console.log("Connected successfully to server");
    
        console.error("connected to db"); 
        db.close();
    });
    
    console.error("starting agenda"); 
    var agenda = new Agenda({ db: { address: mongoConnectionString, collection: 'jobs' } });
    
    agenda.on('ready', function () {
        agenda.every('3 minutes', 'test job');
        agenda.start();
    });

    res.send('tipwallet info!')
})

app.post('/api/tipwallet/deposite', function (req, res) {

    console.log(req.body);

    if (req.body.token === process.env.SLACK_VERIFICATION_TOKEN) {

        request.post(
            req.body.response_url,
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