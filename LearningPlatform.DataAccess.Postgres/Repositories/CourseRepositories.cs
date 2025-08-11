using LearningPlatform.DataAccess.Postgres.Model;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.DataAccess.Postgres.Repositories
{
    public class CourseRepositories
    {
        private readonly LearningDbContext _dbContext;

        public CourseRepositories(LearningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CourseEntity>> Get()
        {
            return await _dbContext.Courses
                .AsNoTracking()/// вот эта функция обязательная запомни ее 
                .OrderBy(c => c.Title)
                .ToListAsync(); 
        }

        public async Task<List<CourseEntity>> GetWithLesson()
        {
            return await _dbContext.Courses
                .AsNoTracking()/// вот эта функция обязательная запомни ее 
                .Include(c => c.Lessons) /// эта тоже нужна
                .ToListAsync();
        }

        public async Task<CourseEntity?> GetById(Guid id)
        {
            return await _dbContext.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CourseEntity>> GetByFilter(string title, decimal price)
        {
            var query = _dbContext.Courses.AsNoTracking();

            if(!string.IsNullOrEmpty(title))
            {
                query = query.Where(c => c.Title.Contains(title));
            }
            if(price > 0)
            {
                query = query.Where(c => c.Price > price);
            }

            return await query.ToListAsync();
        }

        public async Task<List<CourseEntity>> GetByPage(int page, int pageSize) /// пагинация
        {
            return await _dbContext.Courses
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task Add(Guid id, Guid authorId, string title, decimal price, string description)
        {
            var courseEntity = new CourseEntity
            {
                Id = id,
                AuthorId = authorId,
                Title = title,
                Description = description,
                Price = price
            };

            await _dbContext.AddAsync(courseEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Guid id, Guid authorId, string title, decimal price, string description)
        {
            var courseEntiry = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new Exception();

            courseEntiry.Title = title;
            courseEntiry.Description = description;
            courseEntiry.Price = price;

            await _dbContext.SaveChangesAsync();
        }

        public async Task Update2(Guid id, Guid authorId, string title, decimal price, string description)
        {
            await _dbContext.Courses
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.Title, title)
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.Price, price));
        }

        public async Task Delete(Guid id)
        {
            await _dbContext.Courses
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
