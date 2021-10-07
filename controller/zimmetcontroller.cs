using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ofisprojesi.Models;

namespace ofisprojesi
{ 


    [Route("api/zimmet")]
    [ApiController]
    public class zimmetcontroller : ControllerBase
    {
        private OfisProjesiContext _context;
        public zimmetcontroller(OfisProjesiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IList<Zimmet> GetOfis([FromQuery]int name2){

        
        

        var kosul= _context.Zimmets.ToList();
        
        {
            return kosul;
        }

    }
    }
}
     




    


