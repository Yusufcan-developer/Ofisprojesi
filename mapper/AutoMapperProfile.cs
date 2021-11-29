


using AutoMapper;
using Ofisprojesi;

namespace ofisprojesi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Fixture, FixtureDto>();
            CreateMap<Debit, DebitDto>();
            CreateMap<Office, OfficeDto>();
            CreateMap<Employee, EmployeeUpdateDto>();
            CreateMap<Office, OfficeUpdateDto>();
            CreateMap<User,UserDto>();
            CreateMap<Role,RoleDto>();
            CreateMap<Debit, DebitSaveDto>();

        }

    }
}
