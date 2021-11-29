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
namespace Ofisprojesi
{
    public interface IOfficeServices
    {
        OfficeDto GetOfficeById(int id);
        OfficeDto[] GetOfficByName(string name, bool? status);
        Office DeleteOfficeById(int id);
        OfficeUpdateDto SaveOffice(OfficeUpdateDto offices);
        OfficeUpdateDto UpdateDto(OfficeUpdateDto office, int id);
        OfficeDto UpdatePatch(int id, JsonPatchDocument<Office> name);
    }
    public class OfficeService : IOfficeServices
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        public OfficeService(OfisProjesiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Office DeleteOfficeById(int id)
        {
            Office office = _context.Offices.SingleOrDefault(p => p.Id == id);
            if (office == null)
            {
                return null;
            }
            else
            {
                Employee employee = _context.Employees.FirstOrDefault(x => x.OfficeId == id);
                Fixture fixture = _context.Fixtures.FirstOrDefault(p => p.OfficeId == id);

                if (office == null)
                {
                    return null;
                }
                else if ((employee != null) || (fixture != null))
                {
                    return null;
                }
                else
                {
                    _context.Offices.Remove(office);
                    _context.SaveChanges();
                    return office;
                }
            }
        }
        public OfficeDto[] GetOfficByName(string name, bool? status)
        {
            if (name == null && status == null)
            {
                return null;
            }
            Office[] Office = _context.Offices.ToArray();
            if (Office == null)
            {
                return null;
            }
            else
            {
                foreach (Office officeList in Office)
                {
                    Fixture[] fixture = _context.Fixtures.Where(p => p.OfficeId == officeList.Id).ToArray();
                    FixtureDto[] fixtureDto = _mapper.Map<FixtureDto[]>(fixture);
                    officeList.Fixtures = fixture;
                }
                if (!string.IsNullOrWhiteSpace(name))
                {
                    Office = Office.Where(p => p.Name.Contains(name)).ToArray();
                }
                if (status.HasValue)
                {
                    Office = Office.Where(p => p.Status == status).ToArray();
                }
                if (Office.ToList().Count > 0)
                {
                    OfficeDto[] dto = _mapper.Map<OfficeDto[]>(Office);
                    return (dto);
                }
                else
                {
                    return null;
                }
            }
        }
        public OfficeDto GetOfficeById(int id)
        {
            Office office = _context.Offices.Where(p => p.Id == id).FirstOrDefault();
            if (office == null)
            {
                return null;
            }
            else
            {
                OfficeDto officedto = _mapper.Map<OfficeDto>(office);
                Fixture[] fixture = _context.Fixtures.Where(p => p.OfficeId == officedto.Id).ToArray();
                FixtureDto[] fixturedto = _mapper.Map<FixtureDto[]>(fixture);
                Employee[] employee = _context.Employees.Where(p => p.OfficeId == officedto.Id).ToArray();
                EmployeeDto[] employeeDto = _mapper.Map<EmployeeDto[]>(employee);
                officedto.employee = employeeDto;
                officedto.Fixtures = fixturedto;
                if (office == null)
                {
                    return null;
                }
                else
                {
                    return (officedto);
                }
            }
        }
        public OfficeUpdateDto SaveOffice(OfficeUpdateDto offices)
        {
            Office Office = new Office();
            Office.Name = offices.Name;
            Office.Status = offices.Status;
            if (offices.Name == null || !offices.Status.HasValue)
            {
                return null;
            }
            else
            {
                _context.Offices.Add(Office);
                _context.SaveChanges();
                OfficeUpdateDto dto = _mapper.Map<OfficeUpdateDto>(Office);
                return (dto);
            }
        }
        public OfficeUpdateDto UpdateDto(OfficeUpdateDto office, int id)
        {
            Office Office = _context.Offices.FirstOrDefault(p => p.Id == id);
            if (Office == null || office.Name == null)
            {
                return null;
            }
            else
            {
                Office.Name = office.Name;
                Office.Status = office.Status;
                _context.SaveChanges();
                OfficeUpdateDto dto = _mapper.Map<OfficeUpdateDto>(Office);
                return (dto);
            }
        }
        public OfficeDto UpdatePatch(int id, JsonPatchDocument<Office> name)
        {
            Office office = _context.Offices.FirstOrDefault(e => e.Id == id);
            OfficeDto officedto = _mapper.Map<OfficeDto>(office);
            if (office == null || name == null)
            {
                return null;
            }
            else
            {
                name.ApplyTo(office);
                _context.SaveChanges();
                return (officedto);
            }
        }
    }
}