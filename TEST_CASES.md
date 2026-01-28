# Test Cases - Cosmetics Store API

## 1. SOFT DELETE (Xóa mềm)

### TC-SD-01: Soft Delete Success
**Endpoint:** `DELETE /api/cosmetics/{id}`
**Precondition:** Cosmetic với ID hợp lệ tồn tại và Status = 1
**Steps:**
1. Gọi API soft delete với valid ID
2. Verify response trả về thành công
3. Kiểm tra database: record vẫn tồn tại nhưng Status = 0

**Expected Result:**
- HTTP 200
- Status trong DB = 0
- Record không bị xóa khỏi database

### TC-SD-02: Soft Delete - Already Deleted
**Endpoint:** `DELETE /api/cosmetics/{id}`
**Precondition:** Cosmetic đã bị soft delete (Status = 0)
**Steps:**
1. Gọi API soft delete với ID của record đã deleted

**Expected Result:**
- HTTP 404 hoặc 409 Conflict
- Message: "Cosmetic already deleted"

### TC-SD-03: Get Cosmetics - Không lấy bản ghi đã xóa mềm
**Endpoint:** `GET /api/cosmetics`
**Precondition:** 
- Có 5 cosmetics: 3 active (Status=1), 2 deleted (Status=0)

**Steps:**
1. Gọi GET /api/cosmetics (không filter)
2. Đếm số records trả về

**Expected Result:**
- Chỉ trả về 3 cosmetics (Status = 1)
- Không có cosmetics nào có Status = 0

### TC-SD-04: Get By ID - Cosmetic đã xóa mềm
**Endpoint:** `GET /api/cosmetics/{id}`
**Precondition:** Cosmetic với ID đã bị soft delete
**Steps:**
1. Gọi GET /api/cosmetics/{id} với ID của deleted record

**Expected Result:**
- HTTP 404
- Message: "Cosmetic not found"

---

## 2. SEARCH VỚI MIN/MAX (Price Range)

### TC-SR-01: Search với Min Price
**Endpoint:** `GET /api/cosmetics?min-price=100`
**Precondition:** 
- Cosmetic A: Price = 50
- Cosmetic B: Price = 150
- Cosmetic C: Price = 200

**Expected Result:**
- Chỉ trả về Cosmetic B và C
- Không có cosmetic nào có price < 100

### TC-SR-02: Search với Max Price
**Endpoint:** `GET /api/cosmetics?max-price=180`
**Expected Result:**
- Chỉ trả về cosmetics có price ≤ 180
- Cosmetic C (200) không được trả về

### TC-SR-03: Search với Min và Max Price
**Endpoint:** `GET /api/cosmetics?min-price=100&max-price=180`
**Expected Result:**
- Chỉ trả về Cosmetic B (150)
- Price trong khoảng [100, 180]

### TC-SR-04: Search Min > Max (Invalid Range)
**Endpoint:** `GET /api/cosmetics?min-price=200&max-price=100`
**Expected Result:**
- HTTP 400 Bad Request
- Message: "Min price cannot be greater than max price"

### TC-SR-05: Search với Price = 0
**Endpoint:** `GET /api/cosmetics?min-price=0&max-price=0`
**Expected Result:**
- Trả về empty array hoặc cosmetics có price = 0
- HTTP 200

---

## 3. SEARCH THEO KHÓA NGOẠI VÀ KHÓA CHÍNH

### TC-FK-01: Search theo Foreign Key (CategoryId)
**Endpoint:** `GET /api/cosmetics?category-id=CAT001`
**Precondition:**
- Category "CAT001" có 3 cosmetics
- Category "CAT002" có 2 cosmetics

**Expected Result:**
- Chỉ trả về 3 cosmetics thuộc CAT001
- Không có cosmetics từ category khác

### TC-FK-02: Search theo Category Code (User-friendly)
**Endpoint:** `GET /api/cosmetics?category-code=SKINCARE`
**Precondition:**
- Category có code "SKINCARE" tồn tại

**Expected Result:**
- Trả về tất cả cosmetics thuộc category "SKINCARE"
- Kết quả giống với search theo CategoryId

### TC-PK-01: Search theo Primary Key (CosmeticId)
**Endpoint:** `GET /api/cosmetics/{id}`
**Steps:**
1. Gọi API với valid CosmeticId

**Expected Result:**
- HTTP 200
- Trả về đúng 1 cosmetic matching ID

### TC-PK-02: Search theo Code (User-friendly Key)
**Endpoint:** `GET /api/cosmetics/code/{code}`
**Steps:**
1. Gọi API với CosmeticCode (VD: "PROD-001")

**Expected Result:**
- HTTP 200
- Trả về đúng cosmetic có code "PROD-001"
- Code phải unique, không trùng lặp

