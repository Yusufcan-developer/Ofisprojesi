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
    public interface IDebitServices
    {
        List<DebitDto> GetAllDebit(bool? status);
        DebitDto GetDebitById(int id);
        Debit DeleteDebitById(int id);
        Debit UpdatePatch(int id, JsonPatchDocument<Debit> name);
        DebitDto UpdateAllDebit(int id, DebitDto debit);
        DebitDto SaveDebitById(DebitSaveDto debit);
    }
    public class DebitService : IDebitServices
    {
        private OfisProjesiContext _context;
        private IMapper _mapper;
        public DebitService(OfisProjesiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Debit DeleteDebitById(int id)
        {
            Debit deleted = _context.Debits.Where(p => p.Id == id).FirstOrDefault();
            if (deleted == null)
            {
                return null;
            }
            else
            {
                _context.Debits.Remove(deleted);
                _context.SaveChanges();
                return deleted;
            }
        }
        public List<DebitDto> GetAllDebit(bool? status)
        {
            {
                List<Debit> controller = _context.Debits.Where(p => p.Status == status).ToList();
                if (status == null)
                {
                    return null;
                }
                if (status.HasValue)
                {

                    controller = controller.Where(p => p.Status == status).ToList();
                    List<DebitDto> debit = _mapper.Map<List<DebitDto>>(controller);
                    return debit;
                }
                else
                {
                    return null;
                }
            }
        }
        public DebitDto GetDebitById(int id)
        {
            Debit debit = _context.Debits.Where(p => p.Id == id).FirstOrDefault();
            if (debit == null)
            {
                return null;
            }
            else
            {
                DebitDto dto = _mapper.Map<DebitDto>(debit);
                return (dto);
            }
        }
        public DebitDto SaveDebitById(DebitSaveDto debit)
        {
            if (debit == null)
            {
                return null;
            }
            else
            {
                Debit debit1 = new Debit();
                debit1.EmployeeId = debit.EmployeeId;
                debit1.FixtureId = debit.FixtureId;
                debit1.Date = DateTime.Now;
                debit1.Status=true;
                Fixture fixtures = _context.Fixtures.Where(p => p.Id == debit.FixtureId).SingleOrDefault();
                
                if (fixtures == null)
                {
                    return null;
                }
                else
                {
                    Employee employees = _context.Employees.Where(p => p.Id == debit.EmployeeId).SingleOrDefault();
                    if (employees == null)
                    {
                        return null;
                    }
                    else
                    {
                        if ( employees.Status == false)
                        {
                            return null;
                        }
                        if (fixtures.Status == false)
                        {
                            return null;
                        }
                        else
                        {
                            

                            _context.Debits.Add(debit1);
                            _context.SaveChanges();
                            DebitDto debit3=_mapper.Map<DebitDto>(debit1);
                            return (debit3);
                        }
                    }
                }
            }
        }

        public DebitDto UpdateAllDebit(int id, DebitDto debit)
        {
            Employee employee = _context.Employees.Where(p => p.Id == debit.EmployeeId).FirstOrDefault();
            if (employee == null)
            {
                return null;
            }
            else
            {
                Fixture fixture = _context.Fixtures.Where(p => p.Id == debit.FixtureId).FirstOrDefault();
                if (fixture == null)
                { return (null); }
                else
                {
                    Debit update = _context.Debits.Where(p => p.Id == id).FirstOrDefault();
                    if (update == null)
                    {
                        return null;
                    }
                    else
                    {
                        update.EmployeeId = debit.EmployeeId;
                        update.Status = debit.status;
                        update.FixtureId = debit.FixtureId;
                        update.Date = DateTime.Now;
                        _context.SaveChanges();
                        DebitDto dto = _mapper.Map<DebitDto>(update);
                        return (dto);
                    }
                }
            }
        }
        public Debit UpdatePatch(int id, JsonPatchDocument<Debit> name)
        {
            Debit debit = _context.Debits.FirstOrDefault(e => e.Id == id);
            name.ApplyTo(debit);
            _context.SaveChanges();
            return (debit);
        }
    }
}