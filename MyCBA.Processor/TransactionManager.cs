using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trx.Messaging;
using Trx.Messaging.Iso8583;

namespace MyCBAProcessor
{
    public class TransactionManager
    {
        static CustomerAccountRepository actRepo = new CustomerAccountRepository();
        static GlAccountRepository glRepo = new GlAccountRepository();
        static BusinessLogic blogic = new BusinessLogic();
        static FinancialReportLogic frLogic = new FinancialReportLogic();
        public static Iso8583Message ProcessMessage(Iso8583Message msg, CbaListener.MessageSource msgSource)
        {           
            //check the transaction type
            string transactionTypeCode = msg.Fields[MessageField.TRANSACTION_TYPE_FIELD].ToString().Substring(0, 2);
            long accountNumber = Convert.ToInt64(msg.Fields[MessageField.FROM_ACCOUNT_ID_FIELD].ToString());   //the From account number is the holders's account number
            var customerAccount = actRepo.GetByAcctNum(accountNumber);

            decimal amount = Convert.ToDecimal(msg[4].Value) / 100;         //converts to Naira
            decimal charge = Convert.ToDecimal(msg[28].Value) / 100;        //converts to Naira
            decimal processingFee = msg.Fields.Contains(31) ? Convert.ToDecimal(msg[31].Value) /100 : 0;

            decimal totalCharges = charge + processingFee;
            decimal totalAmt = amount + totalCharges;

            if (customerAccount == null && transactionTypeCode != "51")     //only for payment by deposit (51) do we not need From acct
            {
                msg.Fields.Add(MessageField.RESPONSE_FIELD, ResponseCode.NO_SAVINGS_ACCOUNT);
                return msg;
            }

            #region REVERSAL
            if (msg.MessageTypeIdentifier == 420)       //Reversal Advice
            {
                msg.MessageTypeIdentifier = 430;    //Reversal Advice Response
                UtilityLogic.LogMessage("Processing a reversal message...");
                //check the transaction type
                if (transactionTypeCode.Equals(TransactionTypeCode.CASH_WITHDRAWAL))   //Reversing a withdrawal
                {
                    //We can only reverse a withdrawal, transfer or payment transaction for this scope

                    //if Withdrawal, debit the AtmGL and credit the customer account with the (amt+fee) in d msg
                    var atmGl = new GlAccountRepository().GetByName("ATMGL");
                    var withdrawalIncomeGl = glRepo.GetByName("WithdrawalIncomeGl");
                    if (atmGl == null || withdrawalIncomeGl == null)
                    {
                        msg.Fields.Add(39, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE);   //Issuer or switch inoperative. 
                        return msg;
                    }
                    CreditCustomer(customerAccount, totalAmt);
                    DebitGl(atmGl, amount);
                    DebitGl(withdrawalIncomeGl, totalCharges);

                    msg.Fields.Add(39, "00");       //successful transaction
                    UtilityLogic.LogMessage("Withdrawal Transaction reversed successfully");
                    return msg;
                }
                else if (transactionTypeCode.Equals(TransactionTypeCode.PAYMENT_FROM_ACCOUNT))   //Reversing a (Payment from account)
                {
                    var onUsPaymentGl = new GlAccountRepository().GetByName("OnUsPaymentGL");
                    var remoteOnUsPaymentGl = new GlAccountRepository().GetByName("RemoteOnUsPaymentGl");
                    var paymentIncomeGl = glRepo.GetByName("PaymentIncomeGl");
                    if (onUsPaymentGl == null || remoteOnUsPaymentGl == null || paymentIncomeGl == null)
                    {
                        msg.Fields.Add(39, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE);   //Issuer or switch inoperative. 
                        return msg;
                    }
                    
                    CreditCustomer(customerAccount, totalAmt);
                    if (msgSource == CbaListener.MessageSource.OnUs)
                        DebitGl(onUsPaymentGl, amount);
                    else
                        DebitGl(remoteOnUsPaymentGl, amount);
                    
                    DebitGl(paymentIncomeGl, totalCharges);

                    msg.Fields.Add(39, "00");       //successful transaction
                    UtilityLogic.LogMessage("Payment reversed successfully");
                    return msg;
                }
                else if (transactionTypeCode.Equals(TransactionTypeCode.INTRA_BANK_TRANSFER))   //Reversing a Transfer, 24 is assumed
                {
                    GlAccount intraBankTransferIncomeGl = glRepo.GetByName("IntraBankTransferIncomeGl");
                    if (intraBankTransferIncomeGl == null)
                    {
                        msg.Fields.Add(MessageField.RESPONSE_FIELD, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE);
                        return msg;
                    }
                    var toAccountNumber = Convert.ToInt64(msg.Fields[MessageField.TO_ACCOUNT_ID_FIELD].Value);
                    var toAct = new CustomerAccountRepository().GetByAcctNum(toAccountNumber);
                    if (toAct == null)
                    {
                        msg.Fields.Add(MessageField.RESPONSE_FIELD, ResponseCode.NO_SAVINGS_ACCOUNT);
                        return msg;
                    }
                    if (!(new CustomerAccountLogic().CustomerAccountHasSufficientBalance(toAct, totalAmt)))   //insufficient balance
                    {
                        msg.Fields.Add(39, "51");       //insufficient funds (in the ATM)                            
                        return msg;
                    }
                    
                    DebitCustomer(toAct, amount);
                    CreditCustomer(customerAccount, totalAmt);
                    DebitGl(intraBankTransferIncomeGl, totalCharges);

                    msg.Fields.Add(39, ResponseCode.SUCCESS);       //successful transaction
                    UtilityLogic.LogMessage("Transfer reversed successfully");
                    return msg;
                }
            }
            #endregion

            else if (msg.MessageTypeIdentifier == 200)   //Transaction Request
            {
                msg.MessageTypeIdentifier = 210;
                #region BALANCE ENQUIRY
                //if balance enquiry, get the account number and return the balance
                if (transactionTypeCode.Equals(TransactionTypeCode.BALANCE_ENQUIRY))       //BALANCE ENQUIRY
                {
                    var feeIncomeGl = glRepo.GetByName("GeneralRemoteTransactionIncomeGl");
                    if (feeIncomeGl == null)
                    {
                        msg.Fields.Add(MessageField.RESPONSE_FIELD, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE);
                        return msg;
                    }
                    ProcessBalanceEnquiry(msg, customerAccount, totalCharges, feeIncomeGl);
                    return msg;
                }
                #endregion

                #region CASH WITHDRAWAL
                else if (transactionTypeCode.Equals(TransactionTypeCode.CASH_WITHDRAWAL))      //Cash Withdrawal
                {
                    return ProcessCashWithrawal(msg, customerAccount, amount, charge, totalCharges, totalAmt);

                }
                #endregion Withdrawal

                #region PAYMENT FROM ACCOUNT
                else if (transactionTypeCode.Equals(TransactionTypeCode.PAYMENT_FROM_ACCOUNT))      //payment from account
                {
                    return ProcessPaymentFromAccount(msg, msgSource, customerAccount, amount, totalCharges, totalAmt);
                }
                #endregion

                #region PAYMENT BY DEPOSIT
                else if (transactionTypeCode.Equals(TransactionTypeCode.PAYMENT_BY_DEPOSIT))      //PAYMENT BY DEPOSIT
                {
                    return ProcessPaymentByDeposit(msg, msgSource, amount);
                }
                #endregion

                #region INTRA-BANK TRANSFER
                else if (transactionTypeCode.Equals(TransactionTypeCode.INTRA_BANK_TRANSFER))      //Intra-bank Transfer, 24 is just assumed
                {
                    return ProcessIntraBankTransfer(msg, customerAccount, amount, totalCharges, totalAmt);
                }
                #endregion Intra-Bank Transfer
            }//End else-if

            //if we got this far, the transaction is invalid
            msg.Fields.Add(39, ResponseCode.INVALID_TRANSACTION);       //INVALID TRANSACTION
            return msg;
        }

