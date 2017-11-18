const promise = require("bluebird");
const iota = require('iota.lib.js');
const generatePassword = require('password-generator');

var iotaConnect;
var api;

exports.init = function (){
  iotaConnect = new iota({
    'host': process.env.IOTA_NODE,
    'port': 14265,
    'sandbox': 'true'
  });
  api = promise.promisifyAll(iotaConnect.api, {suffix: "Async"});
}

exports.getNodeInfo = async function () {
  var t = await api.getNodeInfoAsync();
  return t;
}

exports.generateNewSeed = function () {
  // Change it with an impredictable parameters from user.
  return generatePassword(81, false, /[A-Z9]/);
}

exports.createAddress = async function (seed) {
  return await iotaConnect.getNewAddress();
}

exports.getAllTransfers = function () {

}

exports.findTransactions = function () {
  
}

exports.sendIotas = function (sourceSeed, targetSeed, amount) {
  var sourceAddress = api.getNewAddress(sourceSeed);
  var targetAddress = api.getNewAddress(targetSeed);

  var transfertBundle = api.prepareTransfers(sourceSeed,
    [{
        'address': targetAddress,
        'value': amount
    }], {
    'inputs': [
        {
            address: sourceAddress,
            balance: 0,
            keyIndex: 0,
            security: 3
        }
    ]});

    console.log(`New bundle ready ${transfertBundle}`);
}