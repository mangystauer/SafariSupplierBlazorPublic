using DataAccessLibrary.Models.Shatem.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.Shatem.DataAccess
{
    public interface IShatemAccess
    {
        Task<ShatemAccessModel> GetAccessTokenAsync();

        Task<List<ShatemAgreement>> GetAgreementsAsync(string accessToken);

        Task<ShatemLocationList> GetLocationListAsync(string accessToken);
    }
}
