using Data.Access.Layer.Models;
using Data.Access.Layer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Layer.Services
{
    public class EmployeeService : IEmployeeService

    {

        private readonly IGenricRepository _repo;

        public EmployeeService(IGenricRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Employee>GetEmp(string fname, string lname)
        {
            return _repo.GetEmployees(fname,lname);
        }
        /*public IEnumerable<Employee> GetEmploy()
        {
            return _repo.GetEmp();
        }*/
    }
}
