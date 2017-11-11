const mongodb = require('mongodb')
var Promise = require('promise');

var upsertUser = async function(db, user) {
    var userIdDb = await getUser(db, user.SlackId);

    if(userIdDb === null) {
        userIdDb = await createUser(db, user);
        console.log(userIdDb);
    }

    return userIdDb;
};

var createUser = async function (db, user) {  
    var response = await db.collection('users').insert(user);

    return response.ops[0];
};

var getUser = async function (db, slackId) {
    var response = await db.collection('users').findOne(
        { slackId: slackId });

    return response;
};

module.exports = {
    createUser: createUser,
    getUser: getUser,
    upsertUser: upsertUser
  }