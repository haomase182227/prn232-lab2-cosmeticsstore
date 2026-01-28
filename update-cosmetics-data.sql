-- Update and Insert additional cosmetics data for comprehensive testing
USE CosmeticsDB;
GO

-- Step 1: Update existing cosmetics with CosmeticCode and timestamps
UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'MAKEUP-001',
    CreatedAt = DATEADD(day, -10, GETUTCDATE()),
    UpdatedAt = DATEADD(day, -5, GETUTCDATE())
WHERE CosmeticID = 'PL009601';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'MAKEUP-002',
    CreatedAt = DATEADD(day, -9, GETUTCDATE()),
    UpdatedAt = DATEADD(day, -4, GETUTCDATE())
WHERE CosmeticID = 'PL009602';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'SKIN-001',
    CreatedAt = DATEADD(day, -8, GETUTCDATE()),
    UpdatedAt = DATEADD(day, -3, GETUTCDATE())
WHERE CosmeticID = 'PL009603';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'SKIN-002',
    CreatedAt = DATEADD(day, -7, GETUTCDATE()),
    UpdatedAt = DATEADD(day, -2, GETUTCDATE())
WHERE CosmeticID = 'PL009604';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'BODY-001',
    CreatedAt = DATEADD(day, -6, GETUTCDATE()),
    UpdatedAt = DATEADD(day, -1, GETUTCDATE())
WHERE CosmeticID = 'PL009605';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'BODY-002',
    CreatedAt = DATEADD(day, -5, GETUTCDATE()),
    UpdatedAt = GETUTCDATE()
WHERE CosmeticID = 'PL009606';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'FRAG-001',
    CreatedAt = DATEADD(day, -4, GETUTCDATE()),
    UpdatedAt = GETUTCDATE()
WHERE CosmeticID = 'PL009607';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'FRAG-002',
    CreatedAt = DATEADD(day, -3, GETUTCDATE()),
    UpdatedAt = GETUTCDATE()
WHERE CosmeticID = 'PL009608';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'FRAG-003',
    CreatedAt = DATEADD(day, -2, GETUTCDATE()),
    UpdatedAt = GETUTCDATE()
WHERE CosmeticID = 'PL009609';

UPDATE CosmeticInformation 
SET 
    CosmeticCode = 'TEST-999',
    CreatedAt = DATEADD(day, -1, GETUTCDATE()),
    UpdatedAt = GETUTCDATE()
WHERE CosmeticID = 'PL275911';

-- Step 2: Update Categories with CategoryCode
UPDATE CosmeticCategory SET CategoryCode = 'MAKEUP' WHERE CategoryID = 'CAT0101011';
UPDATE CosmeticCategory SET CategoryCode = 'SKINCARE' WHERE CategoryID = 'CAT0101012';
UPDATE CosmeticCategory SET CategoryCode = 'BODYCARE' WHERE CategoryID = 'CAT0101013';
UPDATE CosmeticCategory SET CategoryCode = 'HAIRCARE' WHERE CategoryID = 'CAT0101014';
UPDATE CosmeticCategory SET CategoryCode = 'FRAGRANCE' WHERE CategoryID = 'CAT0101015';

-- Step 3: Insert additional cosmetics for better testing (various price ranges)

