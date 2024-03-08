using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace urban_trader_be.DTO.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage ="Symbol cannot be over 10 characters")]
        public string Symbol{get; set;} = string.Empty;
        [Required]
        [MaxLength(10, ErrorMessage ="Company name cannot be over 10 characters")]
        public string CompanyName{get; set;} = string.Empty;
        [Required]
        [Range(1,1000000000)]
        public decimal Purchase {get; set;}=0;
        [Required]
        [Range(0.001, 100)]
        public decimal LastDividend {get; set;}=0;
        [Required]
        [MaxLength(20, ErrorMessage ="Industry cannot be over 20 characters")]
        public string Industry{get; set;}= string.Empty;
        [Required]
        [Range(1, 5000000000000)]
        public long MarketCap{get; set;}=0;
 
    }
}