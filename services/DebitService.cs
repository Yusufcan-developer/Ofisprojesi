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
        DebitDto[] GetAllDebit(debitservicesenum status, DebitSearchDto debitSearch);
        DebitDto GetDebitById(int? id);
        DbActionResult DeleteDebitById(int? id);
        Debit UpdatePatch(int id, JsonPatchDocument<Debit> name);
        DbActionResult UpdateAllDebit(int? id);
        DbActionResult SaveDebitById(DebitSaveDto debitSaveDto);
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
        public DbActionResult DeleteDebitById(int? id)
        {
            Debit deleted = _context.Debits.Where(p => p.Id == id).FirstOrDefault();
            if (deleted == null)
            {
                return DbActionResult.NotFound;
            }
            _context.Debits.Remove(deleted);
            _context.SaveChanges();
            return DbActionResult.Successful;
        }
        public DebitDto[] GetAllDebit(debitservicesenum status, DebitSearchDto debitSearch)
        {
            Debit[] controller = _context.Debits.Include("Employee").Include("Fixture").ToArray();
            DebitDto[] debits = _mapper.Map<DebitDto[]>(controller);
            if (controller != null)
            {
                //Tüm kayıtlar
                if (status == debitservicesenum.DebitAll)
                {
                    return debits;
                }
                else
                {
                    foreach (DebitDto list in debits)
                    {
                        //Pasif kayıtlar
                        if (status == debitservicesenum.DebitPasive)
                        {
                            if (list.Finish_Date != null)
                            {
                                DebitDto[] debits1 = debits.Where(p => p.Finish_Date != null).ToArray();
                                DebitDto[] debitslist1 = debits1.Where(p => p.Created_Date >= debitSearch.Start_Date).ToArray();
                                DebitDto[] debitslist2 = debitslist1.Where(p => p.Finish_Date <= debitSearch.End_Date).ToArray();
                                debits = _mapper.Map<DebitDto[]>(debits1);
                            }
                        }
                        //Aktif kayıtlar
                        else if (status == debitservicesenum.DebitActive)
                        {
                            if (list.Finish_Date == null)
                            {
                                DebitDto[] debits1 = debits.Where(p => p.Finish_Date == null).ToArray();
                                debits = _mapper.Map<DebitDto[]>(debits1);
                            }
                        }
                    }
                }

            }
            return debits;
        }
        public DebitDto GetDebitById(int? id)
        {
            Debit debit = _context.Debits.Where(p => p.Id == id).FirstOrDefault();
            if (debit == null)
            {
                return null;
            }
            Fixture fixture = _context.Fixtures.Where(p => p.Id == debit.FixtureId).SingleOrDefault();
            Employee employee = _context.Employees.Where(p => p.Id == debit.EmployeeId).SingleOrDefault();
            {
                DebitDto dto = _mapper.Map<DebitDto>(debit);
                dto.FixtureName = fixture.Name;
                dto.EmployeeName = employee.Name;
                dto.EmployeeLastname = employee.Lastname;
                return (dto);
            }
        }
        public DbActionResult SaveDebitById(DebitSaveDto debitSaveDto)
        {
            {
                Debit debit = new Debit();
                debit.EmployeeId = debitSaveDto.EmployeeId;
                debit.FixtureId = debitSaveDto.FixtureId;
                debit.CreatedDate = DateTime.Now;
                debit.FinishDate = null;
                Fixture fixtures = _context.Fixtures.Where(p => p.Id == debitSaveDto.FixtureId).SingleOrDefault();

                if (fixtures == null)
                {
                    return DbActionResult.NotHaveFixture;
                }
                Employee employees = _context.Employees.Where(p => p.Id == debitSaveDto.EmployeeId).SingleOrDefault();
                if (employees == null)
                {
                    return DbActionResult.NotHaveEmployee;
                }
                else
                {
                    _context.Debits.Add(debit);
                    DebitDto debit3 = _mapper.Map<DebitDto>(debit);
                    _context.SaveChanges();
                    return (DbActionResult.Successful);
                }
            }
        }

        public DbActionResult UpdateAllDebit(int? id)
        {

            Debit update = _context.Debits.Where(p => p.Id == id).SingleOrDefault();
            if (update == null)
            {
                return DbActionResult.UnknownError;
            }
            else
            {

                update.EmployeeId = update.EmployeeId;
                update.FixtureId = update.FixtureId;
                update.CreatedDate = DateTime.Now;
                update.FinishDate = DateTime.Now;
                DebitDto dto = _mapper.Map<DebitDto>(update);
                _context.SaveChanges();
                return (DbActionResult.Successful);
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