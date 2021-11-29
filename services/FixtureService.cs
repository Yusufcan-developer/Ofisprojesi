using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using ofisprojesi;

namespace Ofisprojesi
{
    public interface IFixtureServices
    {
        FixtureDto[] GetFixtureByName(string name, bool? status);
        FixtureDto GetFixtureById(int id);
        Fixture DeleteFixtureById(int id);
        Fixture SaveFixture(FixtureDto fixture);
        FixtureDto UpdateFixture(FixtureDto fixturee, int id);
        Fixture UpdatePatch(int id, JsonPatchDocument<Fixture> name);
    }
    
    public class FixtureService : IFixtureServices
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        private readonly AppSettings _appsettings;
        public FixtureService(OfisProjesiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Fixture DeleteFixtureById(int id)
        {
            Fixture fixture = _context.Fixtures.FirstOrDefault(p => p.Id == id);
            if (fixture == null) return null;

            Debit debit = _context.Debits.Where(p => p.FixtureId == id).FirstOrDefault();
            if (debit != null) return null;
              
            _context.Fixtures.Remove(fixture);
            _context.SaveChanges();
            
            return fixture;       
        }

        public FixtureDto GetFixtureById(int id)
        {
            Fixture fixture = _context.Fixtures.Where(p => p.Id == id).FirstOrDefault();

            if (fixture == null) return null;

            FixtureDto dto = _mapper.Map<FixtureDto>(fixture);
            return dto;
        }

        public FixtureDto[] GetFixtureByName(string name, bool? status)
        {            
            List<Fixture> fixture = _context.Fixtures.ToList();

            if(!string.IsNullOrWhiteSpace(name)) fixture = fixture.Where(p => p.Name.Contains(name)).ToList();

            if (status.HasValue) fixture = fixture.Where(p => p.Status == status).ToList();
            
            //
            if (fixture.ToList().Count > 0) return _mapper.Map<FixtureDto[]>(fixture);

            return null;
        }
        public Fixture SaveFixture(FixtureDto fixture)
        {
            Office office = _context.Offices.Where(p => p.Id == fixture.Officeid).SingleOrDefault();
            Fixture Fixture = new Fixture();
            Fixture.Name = fixture.Name;
            Fixture.OfficeId = fixture.Officeid;
            Fixture.Status = fixture.Status;


            if (fixture.Status == false || fixture.Name == null || fixture.Id != null || (fixture.Officeid == null || fixture.Officeid < 0 || office == null))
                return null;


            _context.Fixtures.Add(Fixture);
            _context.SaveChanges();
            FixtureDto dto = _mapper.Map<FixtureDto>(Fixture);
            return Fixture;
        }


        public FixtureDto UpdateFixture(FixtureDto fixturee, int id)
        {
            Fixture Fixture = _context.Fixtures.Where(p => p.Id == id).FirstOrDefault();
            Office office = _context.Offices.Where(p => p.Id == fixturee.Officeid).FirstOrDefault();

            if ((fixturee.Status == false && office == null) || (fixturee.Status == true && office == null) || fixturee.Id != null)
                return null;
            
            Fixture.Name = fixturee.Name;
            Fixture.Status = fixturee.Status;
            Fixture.OfficeId = fixturee.Officeid;
            _context.SaveChanges();
            FixtureDto dto = _mapper.Map<FixtureDto>(fixturee);

            return dto;   
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