using AutoMapper;
using Dapper_StoredProcedures.Application.DTOs.Requests;
using Dapper_StoredProcedures.Domain.Entities;
namespace Dapper_StoredProcedures.Application.Profiles
{

    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<UpdateCustomerRequest, Customer>();
            CreateMap<CreateCustomerRequest, Customer>();
        }
    }

}
