using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Resitories
{
    public class CourseRepo : ICourseRepo
    {
        private readonly MvcProjectDbContext _dbContext;

        public CourseRepo(MvcProjectDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Add(Course course)
        {
            _dbContext.Add(course);
            return _dbContext.SaveChanges(); ;
        }

        public int Delete(Course course)
        {
            _dbContext.Remove(course);
            return _dbContext.SaveChanges();

        }

        public IEnumerable<Course> GetAll()
        => _dbContext.Courses.Include(c => c.Instructor);

        public Course GetById(int CourseId)
        {
            return _dbContext.Courses.FirstOrDefault(c => c.Id == CourseId);
        }

        public void Update(Course course)
        {
            _dbContext.Courses.Update(course);
            _dbContext.SaveChanges();
        }
    }
}