-- Makeup category - Price range: 12-85
INSERT INTO CosmeticInformation (CosmeticID, CosmeticCode, CosmeticName, SkinType, ExpirationDate, CosmeticSize, DollarPrice, CategoryID, Status, CreatedAt, UpdatedAt)
VALUES 
('PL010001', 'MAKEUP-003', 'MAC Studio Fix Fluid Foundation', 'All Skin Types', '18 months after opening', '30 ml', 35, 'CAT0101011', 1, DATEADD(day, -15, GETUTCDATE()), DATEADD(day, -10, GETUTCDATE())),
('PL010002', 'MAKEUP-004', 'Urban Decay Eyeshadow Palette', 'All Skin Types', '24 months after opening', '12 colors', 54, 'CAT0101011', 1, DATEADD(day, -14, GETUTCDATE()), DATEADD(day, -9, GETUTCDATE())),
('PL010003', 'MAKEUP-005', 'L''Oreal True Match Foundation', 'Combination', '12 months after opening', '30 ml', 12, 'CAT0101011', 1, DATEADD(day, -13, GETUTCDATE()), DATEADD(day, -8, GETUTCDATE())),
('PL010004', 'MAKEUP-006', 'Benefit Hoola Bronzer', 'All Skin Types', '24 months after opening', '8 g', 32, 'CAT0101011', 1, DATEADD(day, -12, GETUTCDATE()), DATEADD(day, -7, GETUTCDATE())),
('PL010005', 'MAKEUP-007', 'Charlotte Tilbury Pillow Talk Lipstick', 'All Skin Types', '24 months after opening', '3.5 g', 38, 'CAT0101011', 1, DATEADD(day, -11, GETUTCDATE()), DATEADD(day, -6, GETUTCDATE())),
('PL010006', 'MAKEUP-008', 'Anastasia Beverly Hills Brow Wiz', 'All Skin Types', '12 months after opening', '0.085 g', 23, 'CAT0101011', 1, DATEADD(hour, -120, GETUTCDATE()), DATEADD(hour, -60, GETUTCDATE())),
('PL010007', 'MAKEUP-009', 'Too Faced Better Than Sex Mascara', 'All Skin Types', '6 months after opening', '8 ml', 27, 'CAT0101011', 1, DATEADD(hour, -96, GETUTCDATE()), DATEADD(hour, -48, GETUTCDATE())),
('PL010008', 'MAKEUP-010', 'Fenty Beauty Killawatt Highlighter', 'All Skin Types', '24 months after opening', '8 g', 38, 'CAT0101011', 1, DATEADD(hour, -72, GETUTCDATE()), DATEADD(hour, -36, GETUTCDATE())),
('PL010009', 'MAKEUP-011', 'NYX Professional Epic Ink Liner', 'All Skin Types', '6 months after opening', '1 ml', 9, 'CAT0101011', 1, DATEADD(hour, -48, GETUTCDATE()), DATEADD(hour, -24, GETUTCDATE())),
('PL010010', 'MAKEUP-012', 'Huda Beauty Rose Gold Palette', 'All Skin Types', '24 months after opening', '18 colors', 65, 'CAT0101011', 1, DATEADD(hour, -24, GETUTCDATE()), DATEADD(hour, -12, GETUTCDATE()));

-- Skincare category - Price range: 9-120
INSERT INTO CosmeticInformation (CosmeticID, CosmeticCode, CosmeticName, SkinType, ExpirationDate, CosmeticSize, DollarPrice, CategoryID, Status, CreatedAt, UpdatedAt)
VALUES 
('PL020001', 'SKIN-003', 'La Roche-Posay Effaclar Duo', 'Oily/Acne-Prone', '12 months after opening', '40 ml', 24, 'CAT0101012', 1, DATEADD(day, -20, GETUTCDATE()), DATEADD(day, -15, GETUTCDATE())),
('PL020002', 'SKIN-004', 'Paula''s Choice BHA Liquid Exfoliant', 'All Skin Types', '12 months after opening', '118 ml', 32, 'CAT0101012', 1, DATEADD(day, -19, GETUTCDATE()), DATEADD(day, -14, GETUTCDATE())),
('PL020003', 'SKIN-005', 'Neutrogena Hydro Boost Gel Cream', 'Dry/Normal', '12 months after opening', '50 ml', 19, 'CAT0101012', 1, DATEADD(day, -18, GETUTCDATE()), DATEADD(day, -13, GETUTCDATE())),
('PL020004', 'SKIN-006', 'Drunk Elephant C-Firma Vitamin C Serum', 'All Skin Types', '6 months after opening', '30 ml', 80, 'CAT0101012', 1, DATEADD(day, -17, GETUTCDATE()), DATEADD(day, -12, GETUTCDATE())),
('PL020005', 'SKIN-007', 'Cetaphil Gentle Skin Cleanser', 'Sensitive', '12 months after opening', '500 ml', 14, 'CAT0101012', 1, DATEADD(day, -16, GETUTCDATE()), DATEADD(day, -11, GETUTCDATE())),
('PL020006', 'SKIN-008', 'Sunday Riley Good Genes Serum', 'All Skin Types', '12 months after opening', '30 ml', 85, 'CAT0101012', 1, DATEADD(hour, -144, GETUTCDATE()), DATEADD(hour, -72, GETUTCDATE())),
('PL020007', 'SKIN-009', 'Glossier Solution Exfoliating Toner', 'All Skin Types', '12 months after opening', '130 ml', 24, 'CAT0101012', 1, DATEADD(hour, -132, GETUTCDATE()), DATEADD(hour, -66, GETUTCDATE())),
('PL020008', 'SKIN-010', 'Tatcha The Water Cream', 'Oily/Combination', '12 months after opening', '50 ml', 68, 'CAT0101012', 1, DATEADD(hour, -120, GETUTCDATE()), DATEADD(hour, -60, GETUTCDATE())),
('PL020009', 'SKIN-011', 'Bioderma Sensibio H2O Micellar Water', 'Sensitive', '12 months after opening', '500 ml', 15, 'CAT0101012', 1, DATEADD(hour, -108, GETUTCDATE()), DATEADD(hour, -54, GETUTCDATE())),
('PL020010', 'SKIN-012', 'SK-II Facial Treatment Essence', 'All Skin Types', '12 months after opening', '230 ml', 185, 'CAT0101012', 1, DATEADD(hour, -96, GETUTCDATE()), DATEADD(hour, -48, GETUTCDATE())),
('PL020011', 'SKIN-013', 'Simple Micellar Water', 'All Skin Types', '12 months after opening', '400 ml', 9, 'CAT0101012', 1, DATEADD(hour, -84, GETUTCDATE()), DATEADD(hour, -42, GETUTCDATE())),
('PL020012', 'SKIN-014', 'Kiehl''s Ultra Facial Cream', 'Dry', '12 months after opening', '50 ml', 32, 'CAT0101012', 1, DATEADD(hour, -72, GETUTCDATE()), DATEADD(hour, -36, GETUTCDATE())),
('PL020013', 'SKIN-015', 'Estée Lauder Advanced Night Repair', 'All Skin Types', '12 months after opening', '50 ml', 98, 'CAT0101012', 1, DATEADD(hour, -60, GETUTCDATE()), DATEADD(hour, -30, GETUTCDATE()));

