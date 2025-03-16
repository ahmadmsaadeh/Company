using Company.Models;
using Company.Repositories;

namespace Company.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IGenericRepository<Permission> _permissionRepository;

        public PermissionService(IGenericRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync() => await _permissionRepository.GetAllAsync();

        public async Task<Permission> GetPermissionByIdAsync(int id) => await _permissionRepository.GetByIdAsync(id);

        public async Task CreatePermissionAsync(Permission permission)
        {
            ArgumentNullException.ThrowIfNull(permission);
            await _permissionRepository.AddAsync(permission);
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            ArgumentNullException.ThrowIfNull(permission);
            await _permissionRepository.UpdateAsync(permission);
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null) return false;

            await _permissionRepository.DeleteAsync(id);
            return true;
        }
    }
}
