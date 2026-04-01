using Application.Interfaces;
using Domain.Models;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repo
{
	public class RefreshTokenRepo:IRefreshTokenRepository
	{
		private readonly AppDBContext _context;
		public RefreshTokenRepo(AppDBContext context)
		{
			_context = context;
		}
		public async Task<RefreshToken?> GetByTokenAsync(string token)
			=> await _context.RefreshTokens
			.Include(rt => rt.User)
			.FirstOrDefaultAsync(rt => rt.Token == token);
		public async Task AddAsync(RefreshToken token) 
			=> await _context.RefreshTokens.AddAsync(token);
		public async Task SaveChangesAsync()
			=> await _context.SaveChangesAsync();
	}
}
