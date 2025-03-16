using Company.Models;
using Company.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController(IPermissionService permissionService) : ControllerBase
    {
        private readonly IPermissionService _permissionService = permissionService;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetPermissions()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            if (permissions == null || !permissions.Any())
            {
                return NotFound();
            }
            return Ok(permissions);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> GetPermission(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                return NotFound();
            }
            return Ok(permission);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Permission>> CreatePermission(Permission permission)
        {
            if (permission == null)
            {
                return BadRequest("Permission data is required.");
            }

            await _permissionService.CreatePermissionAsync(permission);
            return CreatedAtAction(nameof(GetPermission), new { id = permission.PermissionID }, permission);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] Permission updatedPermission)
        {
            if (updatedPermission == null)
            {
                return BadRequest("Invalid permission data.");
            }

            var existingPermission = await _permissionService.GetPermissionByIdAsync(id);
            if (existingPermission == null)
            {
                return NotFound("Permission not found.");
            }

            if (!string.IsNullOrEmpty(updatedPermission.PermissionName))
            {
                existingPermission.PermissionName = updatedPermission.PermissionName;
            }

            await _permissionService.UpdatePermissionAsync(existingPermission);
            return NoContent();
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var result = await _permissionService.DeletePermissionAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
