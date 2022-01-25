using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections;
using Ofisprojesi;
using System.Web.Http.ModelBinding;

namespace ofisprojesi
{
    public interface IEmployeeService
    {
        EmployeeDto[] GetEmployeeByName(string name, bool? status);
        EmployeeDto GetEmployeeById(int? id);
        DbActionResult DeleteEmployeById(int? id);
        DbActionResult SaveEmployee(EmployeeUpdateDto employeeupdatedto);
        DbActionResult UpdateEmployee(EmployeeUpdateDto employee, int? id, bool? status);
        Employee UpdatePatch(int id, JsonPatchDocument<Employee> name);
    }
    public class EmployeeServices : IEmployeeService
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        public EmployeeServices(OfisProjesiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public DbActionResult DeleteEmployeById(int? id)
        {
            Employee employee = _context.Employees.FirstOrDefault(p => p.Id == id);
            if (employee != null)
            {
                Debit debits = _context.Debits.Where(p => p.EmployeeId == employee.Id).FirstOrDefault();
                if (debits != null)
                {
                    return DbActionResult.HaveDebitError;
                }
                else
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                    return DbActionResult.Successful;
                }
            }
            else
            {
                return DbActionResult.UnknownError;
            }
        }
        public EmployeeDto GetEmployeeById(int? id)
        {
            if (id==null)
            {
                return null;
            }
            Employee employee = _context.Employees.Where(p => p.Id == id).FirstOrDefault();
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            if (employeeDto == null)
            {
                return null;
            }
            else
            {
                Debit[] debit = _context.Debits.Where(p => p.EmployeeId == employeeDto.Id).ToArray();
                DebitDto[] debitDto = _mapper.Map<DebitDto[]>(debit);
                return (employeeDto);
            }
        }
       public EmployeeDto[] GetEmployeeByName(string name, bool? durum)
        {
            Employee[] employee = _context.Employees.ToArray();
            if (name == null && durum == null)
            {
                return null;
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                employee = employee.Where(p => p.Name.Contains(name)).ToArray();
            }
            if (durum.HasValue)
            {
                employee = employee.Where(p => p.Status == durum).ToArray();
            }
            if (employee.ToList().Count > 0)
            {
                EmployeeDto[] employeeDtos = _mapper.Map<EmployeeDto[]>(employee);
                foreach (EmployeeDto EmployeeList in employeeDtos)
                {
                    Debit[] debitList = _context.Debits.Where(p => p.EmployeeId == EmployeeList.Id).ToArray();
                    Debit last = debitList.Last();
                    DebitDto debitDto = _mapper.Map<DebitDto>(last);
                        if (last.FinishDate==null)
                        {
                            EmployeeList.Debit = debitDto;
                            break;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return employeeDtos;
                }
                
            else
            {
                return null;
            }
        }
        public DbActionResult SaveEmployee(EmployeeUpdateDto employeeupdatedto)
        {
            if (employeeupdatedto == null || employeeupdatedto.OfficeId == null)
            {
                return DbActionResult.UnknownError;
            }
            Office office = _context.Offices.Where(p => p.Id == employeeupdatedto.OfficeId).SingleOrDefault();
            Employee Employee = new Employee();
            Employee.Name = employeeupdatedto.Name;
            Employee.OfficeId = employeeupdatedto.OfficeId;
            Employee.Lastname = employeeupdatedto.Lastname;
            Employee.Birthday = employeeupdatedto.birthday;
            Employee.RecordDate = DateTime.Now;
            Employee.UpdateDate = DateTime.Now;
            Employee.Status = true;
            if (String.IsNullOrWhiteSpace(employeeupdatedto.Name) || String.IsNullOrWhiteSpace(employeeupdatedto.Lastname))
            {
                return DbActionResult.NameOrLastNameError;
            }
            else if (office == null)
            {
                return DbActionResult.OfficeNotFound;
            }
            else
            {
                _context.Employees.Add(Employee);
                _context.SaveChanges();
                EmployeeUpdateDto dto = _mapper.Map<EmployeeUpdateDto>(employeeupdatedto);
                return DbActionResult.Successful;
            }
        }
        public DbActionResult UpdateEmployee(EmployeeUpdateDto employeeupdatedto, int? id, bool? Status)
        {
            Employee Employee = _context.Employees.Where(p => p.Id == id).SingleOrDefault();
            if (Employee==null){
                return DbActionResult.UnknownError;
            }
            List<Debit> employee1 = _context.Debits.Where(p => p.EmployeeId == id).ToList();
            Office office = _context.Offices.Where(p => p.Id == employeeupdatedto.OfficeId).SingleOrDefault();
            if (office == null)
            {
                return DbActionResult.OfficeNotFound;
            }
            else
            {
                Employee.Name = employeeupdatedto.Name;
                Employee.Lastname = employeeupdatedto.Lastname;
                Employee.OfficeId = employeeupdatedto.OfficeId;
                Employee.Birthday = employeeupdatedto.birthday;
                Employee.UpdateDate = DateTime.Now;
                if (Employee.Debits.Count <= 0)
                {
                    Employee.Status = Status;
                    _context.SaveChanges();
                    EmployeeUpdateDto dto = _mapper.Map<EmployeeUpdateDto>(employeeupdatedto);
                    return (DbActionResult.Successful);
                }
                else
                {
                    return DbActionResult.HaveDebitError;
                }

            }
        }
        public Employee UpdatePatch(int id, JsonPatchDocument<Employee> name)
        {
            Employee employee = _context.Employees.FirstOrDefault(e => e.Id == id);
                name.ApplyTo(employee);
                _context.SaveChanges();
            return (employee);
        }
    }
}









