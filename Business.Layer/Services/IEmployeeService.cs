using Data.Access.Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Business.Layer.Services
{
    public interface IEmployeeService
    {
        public IEnumerable<Employee> GetEmp(string fname, string lname);
        //public IEnumerable<Employee> GetEmploy();
    }
}
