using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record RoleDtoForInsert : RoleDto
    {
        public string? RoleId { get; init; }
        public List<string> Permissions { get; init; } = new List<string>();
    }
}
