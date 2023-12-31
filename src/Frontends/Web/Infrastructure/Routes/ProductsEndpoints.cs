﻿using System.Linq;

namespace BlazorApp.Client.Infrastructure.Routes;

public static class ProductsEndpoints
{
    public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
    {
        var url = $"product/api/products?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
        if (orderBy?.Any() == true)
        {
            foreach (var orderByPart in orderBy)
            {
                url += $"{orderByPart},";
            }
            url = url[..^1]; 
        }
        return url;
    }

    public static string GetCount = "product/api/products/count";

    public static string GetProductImage(int productId)
    {
        return $"product/api/products/image/{productId}";
    }

    public static string ExportFiltered(string searchString)
    {
        return $"product/{Export}?searchString={searchString}";
    }

    public static string Save = "product/api/products";
    public static string Delete = "product/api/products";

    private static string ExportEndpoint = "product/api/products/export";

    public static string Export(string searchString) => string.IsNullOrWhiteSpace(searchString)
                                                         ? ExportEndpoint
                                                         : ExportFiltered(searchString);

}