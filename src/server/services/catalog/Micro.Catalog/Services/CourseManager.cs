using AutoMapper;
using Micro.Catalog.DTOs;
using Micro.Catalog.Models;
using Micro.Catalog.Settings;
using Micro.Shared.DTOs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Micro.Catalog.Services
{
    public class CourseManager : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;

        public CourseManager(IMapper mapper, IDatabaseSetting setting)
        {
            MongoClient client = new(setting.ConnectionString);
            IMongoDatabase database = client.GetDatabase(setting.DatabaseName);

            _courseCollection = database.GetCollection<Course>(setting.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(setting.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDTO>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                courses.ForEach(async course =>
                {
                    course.Category = await _categoryCollection.Find<Category>(category => category.Id == course.CategoryId).FirstAsync();
                });
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }

        public async Task<Response<CourseDTO>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(course => course.Id.Equals(id)).FirstOrDefaultAsync();

            if (course == null)
                return Response<CourseDTO>.Error("course not found", 404);

            course.Category = await _categoryCollection.Find<Category>(category => category.Id.Equals(course.CategoryId)).FirstAsync();

            return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), 200);
        }

        public async Task<Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(course => course.UserId.Equals(userId)).ToListAsync();

            if (courses.Any())
            {
                courses.ForEach(async course =>
                {
                    course.Category = await _categoryCollection.Find<Category>(category => category.Id == course.CategoryId).FirstAsync();
                });
            }
            else
                courses = new List<Course>();

            return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }

        public async Task<Response<CourseDTO>> CreateAsync(CourseCreateDTO model)
        {
            Course created = _mapper.Map<Course>(model);
            created.EditedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(created);

            return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(created), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO model)
        {
            Course updated = _mapper.Map<Course>(model);
            Course result = await _courseCollection.FindOneAndReplaceAsync(course => course.Id.Equals(model.CourseId), updated);

            if (result == null)
                return Response<NoContent>.Error("course not found.", 404);

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(course => course.Id.Equals(id));

            if (result.DeletedCount > 0)
                return Response<NoContent>.Success(204);

            return Response<NoContent>.Error("course not found", 404);
        }
    }
}
