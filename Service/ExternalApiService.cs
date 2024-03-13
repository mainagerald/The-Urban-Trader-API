using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using urban_trader_be.DTO.Stock;
using urban_trader_be.Interface;
using urban_trader_be.Mappers;
using urban_trader_be.Model;

namespace urban_trader_be.Service
{
    public class ExternalApiService : iExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private ExternalApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient=httpClient;
            _configuration=configuration;
        }
        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var result=await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/${symbol}?apikey={_configuration["APIKey"]}");

                if(result.IsSuccessStatusCode)
                {
                    var content=await result.Content.ReadAsStringAsync();
                    var tasks=JsonConvert.DeserializeObject<ExternalAPIStock[]>(content);
                    var stock=tasks[0];

                    if(stock!=null)
                    {
                        return stock.ToStockFromExternalAPI();
                    }
                    return null;
                }
                return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}