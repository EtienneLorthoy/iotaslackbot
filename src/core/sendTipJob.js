const request = require('request');
var Promise = require('promise');
const iotaManager = require("./iota/iotaManager.js");
const userRepository = require("./user/userRepository.js");
// const createTransactionJob = require("./iota/createTransactionJob.js");

var execute = async function (slackCommand, db) {
    console.log('test 3');
    var slackId = `${slackCommand.team_id}_${slackCommand.user_id}`;
    var user = await userRepository.getUser(db, slackId);

    if (user === null) {
        console.log(`Unknow slackid:${slackId} create new user.`);
        var newSeed = await iotaManager.generateNewSeed();

        newUser = {
            slackId: slackId,
            seed: newSeed
        };

        user = await userRepository.createUser(db, newUser);
        console.log(`User ${user.slackId} created.`);

        var slackResponse = await request.post(
            slackCommand.response_url,
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
        var match = myRegexp.exec(slackCommand.text);
        console.log(match[1]);

        var targetSlackId = `${slackCommand.team_id}_${match[1]}`;

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
        // run job
        // agenda.now('createtransaction', {
        //     sourceSeed: user.seed,
        //     targetSeed: targetUser.seed
        // });
    }


    return;
};

module.exports = {
    execute: execute
}