namespace Supplier.Providers.Tests
{
    using NUnit.Framework;
    using Suppliers.Domain.Exceptions;
    using Suppliers.Domain.Models;
    using Suppliers.Providers;
    using System;
    using System.Linq;

    public class SupplierDaoTest
    {
        private SupplierDao _supplierDao;

        [SetUp]
        public void Setup()
        {
            _supplierDao = new SupplierDao();
        }

        #region GetAll

        [Test]
        public void GetAll_SupplierShouldReturnListOfValues()
        {
            var result = _supplierDao.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            #region Supplier A

            var supplierA = result[0];
            var supplierA_firstRate = supplierA.SupplierRates[0];
            var supplierA_secondRate = supplierA.SupplierRates[1];
            var supplierA_thridRate = supplierA.SupplierRates[2];
            var supplierA_quarterRate = supplierA.SupplierRates[3];

            Assert.IsNotEmpty(supplierA.SupplierRates);
            Assert.AreEqual(1, supplierA.SupplierId);
            Assert.AreEqual("Supplier A", supplierA.Name);

            Assert.AreEqual(1, supplierA_firstRate.SupplierRateId);
            Assert.AreEqual(10, supplierA_firstRate.Rate);
            Assert.AreEqual(new DateTime(2015, 1, 1), supplierA_firstRate.StartDate);
            Assert.AreEqual(new DateTime(2015, 3, 31), supplierA_firstRate.EndDate);

            Assert.AreEqual(2, supplierA_secondRate.SupplierRateId);
            Assert.AreEqual(20, supplierA_secondRate.Rate);
            Assert.AreEqual(new DateTime(2015, 4, 1), supplierA_secondRate.StartDate);
            Assert.AreEqual(new DateTime(2015, 5, 1), supplierA_secondRate.EndDate);

            Assert.AreEqual(3, supplierA_thridRate.SupplierRateId);
            Assert.AreEqual(10, supplierA_thridRate.Rate);
            Assert.AreEqual(new DateTime(2015, 5, 30), supplierA_thridRate.StartDate);
            Assert.AreEqual(new DateTime(2015, 7, 25), supplierA_thridRate.EndDate);

            Assert.AreEqual(4, supplierA_quarterRate.SupplierRateId);
            Assert.AreEqual(25, supplierA_quarterRate.Rate);
            Assert.AreEqual(new DateTime(2015, 10, 1), supplierA_quarterRate.StartDate);
            Assert.AreEqual(null, supplierA_quarterRate.EndDate);

            #endregion

            #region Supplier B

            var supplierB = result[1];
            var supplierB_firstRate = supplierB.SupplierRates[0];
            
            Assert.IsNotEmpty(supplierB.SupplierRates);
            Assert.AreEqual(2, supplierB.SupplierId);
            Assert.AreEqual("Supplier B", supplierB.Name);

            Assert.AreEqual(5, supplierB_firstRate.SupplierRateId);
            Assert.AreEqual(100, supplierB_firstRate.Rate);
            Assert.AreEqual(new DateTime(2016, 11, 1), supplierB_firstRate.StartDate);
            Assert.AreEqual(null, supplierB_firstRate.EndDate);

            #endregion

            #region Supplier C

            var supplierC = result[2];
            var supplierC_firstRate = supplierC.SupplierRates[0];
            var supplierC_secondRate = supplierC.SupplierRates[1];

            Assert.IsNotEmpty(supplierC.SupplierRates);
            Assert.AreEqual(3, supplierC.SupplierId);
            Assert.AreEqual("Supplier C", supplierC.Name);

            Assert.AreEqual(6, supplierC_firstRate.SupplierRateId);
            Assert.AreEqual(30, supplierC_firstRate.Rate);
            Assert.AreEqual(new DateTime(2016, 12, 1), supplierC_firstRate.StartDate);
            Assert.AreEqual(new DateTime(2017, 1, 1), supplierC_firstRate.EndDate);

            Assert.AreEqual(7, supplierC_secondRate.SupplierRateId);
            Assert.AreEqual(30, supplierC_secondRate.Rate);
            Assert.AreEqual(new DateTime(2017, 1, 2), supplierC_secondRate.StartDate);
            Assert.AreEqual(null, supplierC_secondRate.EndDate);

            #endregion
        }

        #endregion

        #region Get

        [TestCase(-1)]
        [TestCase(0)]
        public void Get_ShouldReturnNullForInvalidInexistentSupplierId(int inexistentId)
        {
            var result = _supplierDao.Get(inexistentId);
            Assert.IsNull(result);
        }

