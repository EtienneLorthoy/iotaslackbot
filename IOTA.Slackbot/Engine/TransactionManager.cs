using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOTA.Slackbot.Engine
{
    public interface ITransactionManager
    {
        ResponseMessage SendTip(string fromUserId, string fromUserName, string toUserId, string toUserName, decimal iotas);
    }

    public class TransactionManager : ITransactionManager
    {
        public ResponseMessage SendTip(string fromUserId, string fromUserName, string toUserId, string toUserName, decimal iotas)
        {


            return new ResponseMessage("You send 100 iotas to etienne", true);
        }
    }
}
