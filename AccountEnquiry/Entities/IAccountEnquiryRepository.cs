using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountEnquiry.Entities
{
    public interface IAccountEnquiryRepository
    {
        Task<AccountEnquiryResponse> GetAccountEnquiryByAccountNumber(AccountEnquiryRequest request);
        Task<List<AccountEnquiryResponse>> GetAccountEnquiryByCustomerNumber(CustomerEnquiryRequest request);
        Task<List<AccountEnquiryResponse>> GetAccountEnquiryByPhoneNumber(PhoneEnquiryRequest request);
        string EncData(string value);
    }
}
