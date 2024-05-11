using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using T3_RevillaFernandezHectorRolando.Models;

namespace T3_RevillaFernandezHectorRolando.Data
{
    public class T3_RevillaFernandezHectorRolandoContext : DbContext
    {
        public T3_RevillaFernandezHectorRolandoContext (DbContextOptions<T3_RevillaFernandezHectorRolandoContext> options)
            : base(options)
        {
        }

        public DbSet<T3_RevillaFernandezHectorRolando.Models.Compra> Compra { get; set; } = default!;
        public DbSet<T3_RevillaFernandezHectorRolando.Models.Empleado> Empleado { get; set; } = default!;
    }
}
