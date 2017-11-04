const mongodb = require('mongodb')

var createUser = function (db, user, callback) {
    var collection = db.collection('users');

    collection.insert(
        user
        , function (err, result) {
            //   assert.equal(err, null);
            //   assert.equal(3, result.result.n);
            //   assert.equal(3, result.ops.length);
            console.log("Inserted 1 user");
            callback(result);
        });
}

var getUser = function (db, slackId, callback) {
    var collection = db.collection('users');

    collection.findOne(
        { slackId: slackId }
        , function (err, result) {
            console.log("Inserted 1 user");
            callback(result);
        });
}