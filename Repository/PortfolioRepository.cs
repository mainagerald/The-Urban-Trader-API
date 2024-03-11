using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using urban_trader_be.Data;
using urban_trader_be.Interface;
using urban_trader_be.Model;

namespace urban_trader_be.Repository
{
    public class PortfolioRepository : iPortfolioRespository
    {
        private readonly AppDatabaseContext _context;
        public PortfolioRepository(AppDatabaseContext context)
        {
            _context=context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel=await _context.Portfolios.FirstOrDefaultAsync(x=>x.AppUserId==appUser.Id&&x.Stock.Symbol.ToLower()==symbol.ToLower());

            if(portfolioModel==null) return null;

            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios
            .Where(u=>u.AppUserId==user.Id)
            .Select(stock=>new Stock
            {
                Id=stock.StockId,
                Symbol=stock.Stock.Symbol,
                CompanyName=stock.Stock.CompanyName,
                Purchase=stock.Stock.Purchase,
                LastDividend=stock.Stock.LastDividend,
                Industry=stock.Stock.Industry,
                MarketCap=stock.Stock.MarketCap
            }).ToListAsync();
        }
    }
}