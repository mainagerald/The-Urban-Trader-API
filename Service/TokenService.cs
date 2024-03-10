using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using urban_trader_be.Interface;
using urban_trader_be.Model;

namespace urban_trader_be.Service
{
    public class TokenService : iTokenService
    {
        private readonly IConfiguration _iconfig;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration iconfig)
        {
            _iconfig=iconfig;
            _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfig["JWT:SigningKey"])); //encrypts the token uniquely
        }
        public string CreateToken(AppUser appUser)
        {
            var claims= new List<Claim>
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.GivenName, appUser.UserName)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(7),
                SigningCredentials=credentials,
                Issuer=_iconfig["JWT:Issuer"],
                Audience=_iconfig["JWT:Audience"]
            };

            var tokenHandler=new JwtSecurityTokenHandler(); //token created here!

            var token=tokenHandler.CreateToken(tokenDescriptor); //pass the parameters

            return tokenHandler.WriteToken(token); // returns as string
        }
    }
}