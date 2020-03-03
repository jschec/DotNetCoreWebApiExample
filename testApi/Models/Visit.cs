using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace testApi.Models
{
    [Table("visit_t")]
    public class Visit
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("type")]
        public String Type { get; set; }

        [Column("description")]
        public String Description { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
    }
}
