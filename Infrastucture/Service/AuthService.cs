using Application.Dtos;
using Application.Interfaces;
using Domain.Models;
using Infrastucture.Repo;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Service
{
	public class AuthService:IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRefreshTokenRepository _refreshTokenRepository;
		private readonly IConfiguration _config;
		public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_refreshTokenRepository = refreshTokenRepository;
			_config = configuration;
		}
		public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
		{
			var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
			if (existingUser != null)
				throw new Exception("Email already exists");

			var user = new User
			{
				FullName = dto.FullName,
				Email = dto.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
			};

			await _userRepository.AddAsync(user);
			await _userRepository.SaveChangesAsync();

			return await GenerateTokenAsync(user);
		}
		
		public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
		{
			var user = await _userRepository.GetByEmailAsync(dto.Email);
			if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
				throw new Exception("Invalid email or password");

			return await GenerateTokenAsync(user);
		}
		public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
		{
			var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

			if (token == null || token.IsRevoked || token.Expires < DateTime.UtcNow)
				throw new Exception("Invalid or expired refresh token");

			token.IsRevoked = true;
			await _refreshTokenRepository.SaveChangesAsync();

			return await GenerateTokenAsync(token.User);
		}

		private async Task<AuthResponseDto> GenerateTokenAsync(User user)
		{
			var accessToken = GenerateAccessToken(user);
			var refreshToken = GenerateRefreshToken();

			await _refreshTokenRepository.AddAsync(new RefreshToken
			{
				Token = refreshToken,
				UserId = user.Id,
				Expires = DateTime.UtcNow.AddDays(7)
			});

			await _refreshTokenRepository.SaveChangesAsync();

			return new AuthResponseDto
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken,
				FullName = user.FullName,
				Role = user.Role
			};
		}
		private string GenerateAccessToken(User user)
		{
		    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
	  {
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role)
		};

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(15),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		private string GenerateRefreshToken()
			=>Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}
}
