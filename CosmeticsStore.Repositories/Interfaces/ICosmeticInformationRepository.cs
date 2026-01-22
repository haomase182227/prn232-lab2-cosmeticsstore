using CosmeticsStore.Repositories.Models;

namespace CosmeticsStore.Repositories.Interfaces;

public interface ICosmeticInformationRepository
{
    Task<List<CosmeticInformation>> GetAllCosmetics();
    Task<(List<CosmeticInformation> Items, int TotalCount)> SearchCosmetics(
        string? searchTerm,
        string? cosmeticName,
        string? skinType,
        string? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        string sortBy,
        string sortOrder,
        int pageNumber,
        int pageSize,
        bool includeCategory);
    Task<CosmeticInformation> GetOne(string id);
    Task<CosmeticInformation> Add(CosmeticInformation cosmeticInformation);
    Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation);
    Task<CosmeticInformation> Delete(string id);
    Task<List<CosmeticCategory>> GetAllCategories();
}
