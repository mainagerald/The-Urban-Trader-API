using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urban_trader_be.Model;

namespace urban_trader_be.Interface
{
    public interface iPortfolioRespository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);
    }
}