        private static Iso8583Message ProcessIntraBankTransfer(Iso8583Message msg, CustomerAccount customerAccount, decimal amount, decimal totalCharges, decimal totalAmt)
        {
            UtilityLogic.LogMessage("Processing Intra-bank transfer");
            GlAccount intraBankTransferIncomeGl = glRepo.GetByName("IntraBankTransferIncomeGl");
            if (intraBankTransferIncomeGl == null)
            {
                msg.Fields.Add(MessageField.RESPONSE_FIELD, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE);
                return msg;
            }
            var toAccountNumber = Convert.ToInt64(msg.Fields[MessageField.TO_ACCOUNT_ID_FIELD].Value);
            var toAct = new CustomerAccountRepository().GetByAcctNum(toAccountNumber);
            if (toAct == null)
            {
                msg.Fields.Add(MessageField.RESPONSE_FIELD, ResponseCode.NO_SAVINGS_ACCOUNT);
                return msg;
            }
            if (!(new CustomerAccountLogic().CustomerAccountHasSufficientBalance(customerAccount, totalAmt)))   //insufficient balance
            {
                msg.Fields.Add(39, "51");       //insufficient funds (in the ATM)                            
                return msg;
            }

            DebitCustomer(customerAccount, totalAmt);
            CreditCustomer(toAct, amount);
            CreditGl(intraBankTransferIncomeGl, totalCharges);

            msg.Fields.Add(39, "00");       //successful transaction
            UtilityLogic.LogMessage("Transfer executed successfully");
            return msg;
        }

