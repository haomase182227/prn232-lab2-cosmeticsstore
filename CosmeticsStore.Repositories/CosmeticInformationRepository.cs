using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmeticsStore.Repositories;

public class CosmeticInformationRepository : ICosmeticInformationRepository
{
    private readonly CosmeticsDbContext _context;

    public CosmeticInformationRepository(CosmeticsDbContext context)
    {
        _context = context;
    }

    public async Task<CosmeticInformation> Add(CosmeticInformation cosmeticInformation)
    {
        var categoryObject = await _context.CosmeticCategories
            .FirstOrDefaultAsync(x => x.CategoryId.Equals(cosmeticInformation.CategoryId));
        if (categoryObject == null)
        {
            throw new Exception("Category is not found");
        }

        cosmeticInformation.CosmeticId = GenerateId();
        await _context.CosmeticInformations.AddAsync(cosmeticInformation);
        await _context.SaveChangesAsync();
        return cosmeticInformation;
    }

    public async Task<CosmeticInformation> Delete(string id)
    {
        var deleteObject = await _context.CosmeticInformations
            .FirstOrDefaultAsync(p => p.CosmeticId.Equals(id));
        if (deleteObject == null)
        {
            throw new Exception("CosmeticInformations not found");
        }

        _context.CosmeticInformations.Remove(deleteObject);
        await _context.SaveChangesAsync();
        return deleteObject;
    }

    public async Task<List<CosmeticCategory>> GetAllCategories()
    {
        var listCategories = await _context.CosmeticCategories.ToListAsync();
        return listCategories;
    }

    public async Task<List<CosmeticInformation>> GetAllCosmetics()
    {
        var listCosmetics = await _context.CosmeticInformations
            .Include(x => x.Category)
            .ToListAsync();
        return listCosmetics;
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
        IQueryable<CosmeticInformation> query = _context.CosmeticInformations;

        // Extension - Include related entities
        if (includeCategory)
        {
            query = query.Include(x => x.Category);
        }

        // Search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x =>
                x.CosmeticName.Contains(searchTerm) ||
                x.SkinType.Contains(searchTerm) ||
                x.CosmeticSize.Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(cosmeticName))
        {
            query = query.Where(x => x.CosmeticName.Contains(cosmeticName));
        }

        if (!string.IsNullOrEmpty(skinType))
        {
            query = query.Where(x => x.SkinType.Contains(skinType));
        }

        if (!string.IsNullOrEmpty(categoryId))
        {
            query = query.Where(x => x.CategoryId == categoryId);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(x => x.DollarPrice >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(x => x.DollarPrice <= maxPrice.Value);
        }

        // Total count before paging
        var totalCount = await query.CountAsync();

        // Sort
        query = sortBy?.ToLower() switch
        {
            "cosmeticname" => sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.CosmeticName)
                : query.OrderBy(x => x.CosmeticName),
            "dollarprice" or "price" => sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.DollarPrice)
                : query.OrderBy(x => x.DollarPrice),
            "skintype" => sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.SkinType)
                : query.OrderBy(x => x.SkinType),
            "expirationdate" => sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.ExpirationDate)
                : query.OrderBy(x => x.ExpirationDate),
            _ => query.OrderBy(x => x.CosmeticName)
        };

        // Paging
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<CosmeticInformation> GetOne(string id)
    {
        var resultObject = await _context.CosmeticInformations
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.CosmeticId.Equals(id));
        return resultObject;
    }

    public async Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation)
    {
        var updateObject = await _context.CosmeticInformations
            .FirstOrDefaultAsync(x => x.CosmeticId == cosmeticInformation.CosmeticId);
        if (updateObject == null)
        {
            throw new Exception("CosmeticInformations not found");
        }

        var cate = await _context.CosmeticCategories
            .FirstOrDefaultAsync(x => x.CategoryId.Equals(cosmeticInformation.CategoryId));
        if (cate == null)
        {
            throw new Exception("Cate not found");
        }

        updateObject.CosmeticName = cosmeticInformation.CosmeticName;
        updateObject.SkinType = cosmeticInformation.SkinType;
        updateObject.ExpirationDate = cosmeticInformation.ExpirationDate;
        updateObject.CosmeticSize = cosmeticInformation.CosmeticSize;
        updateObject.DollarPrice = cosmeticInformation.DollarPrice;
        updateObject.CategoryId = cosmeticInformation.CategoryId;

        _context.CosmeticInformations.Update(updateObject);
        await _context.SaveChangesAsync();
        return updateObject;
    }

    private string GenerateId()
    {
        var random = new Random();
        var id = random.Next(100000, 999999);
        return "PL" + id.ToString();
    }
}
