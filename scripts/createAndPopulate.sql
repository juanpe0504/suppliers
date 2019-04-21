--Create Supplier Table
CREATE TABLE Supplier (
	SupplierId int IDENTITY(1,1) NOT NULL,
	Name nvarchar(100) NOT NULL,
	CONSTRAINT PK_Supplier PRIMARY KEY CLUSTERED 
	(
		SupplierId ASC
	)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
  ) ON [PRIMARY]
) ON [PRIMARY]
GO

--Create SupplierRate Table
CREATE TABLE SupplierRate (
	SupplierRateId int IDENTITY(1,1) NOT NULL,
	SupplierId int NOT NULL,
	Rate decimal(18, 0) NULL,
	StartDate date NOT NULL,
	EndDate date NULL,
	CONSTRAINT PK_SupplierRate PRIMARY KEY CLUSTERED 
	(
		SupplierRateId ASC
	)
	WITH (
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE dbo.SupplierRate  WITH CHECK ADD CONSTRAINT FK_SupplierRate_SupplierRate FOREIGN KEY(SupplierId)
REFERENCES dbo.Supplier (SupplierId)
GO

ALTER TABLE dbo.SupplierRate CHECK CONSTRAINT FK_SupplierRate_SupplierRate
GO

--Base Insertions
INSERT INTO Supplier ([Name])
VALUES	('Supplier A'),
		('Supplier B'),
		('Supplier C');

INSERT INTO SupplierRate ([SupplierId], [Rate], [StartDate], [EndDate])
VALUES	(1, 10, '2015/01/01', '2015/03/31'),
		(1, 20, '2015/04/01', '2015/05/01'),
		(1, 10, '2015/05/30', '2015/07/25'),
		(1, 25, '2015/10/01', null),

		(2, 100, '2016/11/01', null),

		(3, 30, '2016/12/01', '2017/01/01'),
		(3, 30, '2017/01/02', null);

GO