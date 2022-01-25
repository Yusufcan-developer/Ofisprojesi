
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ofisprojesi;
namespace Ofisprojesi
{
    public interface IOfficeServices
    {
        OfficeDto GetOfficeById(int id);
        OfficeDto[] GetOfficByName(string name, bool? status);
        DbActionResult DeleteOfficeById(int? id);
        DbActionResult SaveOffice(OfficeUpdateDto officeUpdateDto);
        DbActionResult UpdateDto(OfficeUpdateDto officeUpdateDto, int? id, bool? durum);
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
        public DbActionResult DeleteOfficeById(int? id)
        {
            if (id == null)
            {
                return DbActionResult.UnknownError;
            }
            Office office = _context.Offices.SingleOrDefault(p => p.Id == id);
            if (office == null)
            {
                return DbActionResult.OfficeNotFound;
            }
            Employee employee = _context.Employees.FirstOrDefault(x => x.OfficeId == id);
            Fixture fixture = _context.Fixtures.FirstOrDefault(p => p.OfficeId == id);
            if ((employee != null) || (fixture != null))
            {
                return DbActionResult.HaveDebitError;
            }
            else
            {
                _context.Offices.Remove(office);
                _context.SaveChanges();
                return DbActionResult.Successful;
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
                foreach (Office OfficeList in Office)
                {
                    Fixture[] fixture = _context.Fixtures.Where(p => p.OfficeId == OfficeList.Id).ToArray();
                    FixtureDto[] fixtureDto = _mapper.Map<FixtureDto[]>(fixture);
                    OfficeList.Fixtures = fixture;
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
                FixtureDto[] employeeDto = _mapper.Map<FixtureDto[]>(employee);
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
        public DbActionResult SaveOffice(OfficeUpdateDto officeupdatedto)
        {
            Office Office = new Office();
            Office.Name = officeupdatedto.Name;
            Office.Status = true;
            Office.Recdate = System.DateTime.Now;
            Office.Updatedate = System.DateTime.Now;
            if (officeupdatedto.Name == null)
            {
                return DbActionResult.OfficeNotFound;
            }
            else
            {
                _context.Offices.Add(Office);
                _context.SaveChanges();
                OfficeUpdateDto dto = _mapper.Map<OfficeUpdateDto>(Office);
                return (DbActionResult.Successful);
            }
        }
        public DbActionResult UpdateDto(OfficeUpdateDto officeupdatedto, int? id, bool? status)
        {
            if (officeupdatedto == null || id == null)
            {
                return DbActionResult.UnknownError;
            }
            Office Office = _context.Offices.FirstOrDefault(p => p.Id == id);
            if (Office == null)
            {
                return DbActionResult.OfficeNotFound;
            }

            Office.Name = officeupdatedto.Name;
            Office.Status = status;
            Office.Recdate = Office.Recdate;
            Office.Updatedate = System.DateTime.Now;

            Fixture fixture = _context.Fixtures.Where(p => p.OfficeId == id).FirstOrDefault();
            if (fixture != null && status == false)
            {
                return DbActionResult.HaveDebitError;
            }
            Employee employee = _context.Employees.Where(p => p.OfficeId == id).FirstOrDefault();
            if (employee != null && status == false)
            {
                return DbActionResult.HaveDebitError;
            }
            _context.SaveChanges();
            OfficeUpdateDto dto = _mapper.Map<OfficeUpdateDto>(Office);
            return (DbActionResult.Successful);
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