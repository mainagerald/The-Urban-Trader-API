using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urban_trader_be.DTO.Comment;

namespace urban_trader_be.DTO.Stock
{
    public class StockDto
    {
        public int Id{get; set;}
        public string Symbol{get; set;} = string.Empty;
        public string CompanyName{get; set;} = string.Empty;
        public decimal Purchase {get; set;}
        public decimal LastDividend {get; set;}
        public string Industry{get; set;}= string.Empty;
        public long MarketCap{get; set;}
        public List<CommentDto> Comments{get; set;}
    }
}