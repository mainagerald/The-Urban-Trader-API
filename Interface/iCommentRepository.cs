using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urban_trader_be.DTO.Comment;
using urban_trader_be.Model;

namespace urban_trader_be.Interface
{
    public interface iCommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment commentModel);
        Task<Comment?> UpdateAsync(int id, UpdateCommentDto updateCommentDto );
        Task<Comment?>DeleteAsync(int id);
    }
}
