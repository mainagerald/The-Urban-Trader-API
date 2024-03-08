using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urban_trader_be.DTO.Comment;
using urban_trader_be.Model;

namespace urban_trader_be.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModelMapper){
            return new CommentDto{
                Id=commentModelMapper.Id,
                Title=commentModelMapper.Title,
                Content=commentModelMapper.Content,
                CreatedOn=commentModelMapper.CreatedOn,
                StockId=commentModelMapper.StockId
            };
        }
        public static Comment ToCommentFromCreate(this CreateCommentDto createCommentDto, int stockId){
            return new Comment
            {
                Title=createCommentDto.Title,
                Content=createCommentDto.Content,
                StockId=stockId
            };
        }
    }
}