using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cseone.Models
{
    public class Participant : BaseEntity
    {
        public int Id { get; set;}
        public int ActivityId { get; set; }
        public Activity activity { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
