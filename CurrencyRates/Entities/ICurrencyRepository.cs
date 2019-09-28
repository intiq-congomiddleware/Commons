using Commons.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRates.Entities
{
    public interface ICurrencyRepository
    {
        Task<CurrencyResponse> GetRates();
        string EncData(string value);
    }
}
