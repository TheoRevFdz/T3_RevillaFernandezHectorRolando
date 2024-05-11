using T3_RevillaFernandezHectorRolando.Models;

namespace T3_RevillaFernandezHectorRolando.Services
{
    public interface IEmpleadoService
    {
        Task<Empleado> GetEmpleado(string email, string password);
    }
}
