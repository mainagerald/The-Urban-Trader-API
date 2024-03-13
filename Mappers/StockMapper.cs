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
                Purchase = stockModel.Purchase,
                LastDividend=stockModel.LastDividend,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments=stockModel.Comments.Select(c=>c.ToCommentDto()).ToList()
            };
        }
    
        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockDto)
        {
            return new Stock{
                Symbol=stockDto.Symbol,
                CompanyName=stockDto.CompanyName,
                Purchase=stockDto.Purchase,
                LastDividend=stockDto.LastDividend,
                Industry=stockDto.Industry,
                MarketCap=stockDto.MarketCap
            };
        }

        public static Stock ToStockFromExternalAPI(this ExternalAPIStock externalAPIStock)
        {
            return new Stock{
                Symbol = externalAPIStock.symbol,
                CompanyName = externalAPIStock.companyName,
                Purchase = (decimal)externalAPIStock.price,
                LastDividend=(decimal)externalAPIStock.lastDiv,
                Industry = externalAPIStock.industry,
                MarketCap = externalAPIStock.mktCap,
            };
        }

    }
}