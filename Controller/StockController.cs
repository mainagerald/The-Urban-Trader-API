using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using urban_trader_be.Data;

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
    }
}