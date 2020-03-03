using Microsoft.EntityFrameworkCore;

namespace testApi.Models
{
    public class PatientDbContext : DbContext
    {
       public PatientDbContext(DbContextOptions<PatientDbContext> options)
           : base(options)
       {
       }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
    }
}