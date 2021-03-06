﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundsTransfer.Entities
{
    public interface IFundsTransferRepository
    {
        Task<bool> ValidateTransactionByRef(TransLog transLog);
        Task<FundsTransferResponse> ExecuteTransaction(FundsTransferRequest request);
        Task<bool> UpdateTransactionResponse(FundsTransferResponse resp);
        Task<bool> IsOwnAccount(FundsTransferRequest request);
        Task<AccountEnquiryResponse> GetAccountEnquiryByAccountNumber(AccountEnquiryRequest request);
        string EncData(string value);
    }
}
