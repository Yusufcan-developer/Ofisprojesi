using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ofisprojesi;
using Ofisprojesicontroller;
namespace Ofisprojesicontroller
{
    [Route("api/fixtures")]
    [ApiController]
    public class UserController : Controller
{
    private readonly IMapper _mapper;
    private readonly OfisProjesiContext _context;
    public UserController(IMapper mapper,OfisProjesiContext context)
    {
        _mapper = mapper;
        _context =  context;
    }



[HttpGet]
public ActionResult<Employeedto> GetMovies()
{ 
    Employee employeed=_context.Employees.FirstOrDefault();
    Employeedto dto = _mapper.Map<Employeedto>(employeed);

    return Ok(dto);
}
}
}