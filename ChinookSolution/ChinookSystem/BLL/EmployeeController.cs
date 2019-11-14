using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Data.DTOs;
using ChinookSystem.DAL;
using System.ComponentModel;
using ChinookSystem.Data.Entites;
using Chinook.Data.POCOs;
#endregion

namespace ChinookSystem.BLL
{
    public class EmployeeController
    {
        public List<string> Employees_GetTitles()
        {
            using (var context = new ChinookContext())
            {
                var results = (from x in context.Employees
                               select x.Title).Distinct();
                return results.ToList();
            }
        }
    }
}
