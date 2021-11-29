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

namespace ofisprojesi
{
    public interface IEmployeeService
    {
        EmployeeDto[] GetEmployeeByName(string name, bool? durum);
        EmployeeDto GetEmployeeById(int id);
        Employee DeleteEmployeById(int id);
        Employee SaveEmployee(EmployeeUpdateDto employee);
        EmployeeUpdateDto UpdateEmployee(EmployeeUpdateDto employee, int id,bool durum);
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
        public Employee DeleteEmployeById(int id)
        {
            Employee employee = _context.Employees.FirstOrDefault(p => p.Id == id);
            if (employee != null)
            {
                Debit debits = _context.Debits.Where(p => p.EmployeeId == employee.Id).FirstOrDefault();
                if (debits != null)
                {
                    return null;
                }
                else
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                    return employee;
                }
            }
            else
            {
                return null;
            }
        }
        public EmployeeDto GetEmployeeById(int id)
        {
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
                employeeDto.Debit = debitDto;
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
                    Debit[] debits = _context.Debits.Where(p => p.EmployeeId == EmployeeList.Id).ToArray();
                    DebitDto[] debitDto = _mapper.Map<DebitDto[]>(debits);
                    EmployeeList.Debit = debitDto;
                }
                return employeeDtos;
            }
            else
            {
                return null;
            }
        }
        public Employee SaveEmployee(EmployeeUpdateDto employee)
        {
            Office office = _context.Offices.Where(p => p.Id == employee.OfficeId).SingleOrDefault();
            Employee Employee = new Employee();
            Employee.Name = employee.Name;
            Employee.OfficeId = employee.OfficeId;
            Employee.Lastname = employee.Lastname;
            Employee.Age = employee.Age;
            Employee.RecordDate = DateTime.Now;
            Employee.UpdateDate = DateTime.Now;
            Employee.Status = true;
            if (employee.Age > 150 || employee.Age < 5)
            {
                return null;
            }
            else if (String.IsNullOrWhiteSpace(employee.Name) || String.IsNullOrWhiteSpace(employee.Lastname))
            {
                return null;
            }
            else if (office == null)
            {
                return null;
            }
            else
            {
                _context.Employees.Add(Employee);
                _context.SaveChanges();
                EmployeeUpdateDto dto = _mapper.Map<EmployeeUpdateDto>(employee);
                return Employee;
            }
            return null;
        }
        public EmployeeUpdateDto UpdateEmployee(EmployeeUpdateDto employee, int id,bool durum)
        {

            Employee Employee = _context.Employees.Where(p => p.Id == id).FirstOrDefault();
            Office office = _context.Offices.Where(p => p.Id == employee.OfficeId).FirstOrDefault();
            if (office == null)
            {
                return null;
            }
            if (employee.Age == 0 || employee.Age == null)
            {
                return null;
            }
            if (Employee.Status == false && office.Id != employee.OfficeId)
            {
                return null;
            }
            else
            {
                Employee.Name = employee.Name;
                Employee.Lastname = employee.Lastname;
                Employee.Status = durum;
                Employee.OfficeId = employee.OfficeId;
                Employee.Age = employee.Age;
                Employee.RecordDate = Employee.RecordDate;
                Employee.UpdateDate = DateTime.Now;
                _context.SaveChanges();
                EmployeeUpdateDto dto = _mapper.Map<EmployeeUpdateDto>(employee);
                return (dto);
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