### TC-FK-03: Search với Invalid CategoryId
**Endpoint:** `GET /api/cosmetics?category-id=INVALID999`
**Expected Result:**
- HTTP 200 với empty array
- Message: "No cosmetics found"

---

## 4. PAGE SIZE (Default & Max)

### TC-PS-01: Page Size Default
**Endpoint:** `GET /api/cosmetics` (không truyền page-size)
**Precondition:** Database có 150 cosmetics
**Expected Result:**
- Trả về 50 items (default)
- TotalCount = 150
- TotalPages = 3

### TC-PS-02: Page Size Custom (Valid)
**Endpoint:** `GET /api/cosmetics?page-size=20`
**Expected Result:**
- Trả về 20 items
- Pagination info chính xác

### TC-PS-03: Page Size vượt quá Max (>100)
**Endpoint:** `GET /api/cosmetics?page-size=150`
**Expected Result:**
- Tự động giới hạn về 100 items
- Warning hoặc message: "Page size limited to 100"

### TC-PS-04: Page Size = 0 hoặc âm
**Endpoint:** `GET /api/cosmetics?page-size=0`
**Expected Result:**
- HTTP 400 Bad Request
- Message: "Page size must be greater than 0"

### TC-PS-05: Pagination - Page Number
**Endpoint:** `GET /api/cosmetics?page=2&page-size=50`
**Precondition:** Database có 150 cosmetics
**Expected Result:**
- Trả về items từ 51-100
- CurrentPage = 2
- HasNextPage = true
- HasPreviousPage = true

---

## 5. SORT (Timestamp, Alphabetic, Code)

### TC-ST-01: Sort theo CreatedAt (Mới nhất)
**Endpoint:** `GET /api/cosmetics?sort-by=created-at&sort-order=desc`
**Expected Result:**
- Cosmetics mới tạo nhất ở đầu
- Sắp xếp theo timestamp giảm dần

### TC-ST-02: Sort theo UpdatedAt
**Endpoint:** `GET /api/cosmetics?sort-by=updated-at&sort-order=desc`
**Expected Result:**
- Cosmetics vừa update gần nhất ở đầu

### TC-ST-03: Sort theo Name (Alphabetic)
**Endpoint:** `GET /api/cosmetics?sort-by=name&sort-order=asc`
**Precondition:**
- Cosmetic A: Name = "Zinc Cream"
- Cosmetic B: Name = "Anti-Aging Serum"
- Cosmetic C: Name = "Moisturizer"

**Expected Result:**
- Thứ tự: Anti-Aging Serum → Moisturizer → Zinc Cream

### TC-ST-04: Sort theo Code (Product Code)
**Endpoint:** `GET /api/cosmetics?sort-by=code&sort-order=asc`
**Precondition:**
- Cosmetic A: Code = "PROD-003"
- Cosmetic B: Code = "PROD-001"
- Cosmetic C: Code = "PROD-002"

**Expected Result:**
- Thứ tự: PROD-001 → PROD-002 → PROD-003

### TC-ST-05: Sort theo ID (KHÔNG NÊN)
**Endpoint:** `GET /api/cosmetics?sort-by=id`
**Expected Result:**
- HTTP 400 Bad Request
- Message: "Sorting by ID is not allowed. Use 'code', 'created-at', 'updated-at', or 'name'"

### TC-ST-06: Default Sort (không truyền sort-by)
**Endpoint:** `GET /api/cosmetics`
**Expected Result:**
- Mặc định sort theo created-at DESC
- Cosmetics mới nhất ở đầu

---

## 6. ID vs CODE (Dual Key System)

### TC-DK-01: Tạo Cosmetic - Code phải unique
**Endpoint:** `POST /api/cosmetics`
**Body:**
```json
{
  "cosmeticCode": "PROD-001",
  "cosmeticName": "Test Product",
  ...
}
```
**Precondition:** Code "PROD-001" đã tồn tại
**Expected Result:**
- HTTP 409 Conflict
- Message: "Cosmetic code 'PROD-001' already exists"

### TC-DK-02: ID dùng trong Database relationships
**Scenario:** Khi tạo Foreign Key từ bảng khác
**Expected:**
- FK reference đến CosmeticId (internal ID)
- KHÔNG reference đến CosmeticCode

### TC-DK-03: Code hiển thị cho User
**Endpoint:** `GET /api/cosmetics`
**Expected Response:**
```json
{
  "cosmeticId": "abc-def-123-456",  // Internal, không show ngoài API
  "cosmeticCode": "PROD-001",       // User-friendly, hiển thị UI
  "cosmeticName": "..."
}
```

### TC-DK-04: Update không được đổi Code
**Endpoint:** `PUT /api/cosmetics/{id}`
**Body:**
```json
{
  "cosmeticCode": "PROD-NEW",  // Thử đổi code
  ...
}
```
**Expected Result:**
- HTTP 400 Bad Request
- Message: "Cosmetic code cannot be changed"

