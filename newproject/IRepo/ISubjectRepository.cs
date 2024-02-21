using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Generic;

namespace StudentGradeTracker.IRepo
{
    public interface ISubjectRepository
    {
        Task<Response<GetSubjectResponseDTO>> AddAsync(Guid studentId, SubjectRegistrationRequestDTO model);
        Task<Response<GetSubjectResponseDTO>> GetByIdAsync(Guid id);
        Task<ResponseList<GetSubjectResponseDTO>> GetAllAsync();
        Task<Response<bool>> DeleteAsync(Guid id);
    }
}