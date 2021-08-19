using Micro.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Micro.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<Models.Discount>>> GetAll();
        Task<Response<Models.Discount>> GetById(int id);
        Task<Response<NoContent>> Save(Models.Discount model);
        Task<Response<NoContent>> Update(Models.Discount model);
        Task<Response<NoContent>> Delete(int id);
        Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId);
    }
}
