const express = require('express');
const request = require('request');
const bodyParser = require('body-parser');
const Agenda = require('agenda');
var Promise = require('promise');
const iotaManager = require("./src/core/iota/iotaManager.js");
const userRepository = require("./src/core/user/userRepository.js");
const createTransactionJob = require("./src/core/iota/createTransactionJob.js");
const sendTipJob = require("./src/core/sendTipJob.js");
require('dotenv').config()

const app = express()
const PORT = process.env.PORT || 3000;
var db;
var agenda;

var MongoClient = require('mongodb').MongoClient;
var mongoConnectionString = process.env.MONGO_CONNECTION_STRING;

// iotaManager.init();

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
    
    Agenda({ db: { address: mongoConnectionString, collection: 'jobs' } });

    agenda.define('sendTip', async function (job, done) {
        await sendTipJob.execute(job.attrs.data.slackCommand, db);
        done();
    });

    // agenda.define('createtransaction', function (job, done) {
    //     createTransactionJob.execute(job.attrs.data.sourceSeed, job.attrs.data.targetSeed);
    //     done();

    //     // doSomelengthyTask(function(data) {
    //     //   formatThatData(data);
    //     //   sendThatData(data);
    //     //   done();
    //     // });
    // });

    agenda.on('ready', function () {
        // agenda.every('3 minutes', 'test job');
        agenda.start();
    });
});

app.get('/', async function (req, res) {
    var t = await iotaManager.getNodeInfo();

    // var bundle = await iotaManager.sendIotas(NKAFTGEHJSGURVSEAUYPEDNPULRGZBQDOPXKACXLEJQXQDYNMYBWULHCNEQAFZBAVJLQDBKDRHIEPMSSD, NKAFTGEHJSGURVSEAUYPEDNPULRGZBQDOPXKACXLEJQXQDYNMYBWULHCNEQAFZBAVJLQDBKDRHIEPMSSA, 0)

    res.status(200).send(e);
});

app.post('/api/tipwallet/info', async function (req, res) {

    var user = {
        slackId: "test66",
        seed: "seed123"
    };

    var response = await userRepository.upsertUser(db, user);
    console.log(response);

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

app.post('/api/tipwallet/sendtip', async function (req, res) {
    // console.log(req);

    if (req.body.token !== process.env.SLACK_VERIFICATION_TOKEN) {
        res.status(500).send('invalid token');
        return;
    }

    console.log('Slack token is valid')

    request.post(
        req.body.response_url,
        { json: { text: 'Send tip command processing' } },
        function (error, response, body) {
            if (!error && response.statusCode == 200) {
                console.log(body)
            }
        }
    );

    agenda.now('sendTip', {
        slackCommand: req.body
    });

    res.status(200);
})

