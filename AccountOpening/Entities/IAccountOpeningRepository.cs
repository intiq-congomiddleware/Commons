using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOpening.Entities
{
    public interface IAccountOpeningRepository
    {
        Task<bool> AddCustomer(Personal p, Customer c);
        Task<bool> ExecuteNewCustomer(ExecuteCustomer executeCustomer);
        Task<bool> CustomerExist(string customerNo);
        Task<bool> AddAccount(Account a);
        Task<bool> ExecuteNewAccount(ExecuteCustomer executeCustomer);
        Task<AccountOpeningRequest> GetCustomer(string seq_num, string acct_class);
    }
}
