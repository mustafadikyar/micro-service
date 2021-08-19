using Micro.Basket.DTOs;
using Micro.Shared.DTOs;
using System.Threading.Tasks;

namespace Micro.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDTO>> Get(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketDTO basketDto);
        Task<Response<bool>> Delete(string userId);
    }
}
