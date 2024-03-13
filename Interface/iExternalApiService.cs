using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urban_trader_be.Model;

namespace urban_trader_be.Interface
{
    public interface iExternalApiService
    {
        Task<Stock> FindStockBySymbolAsync(string symbol);
    }
}