using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using newproject;
using newproject.Migrations;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Entities;
using StudentGradeTracker.Generic;
using StudentGradeTracker.IRepo;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeTracker.Repo
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenRepository _jwtRepository;

        public StudentRepository(AppDbContext context, IMapper mapper, UserManager<IdentityUser> userManager, IJwtTokenRepository jwtRepository)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _jwtRepository = jwtRepository;
        }

        public async Task<Response<GetStudentResponseDTO>> AddAsync(StudentRegistrationRequestDTO model)
        {
            Response<GetStudentResponseDTO> response = new();

            try
            {
                //check if user already exist
                var existingUser = await _userManager.FindByEmailAsync(model.EmailAddress);

                if (existingUser != null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "User already exist"
                    };

                    return response;
                }

                //check if student already exist
                var existingStudent = await _context.Students
                    .Where(x => x.EmailAddress == model.EmailAddress)
                .FirstOrDefaultAsync();

                //do a method here that won't save the student twice//
                if (existingStudent != null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "Student already exist"
                    };

                    return response;
                }

                IdentityUser identityUser = new()
                {
                    Email = model.EmailAddress,
                    UserName = model.EmailAddress

                };

                var addUser = await _userManager.CreateAsync(identityUser, model.Password);

                if (!addUser.Succeeded)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "Student Registeration Failed"
                    };

                    return response;
                }

                //add the student to database
                Student student = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    IdentityId = identityUser.Id

                };

                var addStudent = await _context.Students.AddAsync(student);

                int isSaved = await _context.SaveChangesAsync();

                if (isSaved < 1)
                {
                    await _userManager.DeleteAsync(identityUser);
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "Student Registeration Failed"
                    };

                    return response;
                }

                //GetStudentResponseDTO responseDto = new()
                //{
                //    Id = addStudent.Entity.Id,
                //    FirstName = addStudent.Entity.FirstName,
                //    LastName = addStudent.Entity.LastName,
                //    EmailAddress = addStudent.Entity.EmailAddress
                //};

                var responseDto = _mapper.Map<GetStudentResponseDTO>(addStudent.Entity);

                response.IsSuccessful = true;
                response.Data = responseDto;
                response.ResponseCode = "00";
                response.Description = "Successful";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "internal server error"
                };
                response.Description = "please try again later";
            }
            return response;
        }

        public async Task<ResponseList<GetStudentResponseDTO>> GetAllAsync()
        {
            ResponseList<GetStudentResponseDTO> response = new();

            try
            {
                var students = await _context.Students
                    .ProjectTo<GetStudentResponseDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                if (!students.Any())
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 30,
                        ErrorMessage = "No record found"
                    };
                    return response;
                }

                //var studentResponseList = new List<GetStudentResponseDTO>();

                //foreach (Student student in students)
                //{
                //    GetStudentResponseDTO studentResponseDTO = new()

                //    {
                //        Id = student.Id,
                //        FirstName = student.FirstName,
                //        LastName = student.LastName,
                //        EmailAddress = student.EmailAddress,
                //    };

                //    studentResponseList.Add(studentResponseDTO);
                //}

                response.IsSuccessful = true;
                response.Data = students;
                response.ResponseCode = "00";
                response.Description = "succesful";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "internal server error"
                };
                response.Description = "please try again later";
                return response;
            }
        }

        public async Task<Response<GetStudentResponseDTO>> GetByIdAsync(Guid studentId)
        {
            Response<GetStudentResponseDTO> response = new();
            try
            {
                var student = await _context.Students
                    .Where(x => x.Id == studentId)
                    .ProjectTo<GetStudentResponseDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (student == null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 22,
                        ErrorMessage = "StudentId does not exist"
                    };
                    return response;
                };

                //GetStudentResponseDTO responseDTO = new()
                //{
                //    Id = student.Id,
                //    FirstName = student.FirstName,
                //    LastName = student.LastName,
                //    EmailAddress = student.EmailAddress
                //};

                //var responseDTO = _mapper.Map<GetStudentResponseDTO>(student);
                response.IsSuccessful = true;
                response.Data = student;
                response.ResponseCode = "00";
                response.Description = "successful";
                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "Internal server error"
                };
                response.Description = "please try again later";
            }
            return response;
        }

        public async Task<Response<bool>> DeleteAsync(Guid studentId)
        {
            Response<bool> response = new();

            try
            {
                var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

                if (student == null)
                {
                    response.IsSuccessful = false;
                    response.Data = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 23,
                        ErrorMessage = "StudentId does not exist"
                    };
                    return response;
                }
                _context.Students.Remove(student);
                _context.SaveChanges();
                response.IsSuccessful = true;
                response.Data = true;
                response.ResponseCode = "00";
                response.Description = "successful";
                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "Internal server error"
                };
                response.Description = "please try again later";
                return response;
            }

        }

        public async Task<Response<bool>> UpdateAsync(Guid studentId, UpdateStudentRequestDTO model)
        {
            Response<bool> response = new();

            try
            {
                var student = _context.Students.Where(x => x.Id == studentId).FirstOrDefault();

                if (student == null)
                {
                    response.IsSuccessful = false;
                    response.Data = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 24,
                        ErrorMessage = "StudentId does not exist"
                    };
                    return response;
                }
                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.EmailAddress = model.EmailAddress;

                await _context.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Data = true;
                response.ResponseCode = "00";
                response.Description = "successful";
                return response;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 55,
                    ErrorMessage = "Internal server error"
                };
                response.Description = "please try again later";
                return response;
            }
        }
    }

}