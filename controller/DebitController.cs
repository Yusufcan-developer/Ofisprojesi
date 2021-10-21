using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ofisprojesi
{
    [Route("api/debits")]
    [ApiController]
    public class DebitController : ControllerBase
    {
        private OfisProjesiContext _context;
        public DebitController(OfisProjesiContext context)
        {

            _context = context;
        }
        /// <summary>
        /// "Tüm Zimmet Verilerini Getir"
        /// </summary>
        /// <returns></returns>
        [Route("GetAllDebit")]
        [HttpGet]//tüm zimmet verilerini getir 

        public IList<Debit> GetAllDebit()

        {

            var Querry = (from item in _context.Debits
                          join item2 in _context.Employees on item.EmployeeId equals item2.Id
                          join item3 in _context.Fixtures
                          on item.FixtureId equals item3.Id
                          select new Debit()
                          {
                              Id = item.Id,
                              EmployeeId = item2.Id,
                              FixtureId = item3.Id,
                              Date = item.Date,
                              Status = item.Status
                          }
            ).ToList();
            Querry.ToList();
            return Querry;

        }
        /// <summary>
        /// "id ye Göre Zimmet Verisi sil"
        /// </summary>
        /// <returns></returns>
        [Route("DeleteDebitById")]
        [HttpDelete]//id ye göre zimmet verisi sil

        public void DeleteDebitById([FromQuery] int id)

        {

            var Deleted = _context.Debits.Find(id);
            _context.Debits.Remove(Deleted);
            _context.SaveChanges();
            return;

        }
        /// <summary>
        /// "Zimmet Verisini Güncelle"
        /// </summary>
        /// <returns></returns>
        [Route("UpdateAllDebit")]
        [HttpPut]//zimmet verisi güncelle

        public void UpdateAllDebit(int id, [FromQuery] Debit debit)
        {

            var update = _context.Debits.FirstOrDefault(p => p.Id == id);
            update.EmployeeId = debit.EmployeeId;
            update.FixtureId = debit.FixtureId;
            update.Date = debit.Date;
            update.Status = debit.Status;
            _context.SaveChanges();


            return;

        }
        /// <summary>
        /// "id ye Göre Zimmet Verisi Getir"
        /// </summary>
        /// <returns></returns>
        [HttpGet("{Id:int}")]//id ye göre zimmet verisi getir 
        public Debit GetDebitById(int Id)
        {
            return _context.Debits.Where(p => p.Id == Id).FirstOrDefault();




        }
        /// <summary>
        /// "Zimmet Verisi"
        /// </summary>
        /// <returns></returns>
        [Route("SaveDebitById")]
        [HttpPost]//zimmet verisi kaydet

        public void SaveDebitById([FromBody] Debit debit)
        {

            _context.Debits.Add(debit);
            _context.SaveChanges();


        }


    }
}









