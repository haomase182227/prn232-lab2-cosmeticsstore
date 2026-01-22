-- Seed data for CosmeticsStore Database
SET QUOTED_IDENTIFIER ON;
GO

USE CosmeticsDB;
GO

-- Clear existing data
DELETE FROM CosmeticInformation;
DELETE FROM CosmeticCategory;
DELETE FROM SystemAccount;
GO

-- Insert System Accounts with IDENTITY_INSERT
SET IDENTITY_INSERT SystemAccount ON;
GO

INSERT INTO SystemAccount (AccountID, AccountPassword, EmailAddress, AccountNote, Role)
VALUES 
(551, '@1', 'admin@CosmeticsDB.info', 'System Admin', 1),
(552, '@1', 'staff@CosmeticsDB.info', 'Staff', 3),
(553, '@1', 'manager@CosmeticsDB.info', 'Manager', 2),
(554, '@1', 'member1@CosmeticsDB.info', 'Member 1', 4);
GO

SET IDENTITY_INSERT SystemAccount OFF;
GO

-- Insert Categories
INSERT INTO CosmeticCategory (CategoryID, CategoryName, UsagePurpose, FormulationType)
VALUES 
('CAT0101011', 'Makeup', 'To enhance facial features and improve appearance...', 'Includes products like foundations, concealers, eyesh...'),
('CAT0101012', 'Skincare', 'To cleanse, moisturize, protect, and improve the he...', 'Comprises cleansers, toners, serums, moisturizers, a...'),
('CAT0101013', 'Body Care', 'To maintain and improve the health and appearan...', 'Includes body lotions, creams, scrubs, and oils, typica...'),
('CAT0101014', 'Hair Care', 'To cleanse, condition, and style hair, promoting ove...', 'Encompasses shampoos, conditioners, hair masks, ...'),
('CAT0101015', 'Fragrance', 'To provide a pleasant scent and enhance personal ...', 'Includes perfumes, colognes, and body sprays, typica...');
GO

-- Insert Cosmetic Information
INSERT INTO CosmeticInformation (CosmeticID, CosmeticName, SkinType, ExpirationDate, CosmeticSize, DollarPrice, CategoryID)
VALUES 
('PL009601', 'Maybelline Fit Me Matte + Poreless Foundation', 'Oily/Combination', '12 months after opening', '30 ml', 8, 'CAT0101011'),
('PL009602', 'NARS Blush', 'All Skin Types', '24 months after opening', '4.8 g', 30, 'CAT0101011'),
('PL009603', 'CeraVe Hydrating Facial Cleanser', 'Normal/Dry', '12 months after opening', '355 ml', 16, 'CAT0101012'),
('PL009604', 'The Ordinary Niacinamide 10% + Zinc 1%', 'Oily/Acne-Prone', '12 months after opening', '30 ml', 7, 'CAT0101012'),
('PL009605', 'Vaseline Intensive Care Lotion', 'Dry Skin', '24 months after opening', '400 ml', 8, 'CAT0101013'),
('PL009606', 'Neutrogena Hydro Boost Water Gel', 'All Skin Types', '12 months after opening', '50 ml', 20, 'CAT0101013'),
('PL009607', 'Lush Sugar Plum Fairy Shower Gel', 'All Skin Types', '12 months after opening', '250 g', 15, 'CAT0101015'),
('PL009608', 'St. Ives Fresh Skin Apricot Scrub', 'All Skin Types', '24 months after opening', '170 g', 6, 'CAT0101015'),
('PL009609', 'Eucerin Advanced Repair Cream', 'Dry Skin', '24 months after opening', '340 g', 15, 'CAT0101015'),
('PL275911', 'iPhone17promax', 'Oily/Combination', '12 months after opening', '30 ml', 8, 'CAT0101011');
GO

PRINT 'Data seeded successfully!';
PRINT 'System Accounts: 4';
PRINT 'Categories: 5';
PRINT 'Cosmetics: 10';
GO
