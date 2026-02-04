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
        // Ch? l?y categories c� Status = 1 (Active)
        var listCategories = await _context.CosmeticCategories
            .Where(c => c.Status == 1)
            .ToListAsync();
        return listCategories;
    }

    public async Task<List<CosmeticInformation>> GetAllCosmetics()
    {
        // Ch? l?y cosmetics c� Status = 1 (Active)
        var listCosmetics = await _context.CosmeticInformations
            .Include(x => x.Category)
            .Where(x => x.Status == 1)
            .ToListAsync();
        return listCosmetics;
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
        IQueryable<CosmeticInformation> query = _context.CosmeticInformations;

        // L?c ch? l?y records c� Status = 1 (Active)
        query = query.Where(x => x.Status == 1);

        // Always include category
        query = query.Include(x => x.Category);

        // Search (t�m ki?m trong nhi?u fields)
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x =>
                x.CosmeticName.Contains(searchTerm) ||
                x.CosmeticCode.Contains(searchTerm) ||
                x.SkinType.Contains(searchTerm) ||
                x.CosmeticSize.Contains(searchTerm));
        }

        // Filter by specific fields
        if (!string.IsNullOrEmpty(cosmeticName))
        {
            query = query.Where(x => x.CosmeticName.Contains(cosmeticName));
        }

        if (!string.IsNullOrEmpty(cosmeticCode))
        {
            query = query.Where(x => x.CosmeticCode.Contains(cosmeticCode));
        }

        if (!string.IsNullOrEmpty(skinType))
        {
            query = query.Where(x => x.SkinType.Contains(skinType));
        }

        // Search by kh�a ngo?i (CategoryId)
        if (!string.IsNullOrEmpty(categoryId))
        {
            query = query.Where(x => x.CategoryId == categoryId);
        }

        // Search by kh�a ngo?i code (CategoryCode)
        if (!string.IsNullOrEmpty(categoryCode))
        {
            query = query.Where(x => x.Category != null && x.Category.CategoryCode.Contains(categoryCode));
        }

        // Search c� min max
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

        // Sort: Kh�ng sort theo ID, sort theo timestamp, alphabetic, m� code
        query = sortBy?.ToLower() switch
        {
            "created-at" or "createdat" => sortOrder?.ToLower() == "asc"
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt),
            "updated-at" or "updatedat" => sortOrder?.ToLower() == "asc"
                ? query.OrderBy(x => x.UpdatedAt)
                : query.OrderByDescending(x => x.UpdatedAt),
            "cosmetic-code" or "cosmeticcode" or "code" => sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.CosmeticCode)
                : query.OrderBy(x => x.CosmeticCode),
            "cosmetic-name" or "cosmeticname" or "name" => sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.CosmeticName)
                : query.OrderBy(x => x.CosmeticName),
            "dollar-price" or "dollarprice" or "price" => sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.DollarPrice)
                : query.OrderBy(x => x.DollarPrice),
            "skin-type" or "skintype" => sortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.SkinType)
                : query.OrderBy(x => x.SkinType),
            _ => query.OrderByDescending(x => x.CreatedAt) // Default sort by CreatedAt DESC
        };

        // Paging (PageSize default 50, max 100)
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
            .Where(x => x.Status == 1) // Ch? l?y active records
            .FirstOrDefaultAsync(x => x.CosmeticId.Equals(id));
        return resultObject;
    }

    public async Task<CosmeticInformation> GetOneByCode(string code)
    {
        var resultObject = await _context.CosmeticInformations
            .Include(x => x.Category)
            .Where(x => x.Status == 1) // Ch? l?y active records
            .FirstOrDefaultAsync(x => x.CosmeticCode.Equals(code));
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

        updateObject.CosmeticCode = cosmeticInformation.CosmeticCode;
        updateObject.CosmeticName = cosmeticInformation.CosmeticName;
        updateObject.SkinType = cosmeticInformation.SkinType;
        updateObject.ExpirationDate = cosmeticInformation.ExpirationDate;
        updateObject.CosmeticSize = cosmeticInformation.CosmeticSize;
        updateObject.DollarPrice = cosmeticInformation.DollarPrice;
        updateObject.CategoryId = cosmeticInformation.CategoryId;
        updateObject.UpdatedAt = cosmeticInformation.UpdatedAt;

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
