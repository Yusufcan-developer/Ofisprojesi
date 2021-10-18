using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ofisprojesi.controller

    {[Route("api/hareket")]
    [ApiController]
    public class hareketcontrol : ControllerBase
    {
        private OfisProjesiContext _context;
        public hareketcontrol(OfisProjesiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Zimmet> GetEmbezzledById ()
        {

            var Querry = (from item in _context.Zimmets
            join item2 in _context.Calisans
            on item.CalisanId equals item2.Id select new Zimmet(){
                CalisanAd=item2.Ad,
                DemirbasAd=item.DemirbasAd,
                Tarih=item.Tarih,
                Durum=item.Durum
            }).ToList();
             Querry.ToList();
             return Querry;
    }
}
    }