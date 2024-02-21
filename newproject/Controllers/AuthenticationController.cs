using Microsoft.AspNetCore.Mvc;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Generic;
using StudentGradeTracker.IRepo;
using System.Threading.Tasks;

namespace StudentGradeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _repository;
        public AuthenticationController(IAuthenticationRepository repository)
        {
                _repository = repository;
        }

        [HttpPost]
        [Route("loginAsync")]
        public async Task<ActionResult> LoginAsync(LoginRequestDTO model)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new Response<GetLoginResponseDTO>()
                {
                    ResponseCode = "66",
                    IsSuccessful = false,
                    Description = "Login failed"
                });
               
            }
            var response = await _repository.LoginAsync(model);
            if (response.IsSuccessful == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
    }
}
