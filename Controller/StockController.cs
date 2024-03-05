using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using urban_trader_be.Data;
using urban_trader_be.DTO.Stock;
using urban_trader_be.Mappers;

namespace urban_trader_be.Controller
{
    [Route("urban_trader_be/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDatabaseContext _context;
        public StockController(AppDatabaseContext context)
        {
            _context=context;
        }

        [HttpGet]
        public IActionResult GetAll(){
            var stocks = _context.Stock.ToList().Select(s=>s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id){
            var stock = _context.Stock.Find(id);

            if (stock==null){
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]

        public IActionResult Create([FromBody] CreateStockRequestDto stockDto){
            var stockModel=stockDto.ToStockFromCreateDTO();
            _context.Stock.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new{id=stockModel.Id}, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockRequestDto){
            var stockModel=_context.Stock.FirstOrDefault(x=>x.Id==id);
            if(stockModel==null){
                return NotFound();
            }
            stockModel.Symbol=updateStockRequestDto.Symbol;
            stockModel.CompanyName=updateStockRequestDto.CompanyName;
            stockModel.Purchase=updateStockRequestDto.Purchase;
            stockModel.LastDividend=updateStockRequestDto.LastDividend;
            stockModel.Industry=updateStockRequestDto.Industry;
            stockModel.MarketCap=updateStockRequestDto.MarketCap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDto());
        }
    }
}