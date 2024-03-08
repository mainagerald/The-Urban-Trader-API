using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using urban_trader_be.Data;
using urban_trader_be.DTO.Stock;
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
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel=await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);
            if(stockModel==null){
                return null;
            }

            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stock.Include(c=>c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.Include(c=>c.Comments).FirstOrDefaultAsync(i=>i.Id==id);
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stock.AnyAsync(s=>s.Id==id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequestDto)
        {
            var existingStock=await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);

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