### TC-DK-05: Search hỗ trợ cả ID và Code
**Scenario:**
- GET /api/cosmetics/{id} → Search by internal ID
- GET /api/cosmetics/code/{code} → Search by user code

**Expected:**
- Cả 2 endpoint đều hoạt động
- Trả về cùng 1 record

---

## 7. COMBINED SCENARIOS (Test tổng hợp)

### TC-CB-01: Search + Filter + Sort + Paging
**Endpoint:**
```
GET /api/cosmetics?
  search-term=serum
  &category-id=CAT001
  &min-price=50
  &max-price=200
  &sort-by=created-at
  &sort-order=desc
  &page=1
  &page-size=20
```

**Expected Result:**
- Chỉ cosmetics có tên chứa "serum"
- Thuộc CAT001
- Price trong [50, 200]
- Status = 1 (active)
- Sắp xếp theo created-at DESC
- 20 items per page

### TC-CB-02: Soft Delete + Search
**Scenario:**
1. Soft delete cosmetic "PROD-001"
2. Search cosmetic code "PROD-001"

**Expected:**
- GET /api/cosmetics/code/PROD-001 → 404 Not Found
- Cosmetic vẫn tồn tại trong DB nhưng không accessible

### TC-CB-03: Pagination với Soft Deleted Items
**Precondition:**
- 100 total records
- 20 records có Status = 0 (deleted)

**Endpoint:** `GET /api/cosmetics?page-size=50`
**Expected Result:**
- TotalCount = 80 (chỉ đếm active)
- Không show deleted items trong pagination

---

## 8. NEGATIVE TEST CASES

### TC-NG-01: Invalid Sort Field
**Endpoint:** `GET /api/cosmetics?sort-by=invalid_field`
**Expected:** HTTP 400

### TC-NG-02: Invalid Sort Order
**Endpoint:** `GET /api/cosmetics?sort-order=random`
**Expected:** HTTP 400 (chỉ chấp nhận asc/desc)

### TC-NG-03: Negative Price
**Endpoint:** `GET /api/cosmetics?min-price=-100`
**Expected:** HTTP 400

### TC-NG-04: Non-numeric Price
**Endpoint:** `GET /api/cosmetics?min-price=abc`
**Expected:** HTTP 400

### TC-NG-05: Page Number = 0
**Endpoint:** `GET /api/cosmetics?page=0`
**Expected:** HTTP 400 (page bắt đầu từ 1)

---

## POSTMAN COLLECTION STRUCTURE

```
Cosmetics Store API
├── 1. Authentication
│   └── Login
├── 2. Soft Delete
│   ├── Soft Delete Success
│   ├── Get Deleted Item (404)
│   └── List Active Only
├── 3. Search & Filter
│   ├── Search by Name
│   ├── Filter by Price Range
│   ├── Filter by Category ID
│   └── Filter by Category Code
├── 4. Pagination
│   ├── Default Page Size
│   ├── Custom Page Size
│   ├── Exceed Max Page Size
│   └── Multiple Pages
├── 5. Sorting
│   ├── Sort by Created Date
│   ├── Sort by Code
│   ├── Sort by Name
│   └── Invalid Sort Field
└── 6. ID vs Code
    ├── Get by ID
    ├── Get by Code
    ├── Create Duplicate Code
    └── Update Code (Should Fail)
```

## SAMPLE TEST DATA

### Categories
```sql
INSERT INTO CosmeticCategory VALUES 
('CAT001', 'SKINCARE', 'Skin Care', 'Face care', 'Cream', 1, GETUTCDATE(), GETUTCDATE()),
('CAT002', 'MAKEUP', 'Makeup', 'Color cosmetics', 'Powder', 1, GETUTCDATE(), GETUTCDATE());
```

### Cosmetics (Active)
```sql
INSERT INTO CosmeticInformation VALUES
('COS001', 'PROD-001', 'Anti-Aging Serum', 'Dry', '2026-12-31', '30ml', 150.00, 'CAT001', 1, GETUTCDATE(), GETUTCDATE()),
('COS002', 'PROD-002', 'Moisturizer Cream', 'All', '2027-06-30', '50ml', 89.99, 'CAT001', 1, GETUTCDATE(), GETUTCDATE()),
('COS003', 'PROD-003', 'Zinc Sunscreen', 'Sensitive', '2026-08-15', '100ml', 45.00, 'CAT001', 1, GETUTCDATE(), GETUTCDATE());
```

### Cosmetics (Soft Deleted)
```sql
INSERT INTO CosmeticInformation VALUES
('COS004', 'PROD-004', 'Old Product', 'All', '2025-01-01', '30ml', 25.00, 'CAT002', 0, GETUTCDATE(), GETUTCDATE());
```
