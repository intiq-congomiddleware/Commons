using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOpening.Entities
{
    public interface IAccountOpeningRepository
    {
        bool AddCustomer(UploadPersonal up, UploadCustomer uc);
        bool ExecuteNewCustomer(ExecuteCustomer executeCustomer);
        bool CustomerExist(string customerNo);
        bool AddAccount(UploadAccount ua);
        bool ExecuteNewAccount(ExecuteCustomer executeCustomer);
    }
}
