using CosmeticsStore.API.Models.RequestModels;
using CosmeticsStore.API.Models.ResponseModels;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Services.Models;

namespace CosmeticsStore.API.Mappers;

/// <summary>
/// Mapper class to convert between different model types
/// </summary>
public static class CosmeticMapper
{
    // Request Model -> Entity Model (for Repository)
    public static CosmeticInformation ToEntity(CreateCosmeticRequest request)
    {
        return new CosmeticInformation
        {
            CosmeticId = request.CosmeticId,
            CosmeticCode = request.CosmeticCode,
            CosmeticName = request.CosmeticName,
            SkinType = request.SkinType,
            ExpirationDate = request.ExpirationDate,
            CosmeticSize = request.CosmeticSize,
            DollarPrice = request.DollarPrice,
            CategoryId = request.CategoryId
        };
    }

    public static CosmeticInformation ToEntity(UpdateCosmeticRequest request, string id)
    {
        return new CosmeticInformation
        {
            CosmeticId = id,
            CosmeticCode = request.CosmeticCode,
            CosmeticName = request.CosmeticName,
            SkinType = request.SkinType,
            ExpirationDate = request.ExpirationDate,
            CosmeticSize = request.CosmeticSize,
            DollarPrice = request.DollarPrice,
            CategoryId = request.CategoryId
        };
    }

    // Business Model -> Entity Model (for Repository)
    public static CosmeticInformation ToEntity(CosmeticBusinessModel businessModel)
    {
        return new CosmeticInformation
        {
            CosmeticId = businessModel.CosmeticId,
            CosmeticCode = businessModel.CosmeticCode,
            CosmeticName = businessModel.CosmeticName,
            SkinType = businessModel.SkinType,
            ExpirationDate = businessModel.ExpirationDate,
            CosmeticSize = businessModel.CosmeticSize,
            DollarPrice = businessModel.DollarPrice,
            CategoryId = businessModel.CategoryId,
            Status = businessModel.Status,
            CreatedAt = businessModel.CreatedAt,
            UpdatedAt = businessModel.UpdatedAt,
            Category = businessModel.Category != null ? new CosmeticCategory
            {
                CategoryId = businessModel.Category.CategoryId,
                CategoryCode = businessModel.Category.CategoryCode,
                CategoryName = businessModel.Category.CategoryName
            } : null
        };
    }

    // Entity Model -> Business Model (for Service)
    public static CosmeticBusinessModel ToBusinessModel(CosmeticInformation entity)
    {
        return new CosmeticBusinessModel
        {
            CosmeticId = entity.CosmeticId,
            CosmeticCode = entity.CosmeticCode,
            CosmeticName = entity.CosmeticName,
            SkinType = entity.SkinType,
            ExpirationDate = entity.ExpirationDate,
            CosmeticSize = entity.CosmeticSize,
            DollarPrice = entity.DollarPrice,
            CategoryId = entity.CategoryId,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Category = entity.Category != null ? new CategoryBusinessModel
            {
                CategoryId = entity.Category.CategoryId,
                CategoryCode = entity.Category.CategoryCode,
                CategoryName = entity.Category.CategoryName
            } : null
        };
    }

    // Business Model -> Response Model (for API)
    public static CosmeticResponse ToResponse(CosmeticBusinessModel businessModel)
    {
        return new CosmeticResponse
        {
            CosmeticId = businessModel.CosmeticId,
            CosmeticCode = businessModel.CosmeticCode,
            CosmeticName = businessModel.CosmeticName,
            SkinType = businessModel.SkinType,
            ExpirationDate = businessModel.ExpirationDate,
            CosmeticSize = businessModel.CosmeticSize,
            DollarPrice = businessModel.DollarPrice,
            CategoryId = businessModel.CategoryId,
            Status = businessModel.Status,
            CreatedAt = businessModel.CreatedAt,
            UpdatedAt = businessModel.UpdatedAt,
            Category = businessModel.Category != null ? new CategoryResponse
            {
                CategoryId = businessModel.Category.CategoryId,
                CategoryCode = businessModel.Category.CategoryCode,
                CategoryName = businessModel.Category.CategoryName
            } : null
        };
    }

    // Entity Model -> Response Model (direct conversion for API)
    public static CosmeticResponse ToResponse(CosmeticInformation entity)
    {
        return new CosmeticResponse
        {
            CosmeticId = entity.CosmeticId,
            CosmeticCode = entity.CosmeticCode,
            CosmeticName = entity.CosmeticName,
            SkinType = entity.SkinType,
            ExpirationDate = entity.ExpirationDate,
            CosmeticSize = entity.CosmeticSize,
            DollarPrice = entity.DollarPrice,
            CategoryId = entity.CategoryId,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Category = entity.Category != null ? new CategoryResponse
            {
                CategoryId = entity.Category.CategoryId,
                CategoryCode = entity.Category.CategoryCode,
                CategoryName = entity.Category.CategoryName
            } : null
        };
    }

    // Category Entity -> Response Model
    public static CategoryResponse ToCategoryResponse(CosmeticCategory entity)
    {
        return new CategoryResponse
        {
            CategoryId = entity.CategoryId,
            CategoryCode = entity.CategoryCode,
            CategoryName = entity.CategoryName
        };
    }

    // Paged Result mapping
    public static PagedResponse<CosmeticResponse> ToPagedResponse(
        List<CosmeticInformation> items,
        int totalCount,
        int page,
        int pageSize)
    {
        var totalPages = pageSize > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 1;
        
        return new PagedResponse<CosmeticResponse>
        {
            Items = items.Select(ToResponse).ToList(),
            Pagination = new PaginationMetadata
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = totalPages,
                HasPrevious = page > 1,
                HasNext = page < totalPages
            }
        };
    }
}
