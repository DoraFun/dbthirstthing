using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbthirstthing.Interfaces
{
    internal interface IUserModel
    {
        int userid { get; set; }
        string displayname { get; set; }
        string login { get; set; }
        string pass { get; set; }
        string email { get; set; }
        bool neverlogged { get; set; }
        int? roleid { get; set; }
        RoleModel RoleModel { get; set; }
    }
}
