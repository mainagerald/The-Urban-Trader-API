using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using urban_trader_be.DTO.Account;
using urban_trader_be.Interface;
using urban_trader_be.Model;
using urban_trader_be.Service;

namespace urban_trader_be.Controller
{
    [Route("urban_trader_be/account")]
    [ApiController]
    public class AccountController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly iTokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, iTokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager=userManager;
            _tokenService=tokenService;
            _signInManager=signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName==loginDto.Username.ToLower());
            if(user==null){
                return Unauthorized("Invalid username!");
            }

            var result= await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(!result.Succeeded) return Unauthorized("Incorrect password or username!");

            return Ok
            (
                new NewUserDto
                {
                    Username=user.UserName,
                    Email=user.Email,
                    Token=_tokenService.CreateToken(user)
                }
            );
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try{
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var appUser=new AppUser
                {
                    UserName=registerDto.Username,
                    Email=registerDto.Email
                };

                var createdUser=await _userManager.CreateAsync(appUser, registerDto.Password);

                if(createdUser.Succeeded){
                    var roleResult= await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded){
                        return Ok(
                            new NewUserDto{
                                Username=appUser.UserName,
                                Email=appUser.Email,
                                Token=_tokenService.CreateToken(appUser)
                            }
                        );
                    }else{
                        return StatusCode(500, roleResult.Errors);
                    }
                } else {
                    return StatusCode(500, createdUser.Errors);
                }

            }
            catch(Exception e)
            {
                return StatusCode(500, e);  //user manager is complex stuff man
            }
        }
        
    }
}