using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using urban_trader_be.Data;
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
    }
}