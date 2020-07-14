using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBAProcessor
{
    public class MessageUtility
    {
    }
    public class ResponseCode
    {
        public const string ERROR = "06";
        public const string SUCCESS = "00";
        public const string NO_CREDIT_ACCOUNT = "39";
        public const string NO_CHECK_ACCOUNT = "52";
        public const string NO_SAVINGS_ACCOUNT = "53";
        public const string INVALID_TRANSACTION = "12";
        public const int INVALID_AMOUNT = 13;
        public const int INVALID_RESPONSE = 20;
        public const int UNABLE_TO_LOCATE_RECORD = 25;
        public const string ISSUER_OR_SWITCH_INOPERATIVE = "91";
        public const int ROUTING_ERROR = 92;
        public const int DUPLICATE_TRANSACTION = 94;
        public const int EXPIRED_CARD = 54;
        public const int TRANSACTION_NOT_PERMITTED_ON_TERMINAL = 58;
        public const string RESPONSE_RECEIVED_TOO_LATE = "68";
    }
    public static class MessageField
    {
        public const int RESPONSE_FIELD = 39;
        public const int ORIGINAL_DATA_ELEMENT_FIELD = 90;
        public const int TRANSACTION_TYPE_FIELD = 3;
        public const int CHANNEL_ID_FIELD = 41;
        public const int CARD_PAN_FIELD = 2;        //Primary Account Number
        public const int STAN_FIELD = 11;
        public const int AMOUNT_FIELD = 4;
        public const int EXPIRY_DATE_FIELD = 14;
        public const int TRANSACTION_FEE_FIELD = 28;
        public const int CURRENCY_CODE =49;
        public const int FROM_ACCOUNT_ID_FIELD = 102;
        public const int TO_ACCOUNT_ID_FIELD = 103;
    }


    public static class TransactionTypeCode
    {
        public const string CASH_WITHDRAWAL = "01";
        public const string PAYMENT_FROM_ACCOUNT = "50";
        public const string PAYMENT_BY_DEPOSIT = "51";
        public const string INTRA_BANK_TRANSFER = "24";
        public const string BALANCE_ENQUIRY = "31";
    }
}
