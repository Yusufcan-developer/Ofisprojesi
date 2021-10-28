


using AutoMapper;
using Ofisprojesi;

namespace ofisprojesi 
    {  
        public class AutoMapperProfile : Profile  
        {  
            public AutoMapperProfile()  
            {  
                CreateMap<Employee, EmployeeDto>(); 
                CreateMap<Fixture,FixtureDto>();
                CreateMap<Debit,DebitDto>();
                CreateMap<Office,OfficeDto>();

            }  
            
        }  
    }  
