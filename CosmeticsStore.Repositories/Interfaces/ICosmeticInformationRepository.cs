using CosmeticsStore.Repositories.Models;

namespace CosmeticsStore.Repositories.Interfaces;

public interface ICosmeticInformationRepository
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
    Task<List<CosmeticCategory>> GetAllCategories();
}
