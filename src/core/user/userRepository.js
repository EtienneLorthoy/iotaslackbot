const mongodb = require('mongodb')

exports.createUser = function (db, user) {
    var collection = db.collection('users');
    var createdUser;
    var dd = await collection.insert(
        user
        , function (err, result) {
            console.log("Inserted 1 user");
            console.log(result.ops[0]);
            createdUser = result.ops[0];
        });

    console.log(createdUser);
    return createdUser;
}

exports.getUser = function (db, slackId) {
    var collection = db.collection('users');

    collection.findOne(
        { slackId: slackId }
        , function (err, result) {
            console.log("Inserted 1 user");
            return result;
        });
}