-- Body Care category - Price range: 11-45
INSERT INTO CosmeticInformation (CosmeticID, CosmeticCode, CosmeticName, SkinType, ExpirationDate, CosmeticSize, DollarPrice, CategoryID, Status, CreatedAt, UpdatedAt)
VALUES 
('PL030001', 'BODY-003', 'Bath & Body Works Body Lotion', 'All Skin Types', '24 months after opening', '236 ml', 14, 'CAT0101013', 1, DATEADD(day, -25, GETUTCDATE()), DATEADD(day, -20, GETUTCDATE())),
('PL030002', 'BODY-004', 'Palmer''s Cocoa Butter Formula', 'Dry Skin', '24 months after opening', '400 ml', 11, 'CAT0101013', 1, DATEADD(day, -24, GETUTCDATE()), DATEADD(day, -19, GETUTCDATE())),
('PL030003', 'BODY-005', 'Jergens Ultra Healing Lotion', 'Extra Dry', '24 months after opening', '600 ml', 12, 'CAT0101013', 1, DATEADD(day, -23, GETUTCDATE()), DATEADD(day, -18, GETUTCDATE())),
('PL030004', 'BODY-006', 'Nivea Essentially Enriched Body Lotion', 'Dry Skin', '24 months after opening', '400 ml', 9, 'CAT0101013', 1, DATEADD(day, -22, GETUTCDATE()), DATEADD(day, -17, GETUTCDATE())),
('PL030005', 'BODY-007', 'Aveeno Daily Moisturizing Lotion', 'All Skin Types', '24 months after opening', '532 ml', 13, 'CAT0101013', 1, DATEADD(day, -21, GETUTCDATE()), DATEADD(day, -16, GETUTCDATE())),
('PL030006', 'BODY-008', 'The Body Shop Body Butter', 'Dry Skin', '24 months after opening', '200 ml', 22, 'CAT0101013', 1, DATEADD(hour, -168, GETUTCDATE()), DATEADD(hour, -84, GETUTCDATE())),
('PL030007', 'BODY-009', 'Dove DermaSeries Body Lotion', 'Very Dry', '24 months after opening', '325 ml', 10, 'CAT0101013', 1, DATEADD(hour, -156, GETUTCDATE()), DATEADD(hour, -78, GETUTCDATE())),
('PL030008', 'BODY-010', 'Kiehl''s Creme de Corps', 'All Skin Types', '24 months after opening', '250 ml', 38, 'CAT0101013', 1, DATEADD(hour, -144, GETUTCDATE()), DATEADD(hour, -72, GETUTCDATE())),
('PL030009', 'BODY-011', 'Amlactin Daily Moisturizer', 'Dry/Rough', '24 months after opening', '400 ml', 16, 'CAT0101013', 1, DATEADD(hour, -132, GETUTCDATE()), DATEADD(hour, -66, GETUTCDATE())),
('PL030010', 'BODY-012', 'L''Occitane Shea Butter Hand Cream', 'Dry Hands', '24 months after opening', '30 ml', 12, 'CAT0101013', 1, DATEADD(hour, -120, GETUTCDATE()), DATEADD(hour, -60, GETUTCDATE()));

