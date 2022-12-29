using Data.Access.Layer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Layer.Repository
{
    public class GenricRepository : IGenricRepository
    {
        private readonly KitchenerTempBadgeContext _dbContext;
        private static Random rnd = new Random();
        Gaurd ob1 = new Gaurd();

        public GenricRepository(KitchenerTempBadgeContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public IEnumerable<Gaurd> SignInBadge(string fname, string lname, int ecode)
        {
            const string alphanumericCharacters =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789@#*_%!";

            int needLength = 8;

            string randomStr = new string(Enumerable.Range(1, needLength)
                .Select(_ => alphanumericCharacters[rnd.Next(alphanumericCharacters.Length)]).ToArray());


            ob1.FirstName = fname;
            ob1.LastName = lname;
            ob1.EmpCode = ecode;
            ob1.SignIn = DateTime.Now;
            ob1.TempBadge = randomStr;
            _dbContext.Gaurds.Add(ob1);
            _dbContext.SaveChanges();
            return _dbContext.Gaurds.ToList();
        }

        public IEnumerable<Employee> GetEmployees(string fname, string lname)
        {
            var persons = from p in _dbContext.Employees select p;
            if (!string.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(lname))
            {
                persons = persons.Where(p => p.FirstName.StartsWith(fname) && p.LastName.StartsWith(lname));
                return persons.ToList();
            }
            if (!string.IsNullOrEmpty(fname))
            {
                persons = persons.Where(p => p.FirstName.StartsWith(fname));
                return persons.ToList();
            }
            if (!string.IsNullOrEmpty(lname))
            {
                persons = persons.Where(p => p.LastName.StartsWith(lname));
                return persons.ToList();
            }
            return _dbContext.Employees.ToList();
        }


        public IEnumerable<Gaurd> SignOutBadge(int Id)
        {
            var f = _dbContext.Gaurds.FirstOrDefault(x => x.Id == Id);
            f.SignOut = DateTime.Now;

            _dbContext.SaveChanges();
            return _dbContext.Gaurds.ToList();
        }

        public IEnumerable<Gaurd> GetBadges()
        {
            return _dbContext.Gaurds.ToList();
        }

        //================= changes ==================

        public IEnumerable<MultiModelPage> GetMultiModels()
        {
            var data = (from p in _dbContext.Employees
                        join g in _dbContext.Gaurds on p.Empcode equals g.EmpCode
                        select new MultiModelPage
                        {
                            PhotoUrl = p.PhotoUrl,
                            Name = p.FirstName + " " + p.LastName,
                            TempBadge = g.TempBadge,
                            AssignTime = (int)(g.SignOut - g.SignIn).TotalSeconds,
                            SignOut = g.SignOut.ToString()
                        }).ToList();
            foreach (var report in data)
            {
                if (report.SignOut == "0001-01-01 00:00:00.0000000")
                {
                    //report.SignOut = "Active";
                    report.AssignTime = 0;

                }
            }


            return data;
        }


        public IEnumerable<Gaurd> GetReports(DateTime StartDate, DateTime EndDate, string FirstName, string LastName, string Status)
        {
            IQueryable<Gaurd> query = _dbContext.Gaurds.Where(x => x.FirstName != null);
            if (!string.IsNullOrEmpty(FirstName))
            {
                query = query.Where(x => x.FirstName.StartsWith(FirstName));
            }
            if (!string.IsNullOrEmpty(LastName))
            {
                query = query.Where(x => x.LastName.StartsWith(LastName));
            }
            query = query.Where(x => x.SignIn >= StartDate && x.SignIn <= EndDate);


            return query.ToList();

        }


        public IEnumerable<Gaurd> GetNReports()
        {
            return _dbContext.Gaurds.ToList();
        }

        public IEnumerable<Gaurd> SignOutPage(string TempBadge)
        {
            var f = _dbContext.Gaurds.FirstOrDefault(x => x.TempBadge == TempBadge);
            f.SignOut = DateTime.Now;

            _dbContext.SaveChanges();
            return _dbContext.Gaurds;
        }

        public IEnumerable<Report> GetBadgeQueue()
        {
            var data = (from p in _dbContext.Employees
                        join g in _dbContext.Gaurds on p.Empcode equals g.EmpCode
                        select new Report
                        {
                            PhotoUrl = p.PhotoUrl,
                            Name = p.FirstName + " " + p.LastName,
                            TempBadge = g.TempBadge,
                            SignOut = g.SignOut.ToString(),
                            AssignTime = (int)(g.SignOut - g.SignIn).TotalSeconds,
                            Status = "In-Active"
                        }).ToList();
            foreach (var report in data)
            {
                if (report.SignOut == "0001-01-01 00:00:00.0000000")
                {
                    report.Status = "Active";
                }
            }
            return data;
        }



    }
}
