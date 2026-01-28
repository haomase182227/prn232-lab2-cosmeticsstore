using CosmeticsStore.API.Mappers;
using CosmeticsStore.API.Models;
using CosmeticsStore.API.Models.RequestModels;
using CosmeticsStore.API.Models.ResponseModels;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CosmeticsStore.API.Controllers;

/// <summary>
/// Controller for Cosmetic Information endpoints
/// URL: lowercase, Query params: kebab-case
/// All endpoints require authentication
/// </summary>
[ApiController]
[Route("api/cosmetics")]
[Authorize]
public class CosmeticsController : ControllerBase
{
    private readonly ICosmeticInformationService _cosmeticInformationService;

    public CosmeticsController(ICosmeticInformationService cosmeticInformationService)
    {
        _cosmeticInformationService = cosmeticInformationService;
    }

    /// <summary>
    /// Get cosmetics list with search, filter, sort, paging
    /// Query params use kebab-case: search-term, min-price, sort-by, etc.
    /// Supports:
    /// - Search by multiple fields (search-term) or specific fields (cosmetic-name, cosmetic-code, skin-type)
    /// - Filter by foreign key (category-id) or code (category-code)
    /// - Filter by price range (min-price, max-price)
    /// - Sort by timestamp (created-at, updated-at), alphabetic (name), or code (not ID)
    /// - Paging with default 50, max 100 per page
    /// - Field selection and include-category option
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<CosmeticResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<ActionResult<ApiResponse<PagedResponse<CosmeticResponse>>>> GetCosmetics(
        [FromQuery] CosmeticSearchRequest request)
    {
        // Validate page size (max 100)
        if (request.PageSize > 100)
        {
            request.PageSize = 100;
        }

        var (items, totalCount) = await _cosmeticInformationService.SearchCosmetics(
            request.SearchTerm,
            request.CosmeticName,
            request.CosmeticCode,
            request.SkinType,
            request.CategoryId,
            request.CategoryCode,
            request.MinPrice,
            request.MaxPrice,
            GetSortByString(request.SortBy),
            GetSortOrderString(request.SortOrder),
            request.Page,
            request.PageSize,
            request.IncludeCategory
        );

        var pagedResponse = CosmeticMapper.ToPagedResponse(
            items,
            totalCount,
            request.Page,
            request.PageSize
        );

        return Ok(ApiResponse<PagedResponse<CosmeticResponse>>.SuccessResponse(
            pagedResponse,
            "Cosmetics retrieved successfully"
        ));
    }

    /// <summary>
    /// Get single cosmetic by ID (internal database ID)
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CosmeticResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<ActionResult<ApiResponse<CosmeticResponse>>> GetCosmetic(string id)
    {        
        var entity = await _cosmeticInformationService.GetOne(id);
        
        if (entity == null)
        {
            return NotFound(ApiResponse<CosmeticResponse>.ErrorResponse(
                $"Cosmetic with ID '{id}' not found"
            ));
        }

        var response = CosmeticMapper.ToResponse(entity);
        return Ok(ApiResponse<CosmeticResponse>.SuccessResponse(
            response,
            "Cosmetic retrieved successfully"
        ));
    }

