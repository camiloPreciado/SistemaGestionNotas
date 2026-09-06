using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;
using Usuarios.Infrastructure.Persistence;


namespace Usuarios.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);

            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }
    }
}
