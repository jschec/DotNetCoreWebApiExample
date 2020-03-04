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
    [ApiController]
    [Route("[controller]")]
    public class VisitController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly PatientDbContext _context;

        public VisitController(ILogger<PatientController> logger, PatientDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Visit>>> GetAll()
        {
            try
            {
                var visits = _context.Visits.FromSqlRaw("SELECT id, patient_id, type, description, date FROM visit_t");
                if (visits == null)
                {
                    return NotFound();
                }
                return await Task.FromResult(visits.ToList());
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        /*
        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> GetPatient(long id)
        {
            try
            {
                var visit = _context.Visits.FromSqlRaw("SELECT id, patient_id, type, description, date FROM visit_t WHERE id = {0}", id);
                //var patient = await _context.Patients.FindAsync(id);
                if (visit == null)
                {
                    return NotFound();
                }
                return await Visit(visit);
                //return await Task.FromResult(Visit(visit));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        */

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutVisit(long id, Visit visit)
        {
            if (id != visit.Id)
            {
                return BadRequest();
            }

            SqlParameter[] p = new SqlParameter[5];
            p[0] = new SqlParameter("@Id", visit.Id);
            p[1] = new SqlParameter("@PatientId", visit.PatientId);
            p[2] = new SqlParameter("@Type", visit.Type);
            p[3] = new SqlParameter("@Description", visit.Description);
            p[4] = new SqlParameter("@Date", visit.Date);

            try
            {
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE visit_t SET type = @Type, description = @Description," +
                    "date = @Date WHERE id = @Id AND patient_id = @PatientId", p);
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
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM visit_t WHERE id = {0}", id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
