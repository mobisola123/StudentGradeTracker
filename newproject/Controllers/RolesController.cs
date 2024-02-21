using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.IRepo;
using System.Threading.Tasks;

namespace StudentGradeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationRepository _authenticationRepository;

        public RolesController(RoleManager<IdentityRole> roleManager, IAuthenticationRepository authenticationRepository)
        {
            _roleManager = roleManager;
            _authenticationRepository = authenticationRepository;
        }

        [HttpPost]
        [Route("addRole")]
        public async Task<IActionResult> AddRoleAsync([FromQuery] AddRoleRequestDto roleRequestDto)
        {
            var result = await _authenticationRepository.AddRoleAsync(roleRequestDto);
            
            if (result.IsSuccessful)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        [Route("addRoleToUser")]
        public async Task<IActionResult> AddRoleToUserAsync([FromQuery] AddRoleToUserRequestDto roleRequestDto)
        {
            var result = await _authenticationRepository.AddRoleToUserAsync(roleRequestDto);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
