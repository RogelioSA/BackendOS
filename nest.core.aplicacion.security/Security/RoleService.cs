using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using nest.core.dominio.Excepciones;
using nest.core.dominio.Security;
using nest.core.infraestructura.db.DbContext;

namespace nest.core.aplicacion.security.Security
{
    public class RoleService
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly NestDbContext context;
        public RoleService(RoleManager<ApplicationRole> roleManager, NestDbContext context)
        {
            this.roleManager = roleManager;
            this.context = context;
        }

        public async Task<List<ApplicationRole>> ObtenerTodos() => await this.roleManager.Roles.ToListAsync();
        public async Task<ApplicationRole> ObtenerPorId(string id) => await this.roleManager.FindByIdAsync(id);

        public async Task<ApplicationRole> Agregar(ApplicationRoleDto roleDto)
        {
            string lastValue = (await this.context.Roles.IgnoreQueryFilters().Select(x => x.Id).DefaultIfEmpty().MaxAsync(x => x)) ?? "0";
            ApplicationRole role = new ApplicationRole
            {
                Id = (long.Parse(lastValue) + 1).ToString(),
                Name = roleDto.Name,
                NormalizedName = roleDto.Name.ToUpper(),
                EmpresaId = roleDto.EmpresaId,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            IdentityResult result = await this.roleManager.CreateAsync(role);
            if (result.Succeeded)
                return await this.roleManager.FindByNameAsync(role.Name);
            else throw new IdentityException(result.Errors.Select(x => new dominio.Excepciones.IdentityError { Code = x.Code, Description = x.Description }).ToList());
        }

        public async Task<ApplicationRole> Modificar(ApplicationRoleDto role, int roleId)
        {
            string currRoleId = roleId.ToString();
            ApplicationRole currentRole = this.context.Roles.Where(x => x.Id == currRoleId).First();
            currentRole.Name = role.Name;
            currentRole.NormalizedName = role.Name.ToUpper();
            IdentityResult result = await this.roleManager.UpdateAsync(currentRole);
            if (result.Succeeded)
                return await this.roleManager.FindByNameAsync(role.Name);
            else throw new IdentityException(result.Errors.Select(x => new dominio.Excepciones.IdentityError { Code = x.Code, Description = x.Description }).ToList());
        }

        public async Task<bool> Eliminar(int roleId)
        {
            ApplicationRole currentRole = this.context.Roles.Where(x => x.Id == roleId.ToString()).First();
            IdentityResult result = await this.roleManager.DeleteAsync(currentRole);
            if (result.Succeeded)
                return true;
            else throw new IdentityException(result.Errors.Select(x => new dominio.Excepciones.IdentityError { Code = x.Code, Description = x.Description }).ToList());
        }
    }
}
