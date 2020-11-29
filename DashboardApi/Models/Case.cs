using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardApi
{
    public class Case
    {
        [Key]
        public Guid CaseId { get; set; }
        [Required]
        public Guid EntityId { get; set; }
        [Required]
        public int Dead { get; set; }
        [Required]
        public int Infected { get; set; }
        [Required]
        public int Recovered { get; set; }
        [Required]
        public int IntensiveCare { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
    }
}