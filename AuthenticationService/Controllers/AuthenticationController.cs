using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.DbModels;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Controllers
{
    
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationServiceContext ctx;
        private readonly int transientPolicy = 300;
        public AuthenticationController(AuthenticationServiceContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet]
        [Route("authenticationUser")]
        public async Task<ActionResult<Guid>> GetToken(string username, string password)
        {
            AuthenticationUser user = await ctx.AuthenticationUser.FindAsync(username);
            if(user == null)
            {
                return NotFound();
            }
            byte[] passwordByte = GetHash(password + user.salt);
            if (!user.password.SequenceEqual(passwordByte))
            {
                return Unauthorized();
            }
            Guid token = Guid.NewGuid();
            DateTime dateTime = DateTime.UtcNow;

            user.temp_token = token;
            user.temp_token_date = dateTime;

            ctx.AuthenticationUser.Update(user);
            await ctx.SaveChangesAsync();
            return Ok(token);
        }

        [HttpGet]
        [Route("authenticationToken")]
        public async Task<ActionResult> checkToken(Guid token)
        {
            AuthenticationUser user = await ctx.AuthenticationUser.SingleOrDefaultAsync(user => user.temp_token == token);
            if(user == null)
            {
                return NotFound();
            }
            DateTime now = DateTime.UtcNow;
            if(now.Subtract(user.temp_token_date).TotalSeconds > transientPolicy)
            {
                return Unauthorized();
            }
            user.temp_token_date = now;
            ctx.AuthenticationUser.Update(user);
            await ctx.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("creteUser")]
        public async Task<ActionResult> createUser(string username, string password, Guid token)
        {
            AuthenticationUser user = await ctx.AuthenticationUser.SingleOrDefaultAsync(user => user.temp_token == token);
            if (user == null)
            {
                return NotFound();
            }
            DateTime now = DateTime.UtcNow;
            if (now.Subtract(user.temp_token_date).TotalSeconds > transientPolicy)
            {
                return Unauthorized();
            }
            user = await ctx.AuthenticationUser.FindAsync(username);
            if (user != null)
            {
                return Conflict("El usuario indicado ya existe");
            }
            string salt = Guid.NewGuid().ToString().Substring(0, 6);
            user = new AuthenticationUser
            {
                username = username,
                salt = salt,
                password = GetHash(password+salt)
            };            
            ctx.AuthenticationUser.Add(user);
            await ctx.SaveChangesAsync();
            return Ok();
        }


        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
