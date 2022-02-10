using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ofisprojesi;

namespace Ofisprojesi
{
    public interface IFixtureServices
    {
        List<FixtureDto> GetFixtureByName(string name, fixtureservicesenum? status);
        FixtureDto GetFixtureById(int id);
        DbActionResult DeleteFixtureById(int? id);
        DbActionResult SaveFixture(FixtureUpdateDto fixtureupdatedto);
        DbActionResult UpdateFixture(FixtureUpdateDto fixtureupdatedto, int? id, bool? status);
        Fixture UpdatePatch(int id, JsonPatchDocument<Fixture> name);
    }

    public class FixtureService : IFixtureServices
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        public FixtureService(OfisProjesiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public DbActionResult DeleteFixtureById(int? id)
        {
            Fixture fixture = _context.Fixtures.FirstOrDefault(p => p.Id == id);
            if (fixture == null) return DbActionResult.UnknownError;

            Debit debit = _context.Debits.Where(p => p.FixtureId == id).FirstOrDefault();
            if (debit != null) return DbActionResult.HaveDebitError;

            _context.Fixtures.Remove(fixture);
            _context.SaveChanges();

            return DbActionResult.Successful;
        }

        public FixtureDto GetFixtureById(int id)
        {
            Fixture fixture = _context.Fixtures.Where(p => p.Id == id).FirstOrDefault();

            if (fixture == null) return null;

            FixtureDto dto = _mapper.Map<FixtureDto>(fixture);
            return dto;
        }

        public List<FixtureDto> GetFixtureByName(string name, fixtureservicesenum? status)
        {
            List<Fixture> fixture = _context.Fixtures.ToList();
            List<FixtureDto> dto = _mapper.Map<List<FixtureDto>>(fixture);
            
            if (name == null && status == null)
            {
                return null;
            }
            if (!string.IsNullOrWhiteSpace(name))
            {

                fixture = fixture.Where(p => p.Name.Contains(name)).ToList();
                List<FixtureDto> NameSearch = _mapper.Map<List<FixtureDto>>(fixture);
                return NameSearch;
                
            }
            if (status == fixtureservicesenum.FixtureAll)
            {
                return dto;
            }
            if(status==fixtureservicesenum.FixtureActive)
            {
                List<Fixture> fixtures = fixture.Where(p=>p.Status==true).ToList();
                List<FixtureDto> DtoActive = _mapper.Map<List<FixtureDto>>(fixtures);
                return DtoActive;
            }
            if(status==fixtureservicesenum.FixturePasive){
                List<Fixture> fixtures= fixture.Where(p=>p.Status==false).ToList();
                List<FixtureDto> DtoPasive = _mapper.Map<List<FixtureDto>>(fixtures);
                return DtoPasive;
            }
            
            return null;
        }
        public DbActionResult SaveFixture(FixtureUpdateDto fixtureupdatedto)
        {
            if (fixtureupdatedto == null)
            {
                return DbActionResult.UnknownError;
            }
            Office office = _context.Offices.Where(p => p.Id == fixtureupdatedto.Officeid).SingleOrDefault();
            Fixture Fixture = new Fixture();
            Fixture.Name = fixtureupdatedto.name;
            Fixture.OfficeId = fixtureupdatedto.Officeid;
            Fixture.Status = true;
            Fixture.Recdate = System.DateTime.Now;
            Fixture.Updatedate = System.DateTime.Now;
            if (fixtureupdatedto.name == null)
            {
                return DbActionResult.NameOrLastNameError;
            }
            if (fixtureupdatedto.Officeid == null || fixtureupdatedto.Officeid < 0 || office == null)
            {
                return DbActionResult.OfficeNotFound;
            }
            _context.Fixtures.Add(Fixture);
            _context.SaveChanges();
            FixtureDto dto = _mapper.Map<FixtureDto>(Fixture);
            return DbActionResult.Successful;
        }


        public DbActionResult UpdateFixture(FixtureUpdateDto fixtureupdatedto, int? id, bool? status)
        {
            if (fixtureupdatedto.name == null)
            {
                return DbActionResult.NameOrLastNameError;
            }
            if (fixtureupdatedto == null)
            {
                return DbActionResult.UnknownError;
            }
            Fixture Fixture = _context.Fixtures.Where(p => p.Id == id).SingleOrDefault();
            if (Fixture == null)
                return DbActionResult.UnknownError;
            Office office = _context.Offices.Where(p => p.Id == fixtureupdatedto.Officeid).SingleOrDefault();
            if (office == null)
                return DbActionResult.OfficeNotFound;
            Fixture.Name = fixtureupdatedto.name;
            Fixture.Status = status;
            Fixture.OfficeId = fixtureupdatedto.Officeid;
            Fixture.Recdate = Fixture.Recdate;
            Fixture.Updatedate = System.DateTime.Now;
            _context.SaveChanges();
            FixtureUpdateDto dto = _mapper.Map<FixtureUpdateDto>(fixtureupdatedto);

            return DbActionResult.Successful;
        }

        public Fixture UpdatePatch(int id, JsonPatchDocument<Fixture> name)
        {
            Fixture fixture = _context.Fixtures.FirstOrDefault(e => e.Id == id);

            name.ApplyTo(fixture);
            _context.SaveChanges();

            return fixture;
        }
    }
}