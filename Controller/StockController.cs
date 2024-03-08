using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using urban_trader_be.Data;
using urban_trader_be.DTO.Stock;
using urban_trader_be.Helpers;
using urban_trader_be.Interface;
using urban_trader_be.Mappers;

namespace urban_trader_be.Controller
{
    [Route("urban_trader_be/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDatabaseContext _context;
        private readonly iStockRepository _stockRepo;
        public StockController(AppDatabaseContext context, iStockRepository stockRepo)
        {
            _stockRepo=stockRepo;
            _context=context; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var stocks = await _stockRepo.GetAllAsync(queryObject);
            var stockDto = stocks.Select(s=>s.ToStockDto());

            return Ok(stocks);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock==null){
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var stockModel= stockDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new{id=stockModel.Id}, stockModel.ToStockDto());
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockRequestDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var stockModel= await _stockRepo.UpdateAsync(id, updateStockRequestDto);

            if(stockModel==null){
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }
        

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var stockModel = await _stockRepo.DeleteAsync(id);

          if(stockModel==null){
                return NotFound();
            }

            return NoContent();  
        }
    }
}