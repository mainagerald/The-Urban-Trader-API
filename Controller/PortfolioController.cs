using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using urban_trader_be.Extensions;
using urban_trader_be.Interface;
using urban_trader_be.Model;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace urban_trader_be.Controller
{
    [Route("urban_trader_be/portfolio")]
    [ApiController]
    public class PortfolioController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly iStockRepository _istockRepo;
        private readonly iPortfolioRespository _iportfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, iStockRepository istockRepo, iPortfolioRespository iportfolioRepo)
        {
            
            _userManager=userManager;
            _istockRepo=istockRepo;
            _iportfolioRepo=iportfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            var userPortfolio= await _iportfolioRepo.GetUserPortfolio(appUser);

            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            var stock=await _istockRepo.GetBySymbolAsync(symbol);

            if(stock==null)
            {
                return BadRequest("Stock Not Found");
            }

            var userPortfolio= await _iportfolioRepo.GetUserPortfolio(appUser);

            if(userPortfolio.Any(e=>e.Symbol.ToLower()==symbol.ToLower())) 
                return BadRequest("Cannot add the same stock to portfolio");

            var portfolioModel = new Portfolio
            {
                StockId=stock.Id,
                AppUserId=appUser.Id
            };

            await _iportfolioRepo.CreateAsync(portfolioModel);

            if(portfolioModel==null)
            {
                return StatusCode(500, "Could not create portfolio");
            }
            else
            {
                return Created(); 
            }
        }
        
    }
}