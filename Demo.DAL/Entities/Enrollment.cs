using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
        public class Enrollment
        {
            public int Id { get; set; }
            public int CourseId { get; set; }
            public Course Course { get; set; }
            public string UserId { get; set; }
            public ApplicationUser User { get; set; }
            public bool IsPaid { get; set; }
           public DateTime EnrollmentDate { get; set; }

           
        }

}
