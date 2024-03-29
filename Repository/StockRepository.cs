using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using urban_trader_be.Data;
using urban_trader_be.DTO.Stock;
using urban_trader_be.Helpers;
using urban_trader_be.Interface;
using urban_trader_be.Model;

namespace urban_trader_be.Repository
{
    public class StockRepository : iStockRepository
    {
        private readonly AppDatabaseContext _context;
        public StockRepository(AppDatabaseContext context){
            _context=context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel=await _context.Stocks.FirstOrDefaultAsync(x=>x.Id==id);
            if(stockModel==null){
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
        {
            var stocks =  _context.Stocks.Include(c=>c.Comments).ThenInclude(a=>a.AppUser).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObject.CompanyName)){
                stocks=stocks.Where(s=>s.CompanyName.Contains(queryObject.CompanyName));
            }
            if(!string.IsNullOrWhiteSpace(queryObject.Symbol)){
                stocks=stocks.Where(s=>s.Symbol.Contains(queryObject.Symbol));
            }
            if(!string.IsNullOrWhiteSpace(queryObject.SortBy)){
                if(queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)){
                    stocks=queryObject.IsDescending ? stocks.OrderByDescending(s=>s.Symbol): stocks.OrderBy(s=>s.Symbol);
                }
            }

            var skipNumber = (queryObject.PageNumber-1)*queryObject.PageSize;

            return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
        }


        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c=>c.Comments).FirstOrDefaultAsync(i=>i.Id==id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s=>s.Symbol==symbol);
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(s=>s.Id==id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequestDto)
        {
            var existingStock=await _context.Stocks.FirstOrDefaultAsync(x=>x.Id==id);

            if(existingStock==null){
                return null;
            }

            existingStock.Symbol=updateStockRequestDto.Symbol;
            existingStock.CompanyName=updateStockRequestDto.CompanyName;
            existingStock.Purchase=updateStockRequestDto.Purchase;
            existingStock.LastDividend=updateStockRequestDto.LastDividend;
            existingStock.Industry=updateStockRequestDto.Industry;
            existingStock.MarketCap=updateStockRequestDto.MarketCap;

            await _context.SaveChangesAsync();
            return existingStock;
        }
    }
}