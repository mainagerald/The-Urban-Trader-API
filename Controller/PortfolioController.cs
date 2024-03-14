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
    [Route("api/v3/portfolio")]
    [ApiController]
    public class PortfolioController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly iStockRepository _istockRepo;
        private readonly iPortfolioRespository _iportfolioRepo;
        private readonly iExternalApiService _iexternalApiService;
        public PortfolioController(UserManager<AppUser> userManager, iStockRepository istockRepo, iPortfolioRespository iportfolioRepo, iExternalApiService iexternalApiService)
        {
            
            _userManager=userManager;
            _istockRepo=istockRepo;
            _iportfolioRepo=iportfolioRepo;
            _iexternalApiService=iexternalApiService;
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
                stock=await _iexternalApiService.FindStockBySymbolAsync(symbol);
                if(stock==null)
                {
                    return BadRequest("Stock does not exist!");
                }
                else
                {
                    await _istockRepo.CreateAsync(stock);
                }
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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            var userPortfolio=await _iportfolioRepo.GetUserPortfolio(appUser);

            var filterStock=userPortfolio.Where(s=>s.Symbol.ToLower()==symbol.ToLower()).ToList();

            if(filterStock.Count()==1)
            {
                await _iportfolioRepo.DeletePortfolio(appUser, symbol);
            }
            else
            {
                BadRequest("Stock not in your portfolio");
            }

            return Ok();
        }
        
    }
}