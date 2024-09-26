using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        public string Image { get; set; }
        [Required]
        public string Class { get; set; }
        [Required]
        public int RollNumber { get; set; }
        
        public  ICollection<Teacher> Teachers { get; set; }
    }
}
