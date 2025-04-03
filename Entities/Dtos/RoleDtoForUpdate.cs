using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record RoleDtoForUpdate : RoleDto
    {
        public List<string> Permissions { get; init; } = new List<string>();
    }
}
