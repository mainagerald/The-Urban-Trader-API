using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using urban_trader_be.DTO.Comment;
using urban_trader_be.Extensions;
using urban_trader_be.Interface;
using urban_trader_be.Mappers;
using urban_trader_be.Model;
using urban_trader_be.Repository;
using urban_trader_be.Service;

namespace urban_trader_be.Controller
{
    [Route("urban_trader_be/comment")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly iCommentRepository _icommentRepository;
        private readonly iStockRepository _istockRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly iExternalApiService _iexternalApiService;

        public CommentController(iCommentRepository iCommentRepo, iStockRepository iStockRepo, UserManager<AppUser> userManager, iExternalApiService iexternalApiService)
        {
            _icommentRepository=iCommentRepo;
            _istockRepository=iStockRepo;
            _userManager=userManager;
            _iexternalApiService=iexternalApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var comments = await _icommentRepository.GetAllAsync();
            var CommentDto=comments.Select(s=>s.ToCommentDto());
            return Ok(CommentDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var comment=await _icommentRepository.GetByIdAsync(id);
            if(comment==null){
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{symbol:alpha}")]
        public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentDto createCommentDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            
            var stock=await _istockRepository.GetBySymbolAsync(symbol);
            if(stock==null)
            {
                stock=await _iexternalApiService.FindStockBySymbolAsync(symbol);
                if(stock==null)
                {
                    return BadRequest("Stock does not exist!");
                }
                else
                {
                    await _istockRepository.CreateAsync(stock);
                }
            }

            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);

            var commentModel=createCommentDto.ToCommentFromCreate(stock.Id);
            commentModel.AppUserId=appUser.Id;

            await _icommentRepository.CreateAsync(commentModel);
            
            return CreatedAtAction(nameof(GetById), new {id=commentModel.Id}, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateCommentDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var commentModel=await _icommentRepository.UpdateAsync(id, updateCommentDto);
            if(commentModel==null){
                return NotFound("Comment does nt exist");
            }
            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var commentModel=await _icommentRepository.DeleteAsync(id);
            if(commentModel==null){
                return NotFound("Comment does not exist");
            }
            return Ok(commentModel);
        }
    }
}