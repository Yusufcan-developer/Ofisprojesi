using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ofisprojesi
{
    [Route("api/zimmetler")]
    [ApiController]
    public class zimmetcontroller : ControllerBase
    {
        private OfisProjesiContext _Zcontext;
        public zimmetcontroller(OfisProjesiContext context)
        {

            _Zcontext = context;
        }
        [HttpGet]//tüm zimmet verilerini getir 

        public IList<Zimmet> Get([FromQuery] int id)

        {

            var GetAllData = _Zcontext.Zimmets.ToList();

            {

                return GetAllData;

            }

        }

        [HttpDelete]//id ye göre zimmet verisi sil

        public void Delete([FromQuery] int id)

        {

            var Deleted = _Zcontext.Zimmets.SingleOrDefault(p => p.Id == id);
            _Zcontext.Zimmets.Remove(Deleted);
            _Zcontext.SaveChanges();
            return;

        }

        [HttpPut]//zimmet verisi güncelle

        public void Put(int id,[FromQuery] int calisan,int demirbas,DateTime tarih,Boolean Durum)
        {

            var update = _Zcontext.Zimmets.FirstOrDefault(p => p.Id == id);
            update.CalisanId = calisan;
            update.DemirbasId = demirbas;
            update.Tarih = tarih;
            update.Durum = Durum;
            _Zcontext.SaveChanges();
            return;

        }


        [HttpGet("{Id:int}")]//id ye göre zimmet verisi getir 

        public List<Zimmet> id(int Id)
        {

            var ZimmetIdSorgusu = _Zcontext.Zimmets.Where(p => p.Id == Id).ToList();
            return ZimmetIdSorgusu;

        }



        [HttpPost]//zimmet verisi kaydet

        public void Post(int calisan, int demirbas, Boolean Durum)
        {

            Zimmet zimmet = new Zimmet();
            zimmet.CalisanId = calisan;
            zimmet.DemirbasId = demirbas;
            zimmet.Durum = Durum;
            zimmet.Tarih = DateTime.Now;

            _Zcontext.Zimmets.Add(zimmet);
            _Zcontext.SaveChanges();

        }


    }
}









