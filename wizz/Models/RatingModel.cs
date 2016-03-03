using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class RatingModel
    {
        [Required]
        public string fkTutorId { get; set; }
        [Required]
        public string fkStudentId { get; set; }
          [Required]
        public string knowledgeRating { get; set; }
          [Required]
        public string helpfulRating { get; set; }
          [Required]
        public string punctualRating { get; set; }

    }
    public class StudentRatingModel
    {
        [Required]
        public string tutorId { get; set; }
        [Required]
        public string studentId { get; set; }
        [Required]
        public string studentRating { get; set; }


    }
}