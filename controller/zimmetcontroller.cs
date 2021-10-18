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

        public IList<Zimmet> AllGetEmbezzled()

        {

            var Querry = (from item in _Zcontext.Zimmets
            join item2 in _Zcontext.Calisans on item.CalisanId equals item2.Id
            join item3 in _Zcontext.Demirbas
            on item.DemirbasId equals item3.Id select new Zimmet(){
                Id=item.Id,
                CalisanAd=item2.Ad,
                CalisanId=item2.Id,
                DemirbasAd=item3.Ad,
                DemirbasId=item3.Id,
                Tarih=item.Tarih,
                Durum=item.Durum
                
            }).ToList();
             Querry.ToList();
             return Querry;

        }

        [HttpDelete]//id ye göre zimmet verisi sil

        public void DeleteEmbezzledById([FromQuery] int id)

        {

            var Deleted = _Zcontext.Zimmets.SingleOrDefault(p => p.Id == id);
            _Zcontext.Zimmets.Remove(Deleted);
            _Zcontext.SaveChanges();
            return;

        }

        [HttpPut]//zimmet verisi güncelle

        public void AllEmbezzledUpdate(int id, [FromQuery] int calisan, int demirbas, DateTime tarih, Boolean Durum)
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
        public Zimmet GetEmbezzledById(int Id)
        {
            return _Zcontext.Zimmets.Where(p => p.Id == Id).FirstOrDefault();




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









