var getNodeInfo = function (req, res) {
  
  var iotaConnect = new iota({
    'host': 'http://node.iotawallet.info',
    'port': 14265,
    'sandbox': 'true'
  });

  iotaConnect.api.getNodeInfo(function(error, success) {
    if (error) {
        console.error(error);
    } else {
        console.log(success);
    }
  })

  // iota.api.getNewAddress(seed [, options], callback)
  res.send(iotaConnect.version);
}

exports.getNodeInfo = getNodeInfo;