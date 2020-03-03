using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace testApi.Models
{
    [Table("patient_t")]
    public class Patient
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("first_name")]
        public String FirstName { get; set; }

        [Column("last_name")]
        public String LastName { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("date_added")]
        public DateTime DateAdded { get; set; }
    }
}
