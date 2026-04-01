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
	public class UserRepo :IUserRepository
	{
		private readonly AppDBContext _context;
		public UserRepo(AppDBContext context)
		{
			_context = context;
		}
		public async Task<User?> GetByEmailAsync(string email)
			=> await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		public async Task<User?> GetByIdAsync(int id)
			=> await _context.Users.FindAsync(id);
		public async Task AddAsync(User user)
			=> await _context.Users.AddAsync(user);
		public async Task SaveChangesAsync()
			=> await _context.SaveChangesAsync();
	}
}
