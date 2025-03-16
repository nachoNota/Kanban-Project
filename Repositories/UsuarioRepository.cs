using tl2_proyecto_2024_nachoNota.Models;

using Microsoft.EntityFrameworkCore;
using ZstdSharp.Unsafe;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly KanbanContext _context;

        public UsuarioRepository(KanbanContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAll() =>
            await _context.Usuarios.ToListAsync();
         

		public async Task<Usuario?> GetByName(string nombreUsuario) =>
            await _context.Usuarios.SingleOrDefaultAsync(u => u.NombreUsuario == nombreUsuario); //singleOrDefault tira una excepcion si encuentra mas de uno
		

		public async Task<Usuario?> GetById(int id) =>
            await _context.Usuarios.FindAsync(id); 
        

        public async Task<bool> Exists(int id) =>
            await _context.Usuarios.AnyAsync(u => u.Id == id);

        public async Task<bool> ExistsByEmail(string email) =>
            await _context.Usuarios.AnyAsync(u => u.Email == email);

        public async Task<Usuario?> GetByEmail(string email) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<Usuario>> SearchByName(string nombreUsuario) =>
            await _context.Usuarios
            .Where(u => u.NombreUsuario.Contains(nombreUsuario))
            .ToListAsync();

        public async Task<string?> GetPasswordById(int id) =>
            await _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => u.Password)
                .FirstOrDefaultAsync();

        public async Task ChangeRol(int idUsuario, RolUsuario rol)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            
            if(usuario != null)
            {
                usuario.Rol = rol;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangePassword(int idUsuario, string pass)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            
            if(usuario != null)
            {
                usuario.Password = pass;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Create(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Usuario usuario)
        {

            var usuarioEncontrado = await _context.Usuarios.FindAsync(usuario.Id);
            
            if (usuarioEncontrado is null) throw new KeyNotFoundException("Usuario no encontrado");

            usuarioEncontrado.NombreUsuario = usuario.NombreUsuario;
            usuarioEncontrado.Email = usuario.Email;

            _context.Entry(usuarioEncontrado).Property(u => u.NombreUsuario).IsModified = true;
            _context.Entry(usuarioEncontrado).Property(u => u.Email).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null) throw new KeyNotFoundException("Usuario no encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> GetNameById(int? id)
        {
            if (id == 0) return string.Empty;

            return await _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => u.NombreUsuario)
                .FirstOrDefaultAsync();

        }
    }
}
