namespace Suppliers.API.Tests
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using Suppliers.API.Controllers;
    using Suppliers.API.RequestModels;
    using Suppliers.Domain.Exceptions;
    using Suppliers.Domain.Models;
    using Suppliers.Domain.Services;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class SuppliersControllerTest
    {
        private SuppliersController _suppliersController;
        private Mock<ISupplierService> _mockSupplierService;

        [SetUp]
        public void Setup()
        {
            _mockSupplierService = new Mock<ISupplierService>();
            _suppliersController = new SuppliersController(_mockSupplierService.Object);
        }

        #region Get_All

        [Test]
        public void Get_All_ShouldThrowsException()
        {
            _mockSupplierService
                .Setup(x => x.GetAll())
                .Throws(new Exception("Something went wrong"));

            var ex = Assert.Throws<Exception>(() => _suppliersController.Get());

            Assert.AreEqual("Something went wrong", ex.Message);
            _mockSupplierService.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void Get_All_ShouldCallServiceAndReturnEmptyListIfHaveNothing()
        {
            _mockSupplierService
                .Setup(x => x.GetAll())
                .Returns(new List<SupplierDTO>());

            var objectResult = _suppliersController.Get() as OkObjectResult;
            Assert.IsNotNull(objectResult);

            var result = objectResult.Value as IList<SupplierDTO>;

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _mockSupplierService.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void Get_ShouldCallServiceAndReturnAListOfSuppliers()
        {
            var supplierId = 1;
            _mockSupplierService
                .Setup(x => x.GetAll())
                .Returns(new List<SupplierDTO> { new SupplierDTO { SupplierId = supplierId } });

            var objectResult = _suppliersController.Get() as OkObjectResult;
            Assert.IsNotNull(objectResult);

            var result = objectResult.Value as IList<SupplierDTO>;

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(supplierId, result.First().SupplierId);
            _mockSupplierService.Verify(x => x.GetAll(), Times.Once);
        }

        #endregion

        #region Get

        [Test]
        public void Get_ShouldThrowsException()
        {
            _mockSupplierService
                .Setup(x => x.Get(It.IsAny<int>()))
                .Throws(new Exception("Something went wrong"));

            var ex = Assert.Throws<Exception>(() => _suppliersController.Get(1));

            Assert.AreEqual("Something went wrong", ex.Message);
            _mockSupplierService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Get_ShouldThrowsException(int inexistentId)
        {
            _mockSupplierService
                .Setup(x => x.Get(It.IsAny<int>()))
                .Returns<SupplierDTO>(null);

            var ex = Assert.Throws<NotFoundException>(() => _suppliersController.Get(inexistentId));

            Assert.AreEqual($"The supplier of id {inexistentId} does not exists", ex.Message);
            _mockSupplierService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Get_ShouldCallServiceAndReturnRequestedSupplier()
        {
            var supplierId = 1;
            _mockSupplierService
                .Setup(x => x.Get(It.IsAny<int>()))
                .Returns(new SupplierDTO { SupplierId = supplierId });

            var objectResult = _suppliersController.Get(supplierId) as OkObjectResult;
            Assert.IsNotNull(objectResult);

            var result = objectResult.Value as SupplierDTO;

            Assert.IsNotNull(result);
            Assert.AreEqual(supplierId, result.SupplierId);
            _mockSupplierService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        #endregion

        #region Get_Search

        [Test]
        public void Get_Search_ShouldThrowsException()
        {
            _mockSupplierService
                .Setup(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()))
                .Throws(new Exception("Something went wrong"));

            var ex = Assert.Throws<Exception>(() => _suppliersController.Get(1, new SearchSupplier()));

            Assert.AreEqual("Something went wrong", ex.Message);
            _mockSupplierService.Verify(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
        }

        [Test]
        public void Get_Search_ShouldThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => _suppliersController.Get(1, null));
        }

        [Test]
        public void Get_Search_ShouldCallServiceAndReturnEmptyListIfHaveNotData()
        {
            var supplierId = 1;
            _mockSupplierService
                .Setup(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()))
                .Returns(new List<SupplierDTO>());

            var objectResult = _suppliersController.Get(supplierId, new SearchSupplier()) as OkObjectResult;
            Assert.IsNotNull(objectResult);

            var result = objectResult.Value as IList<SupplierDTO>;

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _mockSupplierService.Verify(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
        }

        [Test]
        public void Get_Search_ShouldCallServiceAndReturnListOfSuppliers()
        {
            var supplierId = 1;
            _mockSupplierService
                .Setup(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()))
                .Returns(new List<SupplierDTO>
                {
                    new SupplierDTO
                    {
                        SupplierId = supplierId,
                        SupplierRates = new List<SupplierRateDTO>
                        {
                            new SupplierRateDTO { Rate = 10 }
                        }
                    }
                });

            var objectResult = _suppliersController.Get(supplierId, new SearchSupplier { Rate = 10 }) as OkObjectResult;
            Assert.IsNotNull(objectResult);

            var result = objectResult.Value as IList<SupplierDTO>;

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(supplierId, result.First().SupplierId);
            Assert.IsTrue(result.First().SupplierRates.All(x => x.Rate == 10));

            _mockSupplierService.Verify(x => x.FindSuppliersBy(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
        }

        #endregion

        #region AddSupplierRate

        [Test]
        public void AddSupplierRate_ShouldThrowsException()
        {
            var ex = Assert.Throws<ValidationException>(() => _suppliersController.AddSupplierRate(1, null));
            Assert.AreEqual("The request can not be null", ex.Message);
        }

        [Test]
        public void AddSupplierRate_ShouldCallServiceAndReturnEmptyListIfHaveNotData()
        {
            var supplierId = 1;
            _mockSupplierService
                .Setup(x => x.AddSupplierRate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()))
                .Throws(new Exception("Something went wrong"));

            var ex = Assert.Throws<Exception>(() => _suppliersController.AddSupplierRate(supplierId, new SupplierRateDTO()));
            Assert.AreEqual("Something went wrong", ex.Message);

            _mockSupplierService.Verify(x =>
                x.AddSupplierRate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()), Times.Once);
        }

        [Test]
        public void AddSupplierRate_ShouldCallServiceAndSaveTheSupplierRateWithNullEndDate()
        {
            var supplierId = 1;
            var supplierRate = new SupplierRateDTO
            {
                Rate = 10,
                StartDate = DateTime.Today
            };
            _mockSupplierService
                .Setup(x => x.AddSupplierRate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()))
                .Returns(supplierRate);

            var objectResult = _suppliersController.AddSupplierRate(supplierId, supplierRate) as OkObjectResult;

            Assert.IsNotNull(objectResult);

            var result = objectResult.Value as SupplierRateDTO;

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Rate);
            Assert.AreEqual(DateTime.Today, result.StartDate);
            Assert.IsFalse(result.EndDate.HasValue);

            _mockSupplierService.Verify(x =>
                x.AddSupplierRate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()), Times.Once);
        }

        [Test]
        public void AddSupplierRate_ShouldCallServiceAndSaveTheSupplierRate()
        {
            var supplierId = 1;
            var supplierRate = new SupplierRateDTO
            {
                Rate = 10,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(30)
            };

            _mockSupplierService
                .Setup(x => x.AddSupplierRate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()))
                .Returns(supplierRate);

            var objectResult = _suppliersController.AddSupplierRate(supplierId, supplierRate) as OkObjectResult;

            Assert.IsNotNull(objectResult);

            var result = objectResult.Value as SupplierRateDTO;

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Rate);
            Assert.AreEqual(DateTime.Today, result.StartDate);
            Assert.IsTrue(result.EndDate.HasValue);
            Assert.AreEqual(DateTime.Today.AddDays(30), result.EndDate.Value);

            _mockSupplierService.Verify(x =>
                x.AddSupplierRate(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()), Times.Once);
        }

        #endregion
    }
}