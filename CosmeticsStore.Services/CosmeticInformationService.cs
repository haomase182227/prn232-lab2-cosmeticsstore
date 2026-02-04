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
        cosmeticInformation.Status = 1;
        cosmeticInformation.CreatedAt = DateTime.UtcNow;
        cosmeticInformation.UpdatedAt = DateTime.UtcNow;
        return await _repository.Add(cosmeticInformation);
    }

    public async Task<CosmeticInformation> Delete(string id)
    {
        return await _repository.Delete(id);
    }

    public async Task<CosmeticInformation> SoftDelete(string id)
    {
        var cosmetic = await _repository.GetOne(id);
        if (cosmetic == null)
            throw new KeyNotFoundException($"Cosmetic with ID '{id}' not found");
        
        cosmetic.Status = 0;
        cosmetic.UpdatedAt = DateTime.UtcNow;
        return await _repository.Update(cosmetic);
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
        string? cosmeticCode,
        string? skinType,
        string? categoryId,
        string? categoryCode,
        decimal? minPrice,
        decimal? maxPrice,
        string sortBy,
        string sortOrder,
        int pageNumber,
        int pageSize)
    {
        return await _repository.SearchCosmetics(
            searchTerm, cosmeticName, cosmeticCode, skinType, categoryId, categoryCode, 
            minPrice, maxPrice, sortBy, sortOrder, pageNumber, pageSize);
    }

    public async Task<CosmeticInformation> GetOne(string id)
    {
        return await _repository.GetOne(id);
    }

    public async Task<CosmeticInformation> GetOneByCode(string code)
    {
        return await _repository.GetOneByCode(code);
    }

    public async Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation)
    {
        cosmeticInformation.UpdatedAt = DateTime.UtcNow;
        return await _repository.Update(cosmeticInformation);
    }
}
