const express = require('express');
const request = require('request');
const bodyParser = require('body-parser');
const Agenda = require('agenda');
var Promise = require('promise');
const iotaManager = require("./src/core/iota/iotaManager.js");
const userRepository = require("./src/core/user/userRepository.js");
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

app.get('/', async function (req, res) {
    var t = await iotaManager.getNodeInfo();

    var bundle = await iotaManager.sendIotas(NKAFTGEHJSGURVSEAUYPEDNPULRGZBQDOPXKACXLEJQXQDYNMYBWULHCNEQAFZBAVJLQDBKDRHIEPMSSD, NKAFTGEHJSGURVSEAUYPEDNPULRGZBQDOPXKACXLEJQXQDYNMYBWULHCNEQAFZBAVJLQDBKDRHIEPMSSA, 0)

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
    console.log(req);

    if (req.body.token !== process.env.SLACK_VERIFICATION_TOKEN) {
        res.status(500).send('invalid token');
        return;
    }
    console.log('Slack token is valid');

    var slackId = `${req.body.team_id}_${req.body.user_id}`;
    var user = await userRepository.getUser(db, slackId);

    if (user === null)
    {
        console.log(`Unknow slackid:${slackId} create new user.`);
        var newSeed = await iotaManager.generateNewSeed();

        newUser = {
            slackId: slackId,
            seed: newSeed
        };

        user = await userRepository.createUser(db, newUser);
        console.log(`User ${user.slackId} created.`);

        var slackResponse = await request.post(
            req.body.response_url,
            {
                json: {
                    text: `seed:${newSeed}`,
                    response_type: "ephemeral", // "in_channel" => visible to all
                }
            }
        );
        console.log(`New seed sent to the created user.`);
        
        res.send();
    } else {
        // ex : "<@123444|bob>"
        var myRegexp = /<@([^\|]*)|([^>]*)>/g;
        var match = myRegexp.exec(req.body.text);
        console.log(match[1]);

        var targetSlackId = `${req.body.team_id}_${match[1]}`;

        var targetUser = await userRepository.getUser(db, targetSlackId);

        if (targetUser === null) {
            console.log(`Target user ${targetUser.slackId} doesn't exit.`);
            var targetUserNewSeed = await iotaManager.generateNewSeed();

            newTargetUser = {
                slackId: targetSlackId,
                seed: targetUserNewSeed
            };

            targetUser = await userRepository.createUser(db, newTargetUser);

            console.log(`Target user ${targetUser.slackId} create`);
        }

        // generate new address for target user

        // check if user has enough fund

        // create new iota transaction
    }

    res.status(200);
})

