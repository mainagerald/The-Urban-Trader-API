using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using urban_trader_be.Data;
using urban_trader_be.DTO.Comment;
using urban_trader_be.Interface;
using urban_trader_be.Model;

namespace urban_trader_be.Repository
{
    public class CommentRepository : iCommentRepository
    {
        private readonly AppDatabaseContext _context;
        public CommentRepository(AppDatabaseContext context){
            _context=context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel= await _context.Comments.FirstOrDefaultAsync(x=>x.Id==id);
            if(commentModel==null){
                return null;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(a=>a.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a=>a.AppUser).FirstOrDefaultAsync(c=>c.Id==id);
// findasync does not have include so we switch to firstordefault
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentDto updateCommentDto)
        {
            var existingComment= await _context.Comments.FirstOrDefaultAsync(x=>x.Id==id);
            if(existingComment==null){
                return null;
            }
            existingComment.Title=updateCommentDto.Title;
            existingComment.Content=updateCommentDto.Content;

            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}