using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using urban_trader_be.DTO.Comment;
using urban_trader_be.Interface;
using urban_trader_be.Mappers;
using urban_trader_be.Repository;

namespace urban_trader_be.Controller
{
    [Route("urban_trader_be/comment")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly iCommentRepository _commentRepository;
        private readonly iStockRepository _stockRepository;

        public CommentController(iCommentRepository iCommentRepo, iStockRepository iStockRepo)
        {
            _commentRepository=iCommentRepo;
            _stockRepository=iStockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var comments = await _commentRepository.GetAllAsync();
            var CommentDto=comments.Select(s=>s.ToCommentDto());
            return Ok(CommentDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var comment=await _commentRepository.GetByIdAsync(id);
            if(comment==null){
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{StockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int StockId, CreateCommentDto createCommentDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            if(!await _stockRepository.StockExists(StockId)){
                return BadRequest("Stock does not exist!");
            }
            var commentModel=createCommentDto.ToCommentFromCreate(StockId);
            await _commentRepository.CreateAsync(commentModel);
            
            return CreatedAtAction(nameof(GetById), new {id=commentModel.Id}, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateCommentDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var commentModel=await _commentRepository.UpdateAsync(id, updateCommentDto);
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
            var commentModel=await _commentRepository.DeleteAsync(id);
            if(commentModel==null){
                return NotFound("Comment does not exist");
            }
            return Ok(commentModel);
        }
    }
}