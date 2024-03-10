using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public AccountController(UserManager<AppUser> userManager, iTokenService tokenService)
        {
            _userManager=userManager;
            _tokenService=tokenService;
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