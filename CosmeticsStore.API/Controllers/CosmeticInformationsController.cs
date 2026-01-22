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
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<CosmeticResponse>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<ActionResult<ApiResponse<PagedResponse<CosmeticResponse>>>> GetCosmetics(
        [FromQuery] CosmeticSearchRequest request)
    {
        // Convert kebab-case sort-by to PascalCase for database
        var sortBy = request.SortBy switch
        {
            "cosmetic-name" => "CosmeticName",
            "skin-type" => "SkinType",
            "dollar-price" => "DollarPrice",
            "expiration-date" => "ExpirationDate",
            _ => "CosmeticName"
        };

        int pageSize = request.NoPaging ? int.MaxValue : request.PageSize;
        int pageNumber = request.NoPaging ? 1 : request.Page;

        var (items, totalCount) = await _cosmeticInformationService.SearchCosmetics(
            request.SearchTerm,
            request.CosmeticName,
            request.SkinType,
            request.CategoryId,
            request.MinPrice,
            request.MaxPrice,
            sortBy,
            request.SortOrder ?? "asc",
            pageNumber,
            pageSize,
            request.IncludeCategory
        );

        var pagedResponse = CosmeticMapper.ToPagedResponse(
            items,
            totalCount,
            pageNumber,
            request.NoPaging ? totalCount : request.PageSize
        );

        return Ok(ApiResponse<PagedResponse<CosmeticResponse>>.SuccessResponse(
            pagedResponse,
            "Cosmetics retrieved successfully"
        ));
    }

    /// <summary>
    /// Get single cosmetic by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CosmeticResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<ActionResult<ApiResponse<CosmeticResponse>>> GetCosmetic(string id)
    {        var entity = await _cosmeticInformationService.GetOne(id);
        
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
    /// Delete cosmetic
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CosmeticResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<ActionResult<ApiResponse<CosmeticResponse>>> DeleteCosmetic(string id)
    {
        var deleted = await _cosmeticInformationService.Delete(id);
        var response = CosmeticMapper.ToResponse(deleted);

        return Ok(ApiResponse<CosmeticResponse>.SuccessResponse(
            response,
            "Cosmetic deleted successfully"
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
}
