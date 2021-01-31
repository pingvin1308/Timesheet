using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Timesheet.BussinessLogic.Exceptions;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.BussinessLogic.Services
{
    public class AuthService : IAuthService
    {
        IEmployeeRepository _employeeRepository;
        public AuthService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        
        public string Login(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException(nameof(lastName));
            }

            var employee = _employeeRepository.Get(lastName);

            if (employee == null)
            {
                throw new NotFoundException($"Employee with last name {lastName}");
            }

            var secret = "secret secret secret secret secret";
            var token = GenerateToken(secret, employee);

            return token;
        }

        public static string GenerateToken(string secret, Employee employee)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(secret);

            var descriptor = new SecurityTokenDescriptor 
            {
                Audience = employee.Position.ToString(),
                Claims = new Dictionary<string, object> 
                { 
                    { "LastName", employee.LastName }  
                },
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

        public static class UserSessions
        {

            public static HashSet<string> Sessions { get; set; } = new HashSet<string>();
        }
    }
}