        private static Iso8583Message ProcessPaymentByDeposit(Iso8583Message msg, CbaListener.MessageSource msgSource, decimal amount)
        {
            UtilityLogic.LogMessage("Processing payment by deposit");
            if (!(msgSource == CbaListener.MessageSource.OnUs))
            {
                msg.Fields.Add(MessageField.RESPONSE_FIELD, "31"); //bank not supported
                return msg;
            }
            //Credit custAct and debit AtmGl  
            var toAccountNumber = Convert.ToInt64(msg.Fields[MessageField.TO_ACCOUNT_ID_FIELD].Value);
            var toAct = new CustomerAccountRepository().GetByAcctNum(toAccountNumber);
            if (toAct == null)
            {
                msg.Fields.Add(MessageField.RESPONSE_FIELD, ResponseCode.NO_SAVINGS_ACCOUNT);
                return msg;
            }
            var atmGl = new GlAccountRepository().GetByName("ATMGL");
            if (atmGl == null)
            {
                msg.Fields.Add(39, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE);   //Issuer or switch inoperative. 
                return msg;
            }
            CreditCustomer(toAct, amount);
            DebitGl(atmGl, amount);

            msg.Fields.Add(39, "00");       //successful transaction
            UtilityLogic.LogMessage("Payment Transaction successful");
            return msg;
        }

        private static Iso8583Message ProcessPaymentFromAccount(Iso8583Message msg, CbaListener.MessageSource msgSource, CustomerAccount customerAccount, decimal amount, decimal totalCharges, decimal totalAmt)
        {
            UtilityLogic.LogMessage("Processing payment from account");
            //debit custAct and credit OnUsPaymentGL  (may ba a capital GL)
            if (!(new CustomerAccountLogic().CustomerAccountHasSufficientBalance(customerAccount, totalAmt)))   //insufficient balance
            {
                msg.Fields.Add(39, "51");       //insufficient funds (in the ATM)                            
                return msg;
            }

            var onUsPaymentGl = new GlAccountRepository().GetByName("OnUsPaymentGL");
            var remoteOnUsPaymentGl = new GlAccountRepository().GetByName("RemoteOnUsPaymentGl");
            var paymentIncomeGl = glRepo.GetByName("PaymentIncomeGl");
            if (onUsPaymentGl == null || remoteOnUsPaymentGl == null || paymentIncomeGl == null)
            {
                msg.Fields.Add(39, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE);   //Issuer or switch inoperative. 
                return msg;
            }

            DebitCustomer(customerAccount, totalAmt);
            if (msgSource == CbaListener.MessageSource.OnUs)
                CreditGl(onUsPaymentGl, amount);
            else
                CreditGl(remoteOnUsPaymentGl, amount);

            CreditGl(paymentIncomeGl, totalCharges);

            msg.Fields.Add(39, "00");       //successful transaction
            UtilityLogic.LogMessage("Payment Transaction successful");
            return msg;
        }

