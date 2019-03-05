using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountEnquiry.Entities
{
    public interface IAccountEnquiryRepository
    {
        Task<AccountEnquiryResponse> GetAccountEnquiryByAccountNumber(AccountEnquiryRequest request);
        Task<AccountEnquiryResponse> GetAccountEnquiryByCustomerNumber(CustomerEnquiryRequest request);
        Task<AccountEnquiryResponse> GetAccountEnquiryByPhoneNumber(PhoneEnquiryRequest request);
    }
}
