using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urban_trader_be.DTO.Stock;
using urban_trader_be.Model;

namespace urban_trader_be.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto{
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.LastDividend,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }
    }
}