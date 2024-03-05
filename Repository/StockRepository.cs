using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using urban_trader_be.Data;
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
        public Task<List<Stock>> GetAllAsync()
        {
            return _context.Stock.ToListAsync();
        }
    }
}