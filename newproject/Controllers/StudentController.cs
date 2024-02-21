using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Generic;
using StudentGradeTracker.IRepo;
using System;
using System.Threading.Tasks;

namespace StudentGradeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpPost]
        [Route("registerStudent")]
        //[Produces(typeof(GetStudentResponseDTO))]
        [AllowAnonymous]
        public async Task<ActionResult> AddAsync(StudentRegistrationRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new Response<GetStudentResponseDTO>()
                    {
                        ResponseCode = "66",
                        IsSuccessful = false,
                        Description = "Invalid payload"
                    });
            }
        
            var response = await _studentRepository.AddAsync(model);

            if (response.IsSuccessful == false)
            {
               return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("getStudent/{id}")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            var response = await _studentRepository.GetByIdAsync(id);

                if (response.IsSuccessful == false)
                {
                    return NotFound(response);
                }

                return Ok(response);
        }

        [HttpGet]
        [Route("getAllStudent")]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _studentRepository.GetAllAsync());
        }

        [HttpPut]
        [Route("updateStudent/{id}")]
        public async Task<ActionResult> UpdateAsync( Guid id, UpdateStudentRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<bool>()
                {
                   ResponseCode ="66",
                   IsSuccessful = false,
                   Description = "invalid payload"
                });
            }

            var response = await _studentRepository.UpdateAsync(id, model);

            if (response.IsSuccessful== true)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete]
        [Route("deleteStudent/{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var response = await _studentRepository.DeleteAsync(id);
            
            if(response.IsSuccessful == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
