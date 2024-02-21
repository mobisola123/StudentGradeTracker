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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        [HttpPost]
        [Route("addSubject/{id}")]
        public async Task<ActionResult> AddAsync(Guid id, SubjectRegistrationRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<GetSubjectResponseDTO>()
                {
                    ResponseCode = "66",
                    IsSuccessful = false,
                    Description = "invalid payload"
                });
            }
            var response = await _subjectRepository.AddAsync(id, model);

            if (response.IsSuccessful == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getByIdAsync")]
        public async Task<ActionResult> GetAsync(Guid id) 
        {
            var response = await _subjectRepository.GetByIdAsync(id);
            if (response.IsSuccessful == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getAllAsync")]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _subjectRepository.GetAllAsync());
        }

        [HttpDelete]
        [Route("deleteAsync")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
          var response = await _subjectRepository.DeleteAsync(id);
            if (response.IsSuccessful == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }




    }
}
