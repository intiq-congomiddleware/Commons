using Commons.Entities;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

namespace CurrencyRates.Entities
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly AppSettings _appSettings;
        private readonly Settings _settings;
        private IDataProtector _protector;

        public CurrencyRepository(IOptions<AppSettings> appSettings, IDataProtectionProvider provider
                                   , IOptions<Settings> settings)
        {
            _appSettings = appSettings.Value;
            _settings = settings.Value;
            _protector = provider.CreateProtector("treyryug");
        }
        public async Task<CurrencyResponse> GetRates()
        {
            CurrencyResponse response = new CurrencyResponse()
            {
                status = "FAILED"
            };

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));

            using (oralConnect)
            {
                string query = $@"SELECT DISTINCT BRANCH_CODE, CCY1, CCY2, RATE_TYPE, MID_RATE AS RATE, RATE_DATE, 
                                  INT_AUTH_STAT FROM CYTM_RATES WHERE CCY1 IN ({getCcys(_settings.CCY1)}) 
                                  AND RATE_TYPE = '{_settings.RateType}' AND CCY2 = '{_settings.CCY2}' 
                                  AND INT_AUTH_STAT = '{_settings.IntAuthStat}' AND BRANCH_CODE = '{_settings.BranchCode}'";

                var r = await oralConnect.QueryAsync<Rates>(query);

                response = GetCurrencyResponse(r.ToList());
            }

            return response;
        }

        public string getCcys(string[] ccy1)
        {
            string ccys = string.Empty;

            foreach (var c in ccy1)
            {
                ccys += $"'{c}',";
            }

            return ccys.TrimEnd(',');
        }

        public CurrencyResponse GetCurrencyResponse(List<Rates> rates)
        {
            if (rates == null)
                return null;

            CurrencyResponse resp = new CurrencyResponse()
            {
                 status = "SUCCESSFUL"
            };

            PropertyInfo[] properties = typeof(CurrencyResponse).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                foreach (var r in rates)
                {
                    if (r.ccy1 == property.Name)
                    {
                        property.SetValue(resp, r.rate);
                        break;
                    }
                }               
            }

            return resp;
        }

        public string EncData(string value)
        {
            string output = string.Empty;
            try
            {
                output = _protector.Protect(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return output;
        }
    }
}
