


using AutoMapper;

namespace ofisprojesi 
    {  
        public class AutoMapperProfile : Profile  
        {  
            public AutoMapperProfile()  
            {  
               
                CreateMap<Employee, Employeedto>(); 

            }  
            
        }  
    }  