        private static Iso8583Message ProcessCashWithrawal(Iso8583Message msg, CustomerAccount customerAccount, decimal amount, decimal charge, decimal totalCharges, decimal totalAmt)
        {
            UtilityLogic.LogMessage("Processing cash withdrawal");
            //for withdrawal, get the fromAccount, the amount and charge. Check the available balance and ...                
            if (!(new CustomerAccountLogic().CustomerAccountHasSufficientBalance(customerAccount, totalAmt)))
            {
                msg.Fields.Add(39, "51");   //not sufficient funds (Insufficient balance)   in customer's account 
                return msg;
            }
            //perform withdrawal
            try
            {
                //check if a GL account is set for the atm
                var atmGl = new GlAccountRepository().GetByName("ATMGL");
                var withdrawalIncomeGl = glRepo.GetByName("WithdrawalIncomeGl");
                if (atmGl == null || withdrawalIncomeGl == null)
                {
                    msg.Fields.Add(39, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE);   //Issuer or switch inoperative. 
                    return msg;
                }
                if (atmGl.acountBalance < (amount + charge))
                {
                    msg.Fields.Add(39, "51");       //insufficient funds (in the ATM)                            
                    return msg;
                }

                //DEBIT AND CREDIT
                DebitCustomer(customerAccount, totalAmt);
                CreditGl(atmGl, amount);
                CreditGl(withdrawalIncomeGl, totalCharges);     //same gl for both onus and remote on-us                     

                msg.Fields.Add(39, "00");       //successful transaction
                UtilityLogic.LogMessage("Withdrawal Transaction successful");
                return msg;
            }
            catch (Exception ex)
            {
                UtilityLogic.LogError("Error:  " + ex.Message + "   Inner Exception:      " + ex.InnerException);
                msg.Fields.Add(39, "06");       //ERROR!
                return msg;
            }
        }

        private static void ProcessBalanceEnquiry(Iso8583Message msg, CustomerAccount customerAccount, decimal charges, GlAccount feeIncomeGl)
        {           
            UtilityLogic.LogMessage("Processing balance enquiry");
            decimal balance = customerAccount.acctbalance;
            //append the account balance
            //filed 54 is for additional ammount
            string actType = customerAccount.accType == "Savings" ? "00" : (customerAccount.accType == "Current" ? "20" : "00");  //00 is for a default (unspecified) account
            string amtType = "01";      // Ledger balance
            string currencyCode = msg.Fields[MessageField.CURRENCY_CODE].ToString();
            string amtSign = "C";       //for credit
            string amt = balance.ToString();

            string additionalField = actType + amtType + currencyCode + amtSign + amt;
            msg.Fields.Add(54, additionalField);
            msg.Fields.Add(39, "00");

            DebitCustomer(customerAccount, charges);
            CreditGl(feeIncomeGl, charges);
        }

        static void DebitCustomer(CustomerAccount act, decimal amt)
        {
            blogic.DebitCustomerAccount(act, amt);
            actRepo.Update(act);
            frLogic.CreateTransaction(act, amt, "Debit");    //records every Dr and Cr entries to ensure a balance FR
            UtilityLogic.LogMessage("Customer account: " + act.acctNumber + " debitted NGN" + amt);
        }

        static void CreditCustomer(CustomerAccount act, decimal amt)
        {
            blogic.CreditCustomerAccount(act, amt);
            actRepo.Update(act);
            frLogic.CreateTransaction(act, amt, "Credit");    //records every Dr and Cr entries to ensure a balance FR
            UtilityLogic.LogMessage("Customer account: " + act.acctNumber + " creditted NGN" + amt);
        }

        static void DebitGl(GlAccount gl, decimal amt)
        {
            blogic.DebitGl(gl, amt);    //credit the basic amt
            glRepo.Update(gl);
            frLogic.CreateTransaction(gl, amt, "Debit");
            UtilityLogic.LogMessage("Gl account: " + gl.Code + " debitted NGN" + amt);
        }
        static void CreditGl(GlAccount gl, decimal amt)
        {
            blogic.CreditGl(gl, amt);    //credit the basic amt
            glRepo.Update(gl);
            frLogic.CreateTransaction(gl, amt, "Credit");
            UtilityLogic.LogMessage("Gl account: " + gl.Code + " creditted NGN" + amt);
        }
    }
}
