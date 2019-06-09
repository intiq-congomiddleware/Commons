using Commons.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebitFreeze.Entities
{
    public interface IDebitFreezeRepository
    {
        Task<Response> FreezeAccount(string accountNumber);
        string EncData(string value);
    }
}
