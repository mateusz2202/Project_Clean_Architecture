using MediatR;
using System.Data;
using AutoMapper;
using FluentValidation;
using Product.Application.Requests;
using Product.Shared.Wrapper;
using Product.Application.Interfaces.Repositories;
using Product.Application.Interfaces.Services;
using Product.Application.Application.Features.Brands.Commands.AddEdit;
using Product.Shared.Constans;
using Product.Domain.Entities;

namespace Product.Application.Application.Features.Brands.Commands.Import
{
    public partial class ImportBrandsCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportBrandsCommandHandler : IRequestHandler<ImportBrandsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditBrandCommand> _addBrandValidator;     

        public ImportBrandsCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditBrandCommand> addBrandValidator)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addBrandValidator = addBrandValidator;          
        }

        public async Task<Result<int>> Handle(ImportBrandsCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Brand, object>>
            {
                {"Name", (row,item) => item.Name = row["Name"].ToString() },
                {"Description", (row,item) => item.Description = row["Description"].ToString() },
                {"Tax", (row,item) => item.Tax = decimal.TryParse(row["Tax"].ToString(), out var tax) ? tax : 1 }
            }, "Brands"));

            if (result.Succeeded)
            {
                var importedBrands = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var brand in importedBrands)
                {
                    var validationResult = await _addBrandValidator.ValidateAsync(_mapper.Map<AddEditBrandCommand>(brand), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Brand>().AddAsync(brand);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(brand.Name) ? $"{brand.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandsCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}