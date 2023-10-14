﻿using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces.Repositories;
using Product.Application.Interfaces.Services;
using Product.Application.Requests;
using Product.Shared.Wrapper;

namespace Product.Application.Application.Features.Products.Commands.AddEdit;

public partial class AddEditProductCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Barcode { get; set; }
    [Required]
    public string Description { get; set; }
    public string ImageDataURL { get; set; }
    [Required]
    public decimal Rate { get; set; }
    [Required]
    public int BrandId { get; set; }
    public UploadRequest UploadRequest { get; set; }
}

internal class AddEditProductCommandHandler : IRequestHandler<AddEditProductCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IUploadService _uploadService;    

    public AddEditProductCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _uploadService = uploadService;       
    }

    public async Task<Result<int>> Handle(AddEditProductCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Repository<Domain.Entities.Product>().Entities.Where(p => p.Id != command.Id)
            .AnyAsync(p => p.Barcode == command.Barcode, cancellationToken))
        {
            return await Result<int>.FailAsync("Barcode already exists.");
        }

        var uploadRequest = command.UploadRequest;
        if (uploadRequest != null)
        {
            uploadRequest.FileName = $"P-{command.Barcode}{uploadRequest.Extension}";
        }

        if (command.Id == 0)
        {
            var product = _mapper.Map<Domain.Entities.Product>(command);
            if (uploadRequest != null)
            {
                product.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
            }
            await _unitOfWork.Repository<Domain.Entities.Product>().AddAsync(product);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(product.Id, "Product Saved");
        }
        else
        {
            var product = await _unitOfWork.Repository<Domain.Entities.Product>().GetByIdAsync(command.Id);
            if (product != null)
            {
                product.Name = command.Name ?? product.Name;
                product.Description = command.Description ?? product.Description;
                if (uploadRequest != null)
                {
                    product.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                product.Rate = (command.Rate == 0) ? product.Rate : command.Rate;
                product.BrandId = (command.BrandId == 0) ? product.BrandId : command.BrandId;
                await _unitOfWork.Repository<Domain.Entities.Product>().UpdateAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(product.Id, "Product Updated");
            }
            else
            {
                return await Result<int>.FailAsync("Product Not Found!");
            }
        }
    }
}