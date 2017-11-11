const promise = require("bluebird");
const iota = require('iota.lib.js');
const generatePassword = require('password-generator');
var iotaConnect;

exports.init = function (){
  iotaConnect = new iota({
    'host': process.env.IOTA_NODE,
    'port': 14265,
    'sandbox': 'true'
  });
}

exports.getNodeInfo = async function () {
  var api = promise.promisifyAll(iotaConnect.api, {suffix: "Async"});
  var t = await api.getNodeInfoAsync();
  return t;
}

exports.generateNewSeed = function () {
  // Change it with an impredictable parameters from user.
  return generatePassword(81, false, /[A-Z9]/);
}

exports.createAddress = async function (seed) {
  // return await iotaConnect.getNewAddress();
}

exports.getAllTransfers = function () {

}

exports.findTransactions = function () {
  
}

exports.sendIotas = function (req, res) {

}