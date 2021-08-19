using Micro.Basket.DTOs;
using Micro.Shared.DTOs;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Micro.Basket.Services
{
    public class BasketManager : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketManager(RedisService redisService) => _redisService = redisService;

        public async Task<Response<bool>> Delete(string userId)
        {
            bool status = await _redisService.GetDb().KeyDeleteAsync(userId);
            return status ? Response<bool>.Success(204) : Response<bool>.Error("Basket not found.", 404);
        }

        public async Task<Response<BasketDTO>> Get(string userId)
        {
            RedisValue isExist = await _redisService.GetDb().StringGetAsync(userId);

            if (string.IsNullOrEmpty(isExist))
                return Response<BasketDTO>.Error("Basket not found.", 404);

            return Response<BasketDTO>.Success(JsonSerializer.Deserialize<BasketDTO>(isExist), 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDTO BasketDTO)
        {
            var status = await _redisService.GetDb().StringSetAsync(BasketDTO.UserId, JsonSerializer.Serialize(BasketDTO));
            return status ? Response<bool>.Success(204) : Response<bool>.Error("Basket could not update or save.", 500);
        }
    }
}
