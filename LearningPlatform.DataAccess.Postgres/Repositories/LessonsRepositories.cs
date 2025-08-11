using LearningPlatform.DataAccess.Postgres.Model;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.DataAccess.Postgres.Repositories
{
    public class LessonsRepositories
    {
        private readonly LearningDbContext _dbContext;

        public LessonsRepositories(LearningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddLesson(Guid coursesId, LessonEntity lesson)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == coursesId)
                ?? throw new Exception();

            course.Lessons.Add(lesson);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddLesson2(Guid courseId, string title)
        {
            var lesson = new LessonEntity
            {
                Title = title,
                CourseId = courseId
            };

            await _dbContext.AddAsync(lesson);

            await _dbContext.SaveChangesAsync();
        }
    }
}
