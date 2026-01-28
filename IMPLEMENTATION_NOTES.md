# Các Thay ??i Theo Yêu C?u

## ? 1. Xóa m?m (Soft Delete)
**Yêu c?u**: Thêm status, khi get thì l?c nh?ng b?n ghi ch?a b? xóa

### Th?c hi?n:
- Thêm field `Status` (int) vào `CosmeticInformation` và `CosmeticCategory`
  - Status = 1: Active (?ang ho?t ??ng)
  - Status = 0: Deleted (?ã xóa m?m)
- T?t c? query GET ??u filter `Status == 1` ?? ch? l?y records active
- API endpoints:
  - `DELETE /api/cosmetics/{id}` - Soft delete (set Status = 0)
  - `DELETE /api/cosmetics/{id}/hard` - Hard delete (xóa v?nh vi?n) - Admin only

## ? 2. Hai Lo?i Khóa (ID và Code)
**Yêu c?u**: Có 2 lo?i khóa là id và code, id dùng trong data, code cho user

### Th?c hi?n:
- Thêm `CosmeticCode` và `CategoryCode` fields
  - `CosmeticId` / `CategoryId`: Internal database ID (dùng trong data)
  - `CosmeticCode` / `CategoryCode`: User-friendly code (dùng cho user)
- API endpoints:
  - `GET /api/cosmetics/{id}` - Get by internal ID
  - `GET /api/cosmetics/code/{code}` - Get by user-friendly code
- Search h? tr? c? 2 lo?i khóa:
  - `category-id`: Search by internal CategoryId
  - `category-code`: Search by user-friendly CategoryCode

## ? 3. Search Nâng Cao
**Yêu c?u**: Search có min max, search theo khóa ngo?i và khóa chính

### Th?c hi?n:
- **Search t?ng quát**: `search-term` (tìm trong nhi?u fields: name, code, skin-type, size)
- **Search c? th?**: 
  - `cosmetic-name`: Tìm theo tên
  - `cosmetic-code`: Tìm theo code
  - `skin-type`: Tìm theo lo?i da
- **Filter theo khóa ngo?i**:
  - `category-id`: Filter by CategoryId (khóa chính)
  - `category-code`: Filter by CategoryCode (code thân thi?n)
- **Filter theo giá có min/max**:
  - `min-price`: Giá t?i thi?u
  - `max-price`: Giá t?i ?a

## ? 4. PageSize Default và Max
**Yêu c?u**: PageSize default: 50-100, có max

### Th?c hi?n:
- `page-size`: Default = 50, Max = 100
- N?u request > 100, t? ??ng gi?i h?n v? 100
- Lo?i b? `NoPaging` option (không còn h? tr?)
- Validation: `[Range(1, 100)]` trong request model

## ? 5. Sort Theo Timestamp, Alphabetic, Code
**Yêu c?u**: Không sort theo khóa id, sort theo timestamp, alp, mã code s?n ph?m

### Th?c hi?n:
- Thêm `CreatedAt` và `UpdatedAt` timestamps (DateTime)
- **Không sort theo ID** (CosmeticId, CategoryId)
- **Sort options**:
  - `created-at`: Sort by creation timestamp (default DESC)
  - `updated-at`: Sort by update timestamp
  - `cosmetic-code` / `code`: Sort by product code
  - `cosmetic-name` / `name`: Sort alphabetically by name
  - `dollar-price` / `price`: Sort by price
  - `skin-type`: Sort by skin type
- Default sort: `created-at DESC` (m?i nh?t tr??c)

## ? 6. Include Category và Field Selection
**Yêu c?u**: Paging v?i category b?, thêm fields, include-category

### Th?c hi?n:
- `include-category` (boolean): Include category details in response
- `fields` (string): Select specific fields to return (reserved for future implementation)
- **Lo?i b? NoPaging**: Luôn có paging v?i max 100 items

## ?? API Query Examples

### Example 1: Basic Search
```
GET /api/cosmetics?search-term=moisturizer&page=1&page-size=50
```

### Example 2: Filter by Price Range
```
GET /api/cosmetics?min-price=10&max-price=100&page=1&page-size=20
```

### Example 3: Search by Category Code
```
GET /api/cosmetics?category-code=CAT001&include-category=true
```

### Example 4: Sort by Created Date (Newest First)
```
GET /api/cosmetics?sort-by=created-at&sort-order=desc&page=1
```

### Example 5: Sort by Product Code (Alphabetically)
```
GET /api/cosmetics?sort-by=code&sort-order=asc
```

### Example 6: Complex Search
```
GET /api/cosmetics?search-term=serum&skin-type=oily&min-price=20&max-price=80&sort-by=price&sort-order=asc&include-category=true&page=1&page-size=50
```

## ??? Database Migration

### T?o Migration:
```bash
cd CosmeticsStore.API
dotnet ef migrations add AddSoftDeleteAndCodeFields --project ..\CosmeticsStore.Repositories
```

### Apply Migration:
```bash
dotnet ef database update --project CosmeticsStore.API
```

### New Database Columns:
**CosmeticInformation Table:**
- `CosmeticCode` (nvarchar): User-friendly code
- `Status` (int): 1=Active, 0=Deleted (default: 1)
- `CreatedAt` (datetime2): Creation timestamp (default: GETUTCDATE())
- `UpdatedAt` (datetime2): Last update timestamp (default: GETUTCDATE())

**CosmeticCategory Table:**
- `CategoryCode` (nvarchar): User-friendly code
- `Status` (int): 1=Active, 0=Deleted (default: 1)
- `CreatedAt` (datetime2): Creation timestamp (default: GETUTCDATE())
- `UpdatedAt` (datetime2): Last update timestamp (default: GETUTCDATE())

## ?? Connection String Configuration

### For Local Development:
Update `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnectionString": "Server=localhost,1433;Database=CosmeticsDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  }
}
```

### For Docker:
Use `appsettings.json` with Docker hostname:
```json
{
  "ConnectionStrings": {
    "DefaultConnectionString": "Server=cosmetics-sqlserver,1433;Database=CosmeticsDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  }
}
```

## ?? Running Migrations

### Option 1: Local SQL Server
Ensure SQL Server is running on localhost:1433

### Option 2: Docker SQL Server
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

Then run:
```bash
cd CosmeticsStore.API
dotnet ef database update
```

## ?? Response Model Changes

### CosmeticResponse now includes:
```json
{
  "cosmeticId": "PL123456",
  "cosmeticCode": "PROD-001",
  "cosmeticName": "Anti-Aging Serum",
  "skinType": "Dry",
  "expirationDate": "2025-12-31",
  "cosmeticSize": "50ml",
  "dollarPrice": 49.99,
  "categoryId": "CAT001",
  "status": 1,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-20T14:45:00Z",
  "category": {
    "categoryId": "CAT001",
    "categoryCode": "SKINCARE",
    "categoryName": "Skin Care"
  }
}
```

## ? Performance Enhancements

1. **Retry on Failure**: Added `EnableRetryOnFailure` for transient SQL connection errors
2. **Indexed Queries**: Soft delete filtering with `Status == 1`
3. **Efficient Sorting**: Default sort by timestamp index
4. **Pagination Limits**: Max 100 items per page to prevent large queries

## ?? Implementation Summary

? Soft delete with Status field  
? Dual key system (ID + Code)  
? Advanced search with min/max price  
? Search by foreign key (ID and Code)  
? PageSize default 50, max 100  
? Sort by timestamp, alphabetic, code (not ID)  
? Include category option  
? Field selection support (prepared)  
? Connection string fix for local development  
? Database migration created and ready  
