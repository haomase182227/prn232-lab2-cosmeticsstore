using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Services.Interfaces;

namespace CosmeticsStore.Services;

public class CosmeticInformationService : ICosmeticInformationService
{
    private readonly ICosmeticInformationRepository _repository;
    
    public CosmeticInformationService(ICosmeticInformationRepository repository)
    {
        _repository = repository;
    }

    public async Task<CosmeticInformation> Add(CosmeticInformation cosmeticInformation)
    {
        return await _repository.Add(cosmeticInformation);
    }

    public async Task<CosmeticInformation> Delete(string id)
    {
        return await _repository.Delete(id);
    }

    public async Task<List<CosmeticCategory>> GetAllCategories()
    {
        return await _repository.GetAllCategories();
    }

    public async Task<List<CosmeticInformation>> GetAllCosmetics()
    {
        return await _repository.GetAllCosmetics();
    }

    public async Task<(List<CosmeticInformation> Items, int TotalCount)> SearchCosmetics(
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
        bool includeCategory)
    {
        return await _repository.SearchCosmetics(
            searchTerm, cosmeticName, skinType, categoryId, minPrice, maxPrice,
            sortBy, sortOrder, pageNumber, pageSize, includeCategory);
    }

    public async Task<CosmeticInformation> GetOne(string id)
    {
        return await _repository.GetOne(id);
    }

    public async Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation)
    {
        return await _repository.Update(cosmeticInformation);
    }
}
