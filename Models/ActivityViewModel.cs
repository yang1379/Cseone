using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cseone.Models
{
    public class ActivityViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(2, ErrorMessage = "Title must contain at least 2 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [MyDateAttribute(ErrorMessage ="Date must be in the future.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime ActivityDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Time")]
        public DateTime ActivityTime { get; set; }
        
        [Required(ErrorMessage = "Duration is required.")]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Duration Units is required.")]
        [Display(Name = "Units")]
        public string Units { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MinLength(10, ErrorMessage = "Description must contain at least 10 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
