﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using EcommerceApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using EcommerceApi.ConnecDB;
using EcommerceApi;


using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;
using Org.BouncyCastle.Ocsp;
using Microsoft.AspNetCore.Identity;

namespace JwtWebApiTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authCol : ControllerBase
    {
        private const string TicketIssuedTicks = nameof(TicketIssuedTicks);
        public class myParam
        {
            public string name;
            public object value;
        }

        public static Users user = new Users();
        PasswordModels pass = new PasswordModels();
        ConnecDb conn = new ConnecDb();
        private readonly IConfiguration _configuration;

        public authCol(IConfiguration configuration)
        {
            _configuration = configuration;

        }


        [AllowAnonymous]
        [HttpPost("login")]
        //[ValidateAntiForgeryToken]
        public ActionResult<string> Login(UserDto request)
        {


            user.UserId = conn.CheckIduser(request);
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = conn.getRole(request);
            if (user.UserId == 0)
            {
                return BadRequest("User not found.");
            }
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }
            user.Username = request.Username;

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]

        public ActionResult<string> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddHours(10),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(Users user)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);
           
            
            var token = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim("session", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role,  user.Role)
                },

                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);


            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA384())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA384(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



    }
}
