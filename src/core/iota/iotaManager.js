const promise = require("bluebird");
const iota = require('iota.lib.js');
const generatePassword = require('password-generator');

var iotaConnect;
var api;
var tools;

exports.init = function (){
  iotaConnect = new iota({
    'host': process.env.IOTA_NODE,
    'port': 14265,
    'sandbox': 'true'
  });
  api = promise.promisifyAll(iotaConnect.api, {suffix: "Async"});
  utils = iotaConnect.utils;

  console.log("Iota manager ready");  
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

exports.sendIotas = async function (sourceSeed, targetSeed, amount) {
  // var sourceAddress = await api.getNewAddressAsync(sourceSeed);
  var targetAddress = await api.getNewAddressAsync(targetSeed);
  
  // var transfertBundle = await api.prepareTransfersAsync(sourceSeed,
  //   [{
  //       'address': targetAddress,
  //       'value': amount
  //   }], {
  //   'inputs': [
  //       {
  //           address: sourceAddress,
  //           balance: 0,
  //           keyIndex: 0,
  //           security: 3
  //       }
  //   ]});

  var messageTrytes = utils.toTrytes("IOTA Tip Bot");

  var transfer = [{
      'address': targetAddress,
      'value': amount,
      'message': messageTrytes
  }]

  api.sendTransfer(sourceSeed, 4, 18, transfer, function(error, success) {
    if (error) {
        console.error(error);
    } else {
        console.log(success);
    }
  });

  console.log(`New bundle ready ${resultTransfert}`);
}