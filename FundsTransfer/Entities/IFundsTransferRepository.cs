using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundsTransfer.Entities
{
    public interface IFundsTransferRepository
    {
        Task<bool> ValidateTransactionByRef(TransLog transLog);
        Task<FundsTransferResponse> ExecuteTransaction(FundsTransferRequest request, string sproc);
        Task<bool> UpdateTransactionResponse(FundsTransferResponse resp);
        Task<bool> IsOwnAccount(FundsTransferRequest request);
    }
}
