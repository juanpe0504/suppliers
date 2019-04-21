namespace Suppliers.Services.Tests
{
    using Moq;
    using NUnit.Framework;
    using Suppliers.Domain.Exceptions;
    using Suppliers.Domain.Models;
    using Suppliers.Domain.Providers;
    using Suppliers.Services;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SupplierServiceTest
    {
        private SupplierService _supplierService;
        private Mock<ISupplierDao> _mockSupplierDao;

        [SetUp]
        public void Setup()
        {
            _mockSupplierDao = new Mock<ISupplierDao>();
            _supplierService = new SupplierService(_mockSupplierDao.Object);
        }

        #region GetAll

        [Test]
        public void GetAll_ShouldThrowsException()
        {
            _mockSupplierDao
                .Setup(x => x.GetAll())
                .Throws(new Exception("Something went wrong"));

            var ex = Assert.Throws<Exception>(() => _supplierService.GetAll());

            Assert.AreEqual("Something went wrong", ex.Message);
        }

        [Test]
        public void GetAll_ShouldCallDaoAndReturnAEmptyListOfSuppliers()
        {
            _mockSupplierDao
                .Setup(x => x.GetAll())
                .Returns(new List<SupplierDTO>());

            var result = _supplierService.GetAll();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _mockSupplierDao.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GetAll_ShouldCallDaoAndReturnAListOfSuppliers()
        {
            var supplierTest = GenerateSupplier();
            _mockSupplierDao
                .Setup(x => x.GetAll())
                .Returns(new List<SupplierDTO> { supplierTest });

            var result = _supplierService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            _mockSupplierDao.Verify(x => x.GetAll(), Times.Once);
        }

        #endregion

        #region Get

        [Test]
        public void Get_ShouldThrowsException()
        {
            _mockSupplierDao
                .Setup(x => x.Get(It.IsAny<int>()))
                .Throws(new Exception("Something went wrong"));

            var ex = Assert.Throws<Exception>(() => _supplierService.Get(1));

            Assert.AreEqual("Something went wrong", ex.Message);
        }

        [Test]
        public void Get_ShouldCallDaoAndReturnNullWhenSupplierDoesnotExists()
        {
            _mockSupplierDao
                .Setup(x => x.Get(It.IsAny<int>()))
                .Returns<SupplierDTO>(null);

            var result = _supplierService.Get(1);

            Assert.IsNull(result);
            _mockSupplierDao.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Get_ShouldCallDaoAndReturnRequestedSupplier()
        {
            var supplierTest = GenerateSupplier();
            _mockSupplierDao
                .Setup(x => x.Get(It.IsAny<int>()))
                .Returns(supplierTest);

            var result = _supplierService.Get(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SupplierId);
            _mockSupplierDao.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        #endregion

        #region AddSupplierRate 

        [TestCase(1)]
        [TestCase(2)]
        public void AddSupplierRate_ShouldThrowsExceptionWhenStartDateIsGreaterThanEndDate(int endDay)
        {
            var ex = Assert.Throws<ValidationException>(() =>
                _supplierService.AddSupplierRate(1, 10, new DateTime(2015, 8, 2), new DateTime(2015, 8, endDay)));

            Assert.AreEqual("The end date must be greater than start date", ex.Message);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void AddSupplierRate_ShouldThrowsExceptionWhenSupplierDoesnotExists(int inexistentSupplierId)
        {
            _mockSupplierDao.Setup(x => x.Get(inexistentSupplierId)).Returns<SupplierDTO>(null);

            var ex = Assert.Throws<NotFoundException>(() =>
                _supplierService.AddSupplierRate(inexistentSupplierId, 10, new DateTime(2015, 8, 1), null));

            Assert.AreEqual($"The supplier of id {inexistentSupplierId} does not exists", ex.Message);
        }

        [Test]
        public void AddSupplierRate_ShouldThrowsExceptionWhenPeriodsIsOverlappingWithEndDateNull()
        {
            var supplierTest = GenerateSupplier();
            _mockSupplierDao.Setup(x => x.Get(1)).Returns(supplierTest);

            var ex = Assert.Throws<ValidationException>(() =>
                _supplierService.AddSupplierRate(1, 10, new DateTime(2015, 8, 1), null));

            Assert.AreEqual("The supplied date period was overlapped by an existing periods, try another period", ex.Message);
        }

        [Test]
        public void AddSupplierRate_ShouldThrowsExceptionWhenPeriodsIsOverlapping()
        {
            var supplierTest = GenerateSupplier();
            _mockSupplierDao.Setup(x => x.Get(1)).Returns(supplierTest);

            var ex = Assert.Throws<ValidationException>(() =>
                _supplierService.AddSupplierRate(1, 10, new DateTime(2015, 5, 2), new DateTime(2015, 5, 31)));

            Assert.AreEqual("The supplied date period was overlapped by an existing periods, try another period", ex.Message);
        }

        [Test]
        public void AddSupplierRate_ShouldSaveIfHaveNotRatesAssociatedWithNullEndDate()
        {
            var supplierTest = GenerateSupplier();
            supplierTest.SupplierRates = new List<SupplierRateDTO>();
            var expectedRate = new SupplierRateDTO
            {
                SupplierRateId = 1,
                Rate = 10,
                StartDate = new DateTime(2015, 8, 1),
                EndDate = new DateTime(2015, 8, 31)
            };
            _mockSupplierDao.Setup(x => x.Get(It.IsAny<int>())).Returns(supplierTest);
            _mockSupplierDao
                .Setup(x => x.SaveSupplierRate(It.IsAny<int>(), It.IsAny<SupplierRateDTO>()))
                .Returns(expectedRate);

            var result = _supplierService.AddSupplierRate(1, 10, new DateTime(2015, 8, 1), null);

            _mockSupplierDao.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
            _mockSupplierDao.Verify(x => x.SaveSupplierRate(It.IsAny<int>(), It.IsAny<SupplierRateDTO>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRate.SupplierRateId, result.SupplierRateId);
            Assert.AreEqual(expectedRate.Rate, result.Rate);
            Assert.AreEqual(expectedRate.StartDate, result.StartDate);
            Assert.AreEqual(expectedRate.EndDate, result.EndDate);
        }

        [Test]
        public void AddSupplierRate_ShouldSaveIfHaveNotRatesAssociated()
        {
            var supplierTest = GenerateSupplier();
            supplierTest.SupplierRates = new List<SupplierRateDTO>();
            var expectedRate = new SupplierRateDTO
            {
                SupplierRateId = 1,
                Rate = 10,
                StartDate = new DateTime(2015, 8, 1),
                EndDate = new DateTime(2015, 8, 31)
            };
            _mockSupplierDao.Setup(x => x.Get(It.IsAny<int>())).Returns(supplierTest);
            _mockSupplierDao
                .Setup(x => x.SaveSupplierRate(It.IsAny<int>(), It.IsAny<SupplierRateDTO>()))
                .Returns(expectedRate);

            var result = _supplierService.AddSupplierRate(1, 10, new DateTime(2015, 8, 1), new DateTime(2015, 8, 31));

            _mockSupplierDao.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
            _mockSupplierDao.Verify(x => x.SaveSupplierRate(It.IsAny<int>(), It.IsAny<SupplierRateDTO>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRate.SupplierRateId, result.SupplierRateId);
            Assert.AreEqual(expectedRate.Rate, result.Rate);
            Assert.AreEqual(expectedRate.StartDate, result.StartDate);
            Assert.AreEqual(expectedRate.EndDate, result.EndDate);
        }

        [Test]
        public void AddSupplierRate_ShouldSaveWithValidDatePeriods()
        {
            var supplierTest = GenerateSupplier();
            var expectedRate = new SupplierRateDTO
            {
                SupplierRateId = 1,
                Rate = 10,
                StartDate = new DateTime(2015, 8, 1),
                EndDate = new DateTime(2015, 8, 31)
            };
            _mockSupplierDao.Setup(x => x.Get(It.IsAny<int>())).Returns(supplierTest);
            _mockSupplierDao
                .Setup(x => x.SaveSupplierRate(It.IsAny<int>(), It.IsAny<SupplierRateDTO>()))
                .Returns(expectedRate);

            var result = _supplierService.AddSupplierRate(1, 10, new DateTime(2015, 5, 2), new DateTime(2015, 5, 29));

            _mockSupplierDao.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
            _mockSupplierDao.Verify(x => x.SaveSupplierRate(It.IsAny<int>(), It.IsAny<SupplierRateDTO>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRate.SupplierRateId, result.SupplierRateId);
            Assert.AreEqual(expectedRate.Rate, result.Rate);
            Assert.AreEqual(expectedRate.StartDate, result.StartDate);
            Assert.AreEqual(expectedRate.EndDate, result.EndDate);
        }

        #endregion

        #region FindSupplierBy

        [Test]
        public void FindSuppliersBy_ShouldThrowsException()
        {
            _mockSupplierDao
                .Setup(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()))
                .Throws(new Exception("Something went wrong"));

            var ex = Assert.Throws<Exception>(() =>
                _supplierService.FindSuppliersBy(0, 10));

            Assert.AreEqual("Something went wrong", ex.Message);
        }

        [Test]
        public void FindSuppliersBy_ShouldCallDaoAndReturnAEmptyListOfSuppliersIfSupplierAndRateDoesnotMatch()
        {
            _mockSupplierDao
                .Setup(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()))
                .Returns(new List<SupplierDTO>());

            var result = _supplierService.FindSuppliersBy(1, 10);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _mockSupplierDao.Verify(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
        }

        [Test]
        public void FindSuppliersBy_ShouldCallDaoAndReturnAListOfSuppliers()
        {
            var supplierTest = GenerateSupplier();
            _mockSupplierDao
                .Setup(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()))
                .Returns(new List<SupplierDTO> { supplierTest });

            var result = _supplierService.FindSuppliersBy(1, 10);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            _mockSupplierDao.Verify(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
        }

        #endregion

        #region Models

        private static SupplierDTO GenerateSupplier()
        {
            return new SupplierDTO
            {
                SupplierId = 1,
                Name = "ThisIsASupplierName",
                SupplierRates = GenerateSupplierRates()
            };
        }

        private static List<SupplierRateDTO> GenerateSupplierRates()
        {
            return new List<SupplierRateDTO>
            {
                new SupplierRateDTO
                {
                    SupplierRateId = 1,
                    Rate = 10,
                    StartDate = new DateTime(2015,1,1),
                    EndDate = new DateTime(2015, 3, 31)
                },
                new SupplierRateDTO
                {
                    SupplierRateId = 2,
                    Rate = 20,
                    StartDate = new DateTime(2015,4,1),
                    EndDate = new DateTime(2015, 5, 1)
                },
                new SupplierRateDTO
                {
                    SupplierRateId = 3,
                    Rate = 10,
                    StartDate = new DateTime(2015,5,30),
                    EndDate = new DateTime(2015, 7, 25)
                },
                new SupplierRateDTO
                {
                    SupplierRateId = 4,
                    Rate = 25,
                    StartDate = new DateTime(2015,10,1),
                    EndDate = null
                },
            };
        }

        #endregion
    }
}