        [Test]
        public void Get_SupplierShouldHaveCorrectValues()
        {
            var result = _supplierDao.Get(1);
            var first = result.SupplierRates[0];
            var second = result.SupplierRates[1];
            var thrid = result.SupplierRates[2];
            var quarter = result.SupplierRates[3];

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.SupplierRates);
            Assert.AreEqual(1, result.SupplierId);
            Assert.AreEqual("Supplier A", result.Name);
            
            Assert.AreEqual(10, first.Rate);
            Assert.AreEqual(new DateTime(2015, 1, 1), first.StartDate);
            Assert.AreEqual(new DateTime(2015, 3, 31), first.EndDate);

            Assert.AreEqual(20, second.Rate);
            Assert.AreEqual(new DateTime(2015, 4, 1), second.StartDate);
            Assert.AreEqual(new DateTime(2015, 5, 1), second.EndDate);

            Assert.AreEqual(10, thrid.Rate);
            Assert.AreEqual(new DateTime(2015, 5, 30), thrid.StartDate);
            Assert.AreEqual(new DateTime(2015, 7, 25), thrid.EndDate);

            Assert.AreEqual(25, quarter.Rate);
            Assert.AreEqual(new DateTime(2015, 10, 1), quarter.StartDate);
            Assert.AreEqual(null, quarter.EndDate);
        }

        #endregion

        #region FindSuppliersBy

        [TestCase(-1, 10)]
        [TestCase(0, 10)]
        [TestCase(1, 100)]
        [TestCase(2, 10)]
        [TestCase(3, 20)]
        public void FindSuppliersBy_ShouldReturnEmptyList(int supplierId, decimal rate)
        {
            var result = _supplierDao.FindSuppliersBy(supplierId, rate);
            Assert.IsEmpty(result);
        }

        [TestCase(1, "Supplier A", 10)]
        [TestCase(1, "Supplier A", 20)]
        [TestCase(2, "Supplier B", 100)]
        [TestCase(3, "Supplier C", 30)]
        public void FindSuppliersBy_ShouldReturnCorrectSuppliers(int supplierId, string expectedName, decimal rate)
        {
            var result = _supplierDao.FindSuppliersBy(supplierId, rate);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(supplierId, result.First().SupplierId);
            Assert.AreEqual(expectedName, result.First().Name);
        }

        #endregion

        #region SaveSupplierRate

        [Test]
        public void SaveSupplierRate_ShouldThrowsExceptionWhenSupplierRateIsNull()
        {
            Assert.Throws<NullReferenceException>(() => _supplierDao.SaveSupplierRate(1, null));
        }

        [Test]
        public void SaveSupplierRate_ShouldThrowsExceptionWhenSupplierIdDoesnotExists()
        {
            var supplierRate = new SupplierRateDTO
            {
                Rate = 10,
                StartDate = new DateTime(2015, 8, 1),
                EndDate = new DateTime(2015, 8, 31)
            };

            Assert.Throws<NotFoundException>(() => _supplierDao.SaveSupplierRate(0, supplierRate));
        }

        [Test]
        public void SaveSupplierRate_ShouldSaveCorrectly()
        {
            var supplierRate = new SupplierRateDTO
            {
                Rate = 10,
                StartDate = new DateTime(2015, 8, 1),
                EndDate = new DateTime(2015, 8, 31)
            };

            var result = _supplierDao.SaveSupplierRate(1, supplierRate);
            
            Assert.AreEqual(8, result.SupplierRateId);
            Assert.AreEqual(supplierRate.Rate, result.Rate);
            Assert.AreEqual(supplierRate.StartDate, result.StartDate);
            Assert.AreEqual(supplierRate.EndDate, result.EndDate);
        }

        [Test]
        public void SaveSupplierRate_ShouldSaveCorrectlyWithEndDateNull()
        {
            var supplierRate = new SupplierRateDTO
            {
                Rate = 10,
                StartDate = new DateTime(2015, 8, 1)
            };

            var result = _supplierDao.SaveSupplierRate(1, supplierRate);

            Assert.AreEqual(8, result.SupplierRateId);
            Assert.AreEqual(supplierRate.Rate, result.Rate);
            Assert.AreEqual(supplierRate.StartDate, result.StartDate);
            Assert.IsNull(result.EndDate);
        }

        #endregion
    }
}