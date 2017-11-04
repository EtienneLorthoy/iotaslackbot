# iotaslackbot
Tips Slack bot for IOTA


Slack Commands :
- /info
- /sendtip
- /deposite
- /withdraw


V 0.5 :

When an user uses the command /info or /sendtip :
 - if there is no seed associated with the slack userId :
    => we create a new SEED, and we send that seed to the user via the slack command response.

When an user uses the command /info :
- if there is a seed associated with the slack userId :
    => we return the total balance of the seed.

When an user uses the command /sendtip @username :
- if there is a seed associated the slack userId :
    - if there is no seed associated with the recipient 
        => we create a new SEED for this user.
    - if there is enough balance in the seed
        => we create an iota transaction from the sender to the recipient


