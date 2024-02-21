using Microsoft.AspNetCore.Identity;
using StudentGradeTracker.DTO.RequestDTO;
using StudentGradeTracker.DTO.ResponseDTO;
using StudentGradeTracker.Generic;
using StudentGradeTracker.IRepo;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeTracker.Repo
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenRepository _jwtRepository;

        public AuthenticationRepository(
            UserManager<IdentityUser> userManager,
            IJwtTokenRepository jwtRepository,
            RoleManager<IdentityRole> roleManager)
        {
       
            _userManager = userManager;
            _jwtRepository = jwtRepository;
            _roleManager = roleManager;
        }

        public async Task<Response<bool>> AddRoleAsync(AddRoleRequestDto roleRequestDto)
        {
            Response<bool> response = new();

            try
            {
                var role = roleRequestDto.Role.ToString().ToLower();

                //check if role exist
                var roleExist = await _roleManager.FindByNameAsync(role);

                if (roleExist == null)
                {
                    IdentityRole newRole = new()
                    {
                        Name = role
                    };

                    var addRole = await _roleManager.CreateAsync(newRole);

                    if (addRole.Succeeded)
                    {
                        response.IsSuccessful = true;
                        response.Description = "Role created successfully";
                    }

                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "Failed"
                    };
                    response.Description = "Role not added";
                    return response;
                }

                response.IsSuccessful = false;
                response.Error = new ResponseError()
                {
                    ErrorCode = 21,
                    ErrorMessage = "Role already exist"
                };
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

        public async Task<Response<GetLoginResponseDTO>> LoginAsync(LoginRequestDTO model)
        {
            Response<GetLoginResponseDTO> response = new();

            try
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);

                if (user == null)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "Invalid credential"
                    };
                    response.Description = "Login failed";
                    return response;
                }

                //var existingUser = await _signinManager.PasswordSignInAsync(model.EmailAddress, model.Password, false, false);

                var existingUser = await _userManager.CheckPasswordAsync(user, model.Password);

                if (existingUser == false)
                {
                    response.IsSuccessful = false;
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 21,
                        ErrorMessage = "Invalid credential"
                    };
                    response.Description = "Login failed";
                    return response;
                }

                var token = await _jwtRepository.GenerateJwtToken(user);

                response.Data = new GetLoginResponseDTO()
                {
                    Token = token
                };
                response.IsSuccessful = true;

                response.ResponseCode = "00";
                response.Description = "Successful";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 21,
                    ErrorMessage = "Internal server error"
                };
                return response;
            }

        }

        public async Task<Response<bool>> AddRoleToUserAsync(AddRoleToUserRequestDto model)
        {
            Response<bool> response = new Response<bool>();

            try
            {
                //check for the user with the id provided
                var identityUser = await _userManager.FindByIdAsync(model.IdentityId);

                if (identityUser == null)
                {
                    response.Error = new ResponseError()
                    {
                        ErrorCode = 52,
                        ErrorMessage = "User does not exist"
                    };
                    response.ResponseCode = "30";
                    response.IsSuccessful = false;
                    response.Description = "Failed";
                    return response;
                }

                //Check if role exist
                string role = model.Role.ToString();

                bool roleExist = await _roleManager.RoleExistsAsync(role);

                if (roleExist)
                {
                    //check if user has any role
                    var userRoles = await _userManager.GetRolesAsync(identityUser);

                    if (userRoles.Count() > 0)
                    {
                        response.Error = new ResponseError()
                        {
                            ErrorCode = 53,
                            ErrorMessage = $"User already has role of {userRoles[0]}"
                        };
                        response.ResponseCode = "34";
                        response.IsSuccessful = false;
                        response.Description = "Failed";
                        return response;
                    }

                    //Add the role to the user
                    var addRole = await _userManager.AddToRoleAsync(identityUser, role);

                    if (addRole.Succeeded)
                    {
                        response.ResponseCode = "00";
                        response.Data = true;
                        response.IsSuccessful = true;
                        response.Description = "Success";
                        return response;
                    }

                    response.Error = new ResponseError()
                    {
                        ErrorCode = 53,
                        ErrorMessage = "Cannot add role at the moment. Please try again later."
                    };
                    response.ResponseCode = "31";
                    response.IsSuccessful = false;
                    response.Description = "Failed";
                    return response;
                }

                response.Error = new ResponseError()
                {
                    ErrorCode = 54,
                    ErrorMessage = "Invalid role."
                };
                response.ResponseCode = "31";
                response.IsSuccessful = false;
                response.Description = "Failed";
                return response;
            }
            catch (Exception ex)
            {

               Console.WriteLine(ex.ToString());
                response.Error = new ResponseError()
                {
                    ErrorCode = 21,
                    ErrorMessage = "Internal server error"
                };
                return response;
            }
        }
    }
}
