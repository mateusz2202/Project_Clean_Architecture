using AutoMapper;
using Operation.Application.Features.Operation.Commands.AddOperation;
using Operation.Application.Features.Operation.Queries.GetAll.Operations;
using Operation.Application.Features.Operation.Queries.GetById;

namespace Operation.Application.Common.Mappings;

public class OperationProfile : Profile
{
    public OperationProfile()
    {
        CreateMap<Domain.Entities.Operation, AddOperationCommand>().ReverseMap();    
        CreateMap<Domain.Entities.Operation, GetOperationResponse>().ReverseMap();    
        CreateMap<Domain.Entities.Operation, GetAllOperationsResponse>().ReverseMap();   
       
    }
   
}