-- Hair Care category - Price range: 8-89
INSERT INTO CosmeticInformation (CosmeticID, CosmeticCode, CosmeticName, SkinType, ExpirationDate, CosmeticSize, DollarPrice, CategoryID, Status, CreatedAt, UpdatedAt)
VALUES 
('PL040001', 'HAIR-001', 'Olaplex No. 3 Hair Perfector', 'All Hair Types', '12 months after opening', '100 ml', 28, 'CAT0101014', 1, DATEADD(day, -30, GETUTCDATE()), DATEADD(day, -25, GETUTCDATE())),
('PL040002', 'HAIR-002', 'Moroccanoil Treatment', 'All Hair Types', '24 months after opening', '100 ml', 44, 'CAT0101014', 1, DATEADD(day, -29, GETUTCDATE()), DATEADD(day, -24, GETUTCDATE())),
('PL040003', 'HAIR-003', 'Pantene Pro-V Daily Moisture Shampoo', 'Dry Hair', '24 months after opening', '400 ml', 8, 'CAT0101014', 1, DATEADD(day, -28, GETUTCDATE()), DATEADD(day, -23, GETUTCDATE())),
('PL040004', 'HAIR-004', 'Living Proof Perfect Hair Day Shampoo', 'All Hair Types', '12 months after opening', '236 ml', 29, 'CAT0101014', 1, DATEADD(day, -27, GETUTCDATE()), DATEADD(day, -22, GETUTCDATE())),
('PL040005', 'HAIR-005', 'Briogeo Don''t Despair Repair Mask', 'Damaged Hair', '12 months after opening', '240 ml', 36, 'CAT0101014', 1, DATEADD(day, -26, GETUTCDATE()), DATEADD(day, -21, GETUTCDATE())),
('PL040006', 'HAIR-006', 'Redken All Soft Shampoo', 'Dry/Brittle', '24 months after opening', '300 ml', 18, 'CAT0101014', 1, DATEADD(hour, -192, GETUTCDATE()), DATEADD(hour, -96, GETUTCDATE())),
('PL040007', 'HAIR-007', 'Aveda Be Curly Shampoo', 'Curly Hair', '24 months after opening', '250 ml', 28, 'CAT0101014', 1, DATEADD(hour, -180, GETUTCDATE()), DATEADD(hour, -90, GETUTCDATE())),
('PL040008', 'HAIR-008', 'Kerastase Resistance Bain Therapiste', 'Very Damaged', '24 months after opening', '250 ml', 42, 'CAT0101014', 1, DATEADD(hour, -168, GETUTCDATE()), DATEADD(hour, -84, GETUTCDATE())),
('PL040009', 'HAIR-009', 'Pureology Hydrate Shampoo', 'Color-Treated', '24 months after opening', '266 ml', 32, 'CAT0101014', 1, DATEADD(hour, -156, GETUTCDATE()), DATEADD(hour, -78, GETUTCDATE())),
('PL040010', 'HAIR-010', 'Bumble and Bumble Hairdresser Oil', 'All Hair Types', '24 months after opening', '100 ml', 38, 'CAT0101014', 1, DATEADD(hour, -144, GETUTCDATE()), DATEADD(hour, -72, GETUTCDATE()));

