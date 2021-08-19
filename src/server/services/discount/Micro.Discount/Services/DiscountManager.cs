using Dapper;
using Micro.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Micro.Discount.Services
{
    public class DiscountManager : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            int status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Error("Discount not found.", 404);
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            IEnumerable<Models.Discount> discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
            return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>(
                "select * from discount where userid=@UserId and code=@Code", new { UserId = userId, Code = code });

            Models.Discount hasDiscount = discounts.FirstOrDefault();

            if (hasDiscount == null)
                return Response<Models.Discount>.Error("Discount not found", 404);

            return Response<Models.Discount>.Success(hasDiscount, 200);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>(
                "select * from discount where id=@Id", new { Id = id })).SingleOrDefault();

            if (discount == null)
                return Response<Models.Discount>.Error("Discount not found.", 404);

            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Models.Discount model)
        {
            var saveStatus = await _dbConnection.ExecuteAsync(
                "INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)", model );

            if (saveStatus > 0)
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Error("an error occurred while adding", 500);
        }

        public async Task<Response<NoContent>> Update(Models.Discount model)
        {
            var status = await _dbConnection.ExecuteAsync(
                "update discount set userid=@UserId, code=@Code, rate=@Rate where id=@Id",
                new
                {
                    Id = model.Id,
                    UserId = model.UserId,
                    Code = model.Code,
                    Rate = model.Rate
                });

            if (status > 0)
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Error("Discount not found", 404);
        }
    }
}
