using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dbthirstthing.Interfaces
{
    public interface IPreregistrationService
    {
        List<PreregistrationModel> GetPreregistrationList();
        PreregistrationModel GetPreregistration(int? id);
        void AddPreregistration(PreregistrationModel preregistrationModel);

        void DeletePreregistration(int? id);

        bool IsEmailValid(string email);
        bool IsLoginValid(string login);

        
    }
}