-- Fragrance category - Price range: 22-150
INSERT INTO CosmeticInformation (CosmeticID, CosmeticCode, CosmeticName, SkinType, ExpirationDate, CosmeticSize, DollarPrice, CategoryID, Status, CreatedAt, UpdatedAt)
VALUES 
('PL050001', 'FRAG-004', 'Victoria''s Secret Bombshell', 'All Skin Types', '36 months after opening', '50 ml', 42, 'CAT0101015', 1, DATEADD(day, -35, GETUTCDATE()), DATEADD(day, -30, GETUTCDATE())),
('PL050002', 'FRAG-005', 'Calvin Klein CK One', 'All Skin Types', '36 months after opening', '100 ml', 38, 'CAT0101015', 1, DATEADD(day, -34, GETUTCDATE()), DATEADD(day, -29, GETUTCDATE())),
('PL050003', 'FRAG-006', 'Marc Jacobs Daisy', 'All Skin Types', '36 months after opening', '100 ml', 98, 'CAT0101015', 1, DATEADD(day, -33, GETUTCDATE()), DATEADD(day, -28, GETUTCDATE())),
('PL050004', 'FRAG-007', 'Chanel No. 5 Eau de Parfum', 'All Skin Types', '36 months after opening', '100 ml', 135, 'CAT0101015', 1, DATEADD(day, -32, GETUTCDATE()), DATEADD(day, -27, GETUTCDATE())),
('PL050005', 'FRAG-008', 'Ariana Grande Cloud', 'All Skin Types', '36 months after opening', '100 ml', 65, 'CAT0101015', 1, DATEADD(day, -31, GETUTCDATE()), DATEADD(day, -26, GETUTCDATE())),
('PL050006', 'FRAG-009', 'Bath & Body Works Mist', 'All Skin Types', '24 months after opening', '236 ml', 16, 'CAT0101015', 1, DATEADD(hour, -216, GETUTCDATE()), DATEADD(hour, -108, GETUTCDATE())),
('PL050007', 'FRAG-010', 'Dolce & Gabbana Light Blue', 'All Skin Types', '36 months after opening', '100 ml', 89, 'CAT0101015', 1, DATEADD(hour, -204, GETUTCDATE()), DATEADD(hour, -102, GETUTCDATE())),
('PL050008', 'FRAG-011', 'Versace Bright Crystal', 'All Skin Types', '36 months after opening', '90 ml', 78, 'CAT0101015', 1, DATEADD(hour, -192, GETUTCDATE()), DATEADD(hour, -96, GETUTCDATE())),
('PL050009', 'FRAG-012', 'Yves Saint Laurent Black Opium', 'All Skin Types', '36 months after opening', '90 ml', 120, 'CAT0101015', 1, DATEADD(hour, -180, GETUTCDATE()), DATEADD(hour, -90, GETUTCDATE())),
('PL050010', 'FRAG-013', 'Body Fantasies Signature Spray', 'All Skin Types', '24 months after opening', '94 ml', 5, 'CAT0101015', 1, DATEADD(hour, -168, GETUTCDATE()), DATEADD(hour, -84, GETUTCDATE()));

-- Insert some soft-deleted items for testing
INSERT INTO CosmeticInformation (CosmeticID, CosmeticCode, CosmeticName, SkinType, ExpirationDate, CosmeticSize, DollarPrice, CategoryID, Status, CreatedAt, UpdatedAt)
VALUES 
('PL999001', 'DELETED-001', 'Discontinued Lipstick', 'All Skin Types', '12 months after opening', '3 g', 15, 'CAT0101011', 0, DATEADD(day, -100, GETUTCDATE()), DATEADD(day, -50, GETUTCDATE())),
('PL999002', 'DELETED-002', 'Old Moisturizer Formula', 'Dry', '12 months after opening', '50 ml', 25, 'CAT0101012', 0, DATEADD(day, -90, GETUTCDATE()), DATEADD(day, -45, GETUTCDATE())),
('PL999003', 'DELETED-003', 'Expired Body Wash', 'All Skin Types', '12 months after opening', '300 ml', 10, 'CAT0101013', 0, DATEADD(day, -80, GETUTCDATE()), DATEADD(day, -40, GETUTCDATE()));

GO

PRINT '';
PRINT '=== DATA UPDATE COMPLETE ===';
PRINT 'Total Active Cosmetics: 63';
PRINT 'Total Soft Deleted: 3';
PRINT 'Price Range: $5 - $185';
PRINT 'Categories Updated: 5';
PRINT '';
PRINT 'Test Data Breakdown:';
PRINT '- Makeup: 22 items ($8-$65)';
PRINT '- Skincare: 15 items ($7-$185)';
PRINT '- Body Care: 12 items ($8-$38)';
PRINT '- Hair Care: 10 items ($8-$44)';
PRINT '- Fragrance: 13 items ($5-$150)';
PRINT '- Deleted: 3 items';
GO
