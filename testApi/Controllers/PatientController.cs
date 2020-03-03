﻿using System;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(long id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
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
        /*
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
                var newPatient = _context.Visits.FromSqlRaw(
                    "INSERT INTO patient_t (id, first_name, last_name, date_of_birth) VALUES " +
                    "(@Id, @FirstName, @LastName, @DateOfBirth)", p);

                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetPatient),
                    new
                    {
                        Id = patient.Id,
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        DateOfBirth = patient.DateOfBirth,
                        DateAdded = patient.DateAdded
                    },
                    patient);
                )
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        */


        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutPatient(long id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            SqlParameter[] p = new SqlParameter[4];
            p[0] = new SqlParameter("@Id", patient.Id);
            p[1] = new SqlParameter("@FirstName", patient.FirstName);
            p[2] = new SqlParameter("@LastName", patient.LastName);
            p[3] = new SqlParameter("@DateOfBirth", patient.DateOfBirth);
   
            try
            {
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE patient_t SET first_name = @FirstName, last_name = @LastName," +
                    "date_of_birth = @DateOfBirth WHERE id = @Id", p);
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
                return rowsAffected;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> DeletePatient(long id)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM patient_t WHERE id = {0}", id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
