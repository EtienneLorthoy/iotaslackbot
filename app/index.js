const express = require('express')
const app = express()

app.get('/', function (req, res) {
  res.send('Hello tipwallet!')
})

app.post('/api/tipwallet/info', function (req, res) {
    res.send('tipwallet info!')
})

app.post('/api/tipwallet/deposite', function (req, res) {
    res.send('tipwallet deposite!')
})

app.post('/api/tipwallet/withdraw', function (req, res) {
    res.send('tipwallet withdraw!')
})

app.post('/api/tipwallet/sendtip', function (req, res) {
    res.send('tipwallet sendtip!')
})


app.listen(3000, function () {
  console.log('Example app listening on port 3000!')
})