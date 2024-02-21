using Microsoft.AspNetCore.Mvc;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Generic;
using System.Threading.Tasks;

namespace StudentGradeTracker.IRepo
{
    public interface IAuthenticationRepository
    {
        Task<Response<GetLoginResponseDTO>> LoginAsync(LoginRequestDTO model);
        Task<Response<bool>> AddRoleToUserAsync(AddRoleToUserRequestDto model);
        Task<Response<bool>> AddRoleAsync(AddRoleRequestDto roleRequestDto);
    }
}
