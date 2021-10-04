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
       public int Demirbasid { get;set;}
       public int DemirbasZimmet { get;set;}
       public int DemirbasAdi { get;set;}
        
    }
}