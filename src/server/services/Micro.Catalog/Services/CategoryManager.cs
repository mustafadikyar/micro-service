using AutoMapper;
using Micro.Catalog.DTOs;
using Micro.Catalog.Models;
using Micro.Catalog.Settings;
using Micro.Shared.DTOs;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Micro.Catalog.Services
{
    public class CategoryManager 
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryManager(IMapper mapper, IDatabaseSetting setting)
        {
            _mapper = mapper;

            MongoClient client = new(setting.ConnectionString);
            IMongoDatabase database = client.GetDatabase(setting.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(setting.CategoryCollectionName);
        }

        public async Task<Response<List<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();
            List<CategoryDTO> result = _mapper.Map<List<CategoryDTO>>(categories);
            return Response<List<CategoryDTO>>.Success(result, 200);
        }

        public async Task<Response<CategoryDTO>> CreateAsync(CategoryCreateDTO model)
        {
            Category created = _mapper.Map<Category>(model);
            await _categoryCollection.InsertOneAsync(created);
            return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(created), 200);
        }

        public async Task<Response<CategoryDTO>> GetByIdAsync(string id)
        {
            Category category = await _categoryCollection.Find(category => category.Id.Equals(id)).FirstOrDefaultAsync();

            if (category == null)
                return Response<CategoryDTO>.Error("Category not found", 404);

            return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
        }
    }
}
