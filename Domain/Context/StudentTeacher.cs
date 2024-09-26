using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class StudentTeacher
    {
        [Key]
        public int Id { get; set; } 
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
    }
}
