using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab7.Data;
using Lab7.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure;

namespace Lab7.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        /// Get collection of Students.
        /// <returns>A collection of Students</returns>
        /// <response code="200">Returns a collection of Students</response>
        /// <response code="500">Internal error</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]                            
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]            
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        /// <returns>A Student</returns>
        /// <param id="id"></param>
        /// <response code="200">Returns a Student</response>
        /// <response code="400">If the id is malformed</response> 
        /// <response code = "404" > If the Student is null</response>
        /// <response code="500">Internal error</response>
        [HttpGet("{id}")]                                                   
        [ProducesResponseType(StatusCodes.Status200OK)]                    
        [ProducesResponseType(StatusCodes.Status400BadRequest)]          
        [ProducesResponseType(StatusCodes.Status404NotFound)]              
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]   
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <param id="id"></param>
        /// <returns>An updated Student</returns>
        /// <response code="200">Returns the updated Student</response>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student or id is malformed</response>      
        /// <response code="500">Internal error</response>

        [HttpPut("{id}")]                                                 
        [ProducesResponseType(StatusCodes.Status200OK)]                    
        [ProducesResponseType(StatusCodes.Status400BadRequest)]             
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  
        public async Task<IActionResult> PutStudent(Guid id, Student student)
        {
            Student? isAstudent = null;

            try

            {
                isAstudent = _context.Students.Single(c => c.Id == id);
                isAstudent.FirstName = student.FirstName;
                isAstudent.LastName = student.LastName;
                isAstudent.Program = student.Program;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(isAstudent); ;
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <returns>A newly created Students</returns>
        /// <response code="201">Returns the newly created Students</response>
        /// <response code="400">If the Students is malformed</response>      
        /// <response code="500">Internal error</response>    
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]                            
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                         
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]               
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        /// <param id="id"></param>
        /// <response code="202">Student is deleted</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="500">Internal error</response>

        [HttpDelete("{id}")]                                                      
        [ProducesResponseType(StatusCodes.Status202Accepted)]                    
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                    
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]         
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
