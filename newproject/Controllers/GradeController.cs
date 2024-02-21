using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Generic;
using StudentGradeTracker.IRepo;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeRepository _gradeRepository;
        public GradeController(IGradeRepository gradeRepository) 
        {
            _gradeRepository = gradeRepository;
        }
        [HttpPost]
        [Route("addGradeAsync")]
        public async Task<ActionResult> AddAsync([FromRoute]Guid id, [FromBody]GradeRegistrationRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<GetGradeResponseDTO>()
                {
                    ResponseCode = "66",
                    IsSuccessful = false,
                    Description = "Invalid payload"
                });
            }
            var response = await _gradeRepository.AddAsync(id, model);

            if (response is null)
            {
                response.IsSuccessful = false;
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getStudentGradesAsync/{id}")]
        public async Task<IActionResult> GetStudentGrades(Guid id)
        {
            var response = await _gradeRepository.GetGrades(id);
            return Ok(response);
        }


        [HttpGet]
        [Route("getStudentAverageGrades/{id}")]
        public async Task<IActionResult> GetStudentAverageGrades(Guid id)
        {
            var response = await _gradeRepository.GetAverageGrade(id);
            return Ok(response);
        }
    }
    
}

