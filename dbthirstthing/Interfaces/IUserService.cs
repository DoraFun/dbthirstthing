using dbthirstthing.DTO;
using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbthirstthing.Interfaces
{
    public interface IUserService
    {
        List<RoleModel> GetAllRoles();

        List<UserDTO> GetUsers();
    }
}
