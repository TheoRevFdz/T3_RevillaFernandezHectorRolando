using Microsoft.EntityFrameworkCore;
using T3_RevillaFernandezHectorRolando.Data;
using T3_RevillaFernandezHectorRolando.Models;

namespace T3_RevillaFernandezHectorRolando.Services
{
    public class EmpleadoServiceImpl : IEmpleadoService
    {
        private readonly T3_RevillaFernandezHectorRolandoContext _context;

        public EmpleadoServiceImpl(T3_RevillaFernandezHectorRolandoContext context)
        {
            _context = context;
        }

        public async Task<Empleado> GetEmpleado(string email, string password)
        {
            Empleado result = await _context.Empleado.Where(e => e.email == email && e.password == password)
                .FirstOrDefaultAsync();
            return result;
        }
    }
}
