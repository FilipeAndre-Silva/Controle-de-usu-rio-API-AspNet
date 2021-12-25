using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UserControl.Api.Services;
using UserControl.Api.ViewModels.User;
using UserControl.Domain.Models;
using UserControl.Infra.Repository;

namespace UserControl.Api.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        protected readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            return userViewModel == null ? NotFound() : Ok(userViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync()
        {
            var users = await _userRepository.GetUsers();
            var usersViewModel = _mapper.Map<List<UserViewModel>>(users);
            return !usersViewModel.Any() ? NotFound() : Ok(usersViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel createUserViewModel)
        {   
            var user = _mapper.Map<User>(createUserViewModel);
            if(!user.IsValid())
            {
                return BadRequest(user.validationResult);
            }
            user.Password = new EncryptPassword().criptografarSenha(user.Password);
            try
            {
                var createdUser = await _userRepository.CreateUserAsync(user);
                //var a = createdUser.IsValid();
                var userViewModel = _mapper.Map<UserViewModel>(createdUser);
                return Created($"v1/users/{userViewModel.Id}", userViewModel);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "employee,manager")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUserViewModel updateUserViewModel)
        {
            if (id != updateUserViewModel.Id)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            var user = _mapper.Map<User>(updateUserViewModel);
            var updatedUser = await _userRepository.UpdateUserAsync(user);
            var userViewModel = _mapper.Map<UserViewModel>(updatedUser);
            return Ok(userViewModel);

        }

        [HttpPatch]
        [Route("{id}")]
        [Authorize(Roles = "employee,manager")]
        // https://docs.microsoft.com/pt-br/aspnet/core/web-api/jsonpatch?view=aspnetcore-6.0
        // https://github.com/dotnet/aspnetcore/issues/13938

        /*
        [   
            {
                "op": "add",
                "path": "/customerName",
                "value": "Barry"
            },
            {
                "op": "add",
                "path": "/orders/-",
                "value": {
                "orderName": "Order2",
                "orderType": null
                }
            }
        ]
        */
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<User> patch)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            patch.ApplyTo(user);
            var updatedUser = await _userRepository.UpdateUserAsync(user);
            var userViewModel = _mapper.Map<UserViewModel>(updatedUser);
            return Ok(userViewModel);

        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return BadRequest(new { message = "Não foi possível remover o usuário." });
            }

            var deletedUser = await _userRepository.DeleteUserAsync(user.Id);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromServices] IConfiguration configuration,
                                                                   [FromBody]LoginUserViewModel credentials)
        {
            credentials.Password = new EncryptPassword().criptografarSenha(credentials.Password);
            var user = await _userRepository.AuthenticateAsync(credentials.Username, credentials.Password);

            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos." });
            }

            var token = new TokenService(configuration).GenerateToken(user);

            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
        }
    }
}