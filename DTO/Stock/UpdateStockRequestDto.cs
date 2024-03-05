using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace urban_trader_be.DTO.Stock
{
    public class UpdateStockRequestDto
    {
        public string Symbol{get; set;} = string.Empty;
        public string CompanyName{get; set;} = string.Empty;
        public decimal Purchase {get; set;}=0;
        public decimal LastDividend {get; set;}=0;
        public string Industry{get; set;}= string.Empty;
        public long MarketCap{get; set;}=0;
 
    }
}