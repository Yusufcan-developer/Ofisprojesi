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
using Ofisprojesi;
using System.Collections;
using ofisprojesi;

namespace ofisprojesi
{

    public interface ICoreService
    {
        
        EmployeeDto GetEmployeeById(int id);
    }

    public class IEmployeeServices : ICoreService
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        public IEmployeeServices(OfisProjesiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public EmployeeDto GetEmployeeById(int id)
        {
            Employee employee = _context.Employees.Where(p => p.Id == id).FirstOrDefault();
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            Debit[] debit = _context.Debits.Where(p => p.EmployeeId == employeeDto.Id).ToArray();
            DebitDto[] debit1 = _mapper.Map<DebitDto[]>(debit);
            employeeDto.Debit = debit1;


            return(employeeDto);
        }
        }
    }









