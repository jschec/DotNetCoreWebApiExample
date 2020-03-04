using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

using testApi.Models;

namespace testApi.Controllers
{
    //get request to https://localhost:<port>/patient
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly PatientDbContext _context;

        public PatientController(ILogger<PatientController> logger, PatientDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetAll()
        {
            try
            {
                var patients = _context.Patients.FromSqlRaw("SELECT id, first_name, last_name, date_of_birth, date_added FROM patient_t");
                if (patients == null)
                {
                    return NotFound();
                }
                return await Task.FromResult(patients.ToList());
            }
            catch(Exception)
            {
                return BadRequest();
            }
            
        }

        //get request to https://localhost:<port>/patient/<id>
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(long id)
        {
            Console.WriteLine(id);
            try
            {
                var patient =  await _context.Patients.FromSqlRaw("SELECT id, first_name, last_name, date_of_birth, date_added FROM patient_t WHERE id = {0}", id).SingleOrDefaultAsync();
                if (patient == null)
                {
                    return NotFound();
                }
                return patient;
            }
            catch(Exception) 
            {
                return BadRequest();
            }
        }

        //post request to https://localhost:<port>/patient with JSON body
        [HttpPost]
        public async Task<ActionResult> CreatePatient(Patient patient)
        {
            SqlParameter[] p = new SqlParameter[4];
            p[0] = new SqlParameter("@Id", patient.Id);
            p[1] = new SqlParameter("@FirstName", patient.FirstName);
            p[2] = new SqlParameter("@LastName", patient.LastName);
            p[3] = new SqlParameter("@DateOfBirth", patient.DateOfBirth);

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO patient_t (id, first_name, last_name, date_of_birth) VALUES " +
                    "(@Id, @FirstName, @LastName, @DateOfBirth)", p);

                return CreatedAtAction(
                    nameof(GetPatient),
                    new
                    {
                        Id = patient.Id,
                    },
                    patient);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        //put request to https://localhost:<port>/patient/<id> with JSON body
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPatient(long id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            SqlParameter[] p = new SqlParameter[4];
            p[0] = new SqlParameter("@Id", id);
            p[1] = new SqlParameter("@FirstName", patient.FirstName);
            p[2] = new SqlParameter("@LastName", patient.LastName);
            p[3] = new SqlParameter("@DateOfBirth", patient.DateOfBirth);
   
            try
            {
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE patient_t SET first_name = @FirstName, last_name = @LastName," +
                    "date_of_birth = @DateOfBirth WHERE id = @Id", p);

                if (rowsAffected.Equals(0))
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //delete request to https://localhost:<port>/patient/<id>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> DeletePatient(long id)
        {
            try
            {
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync("DELETE FROM patient_t WHERE id = {0}", id);
                if (rowsAffected.Equals(0))
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
