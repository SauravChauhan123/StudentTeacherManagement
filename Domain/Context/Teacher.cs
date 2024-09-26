using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Sex { get; set; }

        // Navigation property
         public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
         public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
