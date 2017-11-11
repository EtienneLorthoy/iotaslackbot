const express = require('express');
const request = require('request');
const bodyParser = require('body-parser');
const Agenda = require('agenda');
var Promise = require('promise');
const iotaManager = require("./src/core/iota/iotaManager.js");
// const { createUser } = require("./src/core/user/userRepository.js");
require('dotenv').config()

const app = express()
const PORT = process.env.PORT || 3000;
var db;
var agenda;

var MongoClient = require('mongodb').MongoClient;
var mongoConnectionString = process.env.MONGO_CONNECTION_STRING;

iotaManager.init();

app.use(bodyParser.urlencoded({ extended: false }));

MongoClient.connect(mongoConnectionString, function (err, database) {
    if (err) {
        console.log(err);
        process.exit(1);
    }

    db = database;

    console.log("Database connection ready");

    app.listen(PORT, function () {
        console.log(`Server is running on port ${PORT}`)
    });

    agenda = new Agenda({ db: { address: mongoConnectionString, collection: 'jobs' } });

    agenda.on('ready', function () {
        agenda.every('3 minutes', 'test job');
        agenda.start();
    });
});

app.get('/', async function(req, res) {
    var t = await iotaManager.getNodeInfo();

    res.status(200).send(t);
});

app.post('/api/tipwallet/info', async function (req, res) {

    var user = {
        slackId: "test123",
        seed: "seed123"
    };

    var response = await db.collection('users').insert(user);

    // var user = createUser(db, );

    console.log(response.ops[0]);

    res.send('tipwallet info!');
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

    res.status(500).send('invalid token');
})

app.post('/api/tipwallet/withdraw', function (req, res) {
    res.send('tipwallet withdraw!');
})

app.post('/api/tipwallet/sendtip', function (req, res) {
    var newSeed = iotaManager.generateNewSeed();
    res.send(newSeed);
})

