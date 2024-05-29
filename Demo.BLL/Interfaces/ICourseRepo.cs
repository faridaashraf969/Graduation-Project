using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ICourseRepo
    {
        IEnumerable<Course> GetAll();
        Course GetById(int id);
        int Add(Course course);
        void Update(Course course);
        int Delete(Course course);
    }
}
