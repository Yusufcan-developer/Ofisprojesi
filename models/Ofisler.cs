using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace ofisprojesi
{
   [Table("OfisProjesi")]
    public class User
    {
        
        [Key]
        public int Ofisid { get;set;}
       
        
    }
}