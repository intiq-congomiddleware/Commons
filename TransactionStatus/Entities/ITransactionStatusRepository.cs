using Commons.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionStatus.Entities
{
    public interface ITransactionStatusRepository
    {
        Task<FundsTransferResponse> ValidateTransactionByRef(string transactionRef);
        string EncData(string value);
    }
}
