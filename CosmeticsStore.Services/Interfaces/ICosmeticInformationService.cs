using CosmeticsStore.Repositories.Models;

namespace CosmeticsStore.Services.Interfaces;

public interface ICosmeticInformationService
{
    Task<List<CosmeticInformation>> GetAllCosmetics();
    Task<(List<CosmeticInformation> Items, int TotalCount)> SearchCosmetics(
        string? searchTerm,
        string? cosmeticName,
        string? cosmeticCode,
        string? skinType,
        string? categoryId,
        string? categoryCode,
        decimal? minPrice,
        decimal? maxPrice,
        string sortBy,
        string sortOrder,
        int pageNumber,
        int pageSize);
    Task<CosmeticInformation> GetOne(string id);
    Task<CosmeticInformation> GetOneByCode(string code);
    Task<CosmeticInformation> Add(CosmeticInformation cosmeticInformation);
    Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation);
    Task<CosmeticInformation> Delete(string id);
    Task<CosmeticInformation> SoftDelete(string id);
    Task<List<CosmeticCategory>> GetAllCategories();
}