    /// <summary>
    /// Get single cosmetic by Code (user-friendly code)
    /// </summary>
    [HttpGet("code/{code}")]
    [ProducesResponseType(typeof(ApiResponse<CosmeticResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<ActionResult<ApiResponse<CosmeticResponse>>> GetCosmeticByCode(string code)
    {        
        var entity = await _cosmeticInformationService.GetOneByCode(code);
        
        if (entity == null)
        {
            return NotFound(ApiResponse<CosmeticResponse>.ErrorResponse(
                $"Cosmetic with Code '{code}' not found"
            ));
        }

        var response = CosmeticMapper.ToResponse(entity);
        return Ok(ApiResponse<CosmeticResponse>.SuccessResponse(
            response,
            "Cosmetic retrieved successfully"
        ));
    }

    /// <summary>
    /// Create a new cosmetic
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CosmeticResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<ActionResult<ApiResponse<CosmeticResponse>>> CreateCosmetic(
        [FromBody] CreateCosmeticRequest request)
    {
        var entity = CosmeticMapper.ToEntity(request);
        var created = await _cosmeticInformationService.Add(entity);
        var response = CosmeticMapper.ToResponse(created);

        return CreatedAtAction(
            nameof(GetCosmetic),
            new { id = response.CosmeticId },
            ApiResponse<CosmeticResponse>.SuccessResponse(
                response,
                "Cosmetic created successfully"
            )
        );
    }

    /// <summary>
    /// Update existing cosmetic
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CosmeticResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<ActionResult<ApiResponse<CosmeticResponse>>> UpdateCosmetic(
        string id,
        [FromBody] UpdateCosmeticRequest request)
    {
        var entity = CosmeticMapper.ToEntity(request, id);
        var updated = await _cosmeticInformationService.Update(entity);
        var response = CosmeticMapper.ToResponse(updated);

        return Ok(ApiResponse<CosmeticResponse>.SuccessResponse(
            response,
            "Cosmetic updated successfully"
        ));
    }

    /// <summary>
    /// Soft delete cosmetic (set Status = 0, kh�ng x�a kh?i database)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CosmeticResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<ActionResult<ApiResponse<CosmeticResponse>>> SoftDeleteCosmetic(string id)
    {
        var deleted = await _cosmeticInformationService.SoftDelete(id);
        var response = CosmeticMapper.ToResponse(deleted);

        return Ok(ApiResponse<CosmeticResponse>.SuccessResponse(
            response,
            "Cosmetic soft deleted successfully"
        ));
    }

    /// <summary>
    /// Hard delete cosmetic (x�a v?nh vi?n kh?i database) - Admin only
    /// </summary>
    [HttpDelete("{id}/hard")]
    [Authorize(Roles = "1")] // Admin only
    [ProducesResponseType(typeof(ApiResponse<CosmeticResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<ActionResult<ApiResponse<CosmeticResponse>>> HardDeleteCosmetic(string id)
    {
        var deleted = await _cosmeticInformationService.Delete(id);
        var response = CosmeticMapper.ToResponse(deleted);

        return Ok(ApiResponse<CosmeticResponse>.SuccessResponse(
            response,
            "Cosmetic permanently deleted"
        ));
    }

    /// <summary>
    /// Get all cosmetic categories
    /// </summary>
    [HttpGet("~/api/categories")]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryResponse>>), 200)]
    public async Task<ActionResult<ApiResponse<List<CategoryResponse>>>> GetCategories()
    {
        var entities = await _cosmeticInformationService.GetAllCategories();
        var response = entities.Select(CosmeticMapper.ToCategoryResponse).ToList();

        return Ok(ApiResponse<List<CategoryResponse>>.SuccessResponse(
            response,
            "Categories retrieved successfully"
        ));
    }

    // ========== OData Endpoints (Legacy Support) ==========

    /// <summary>
    /// OData endpoint for querying cosmetic informations
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] // Hide from Swagger
    [EnableQuery]
    [HttpGet("~/odata/CosmeticInformations")]
    public async Task<ActionResult<IEnumerable<CosmeticInformation>>> GetODataCosmeticInformations()
    {
        var result = await _cosmeticInformationService.GetAllCosmetics();
        return Ok(result);
    }

    /// <summary>
    /// OData endpoint for getting count of cosmetic informations
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] // Hide from Swagger
    [EnableQuery]
    [HttpGet("~/odata/CosmeticInformations/$count")]
    public async Task<ActionResult<int>> GetODataCosmeticInformationsCount()
    {
        var result = await _cosmeticInformationService.GetAllCosmetics();
        return Ok(result.Count);
    }

    // Helper methods to convert enum to kebab-case string
    private string GetSortByString(SortByOption? sortBy)
    {
        return sortBy switch
        {
            SortByOption.CreatedAt => "created-at",
            SortByOption.UpdatedAt => "updated-at",
            SortByOption.Name => "name",
            SortByOption.Code => "code",
            SortByOption.Price => "price",
            _ => "created-at"
        };
    }

    private string GetSortOrderString(SortOrderOption? sortOrder)
    {
        return sortOrder switch
        {
            SortOrderOption.Asc => "asc",
            SortOrderOption.Desc => "desc",
            _ => "desc"
        };
    }
}
