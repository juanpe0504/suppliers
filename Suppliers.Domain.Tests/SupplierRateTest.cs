namespace Suppliers.Domain.Tests
{
    using NUnit.Framework;
    using Suppliers.Domain.Models;
    using System;
    using System.Collections.Generic;

    public class SupplierRateTest
    {
        [Test]
        public void SupplierRate_NewInstanceShouldCreateCorrectlyClean()
        {
            var supplierRate = new SupplierRate();
            Assert.AreEqual(0, supplierRate.SupplierRateId);
            Assert.IsNull(supplierRate.Supplier);
            Assert.AreEqual(0, supplierRate.Rate);
            Assert.AreEqual(DateTime.MinValue, supplierRate.StartDate);
            Assert.IsNull(supplierRate.EndDate);
        }

        [Test]
        public void SupplierRate_NewInstanceShouldCreateCorrectlyWithEndDate()
        {
            var supplierRate = new SupplierRate
            {
                SupplierRateId = 1,
                Rate = 10,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(30),
                Supplier = new Supplier
                {
                    SupplierId = 1,
                    Name = "SuppplierTest"
                }
            };
            supplierRate.Supplier.SupplierRates = new List<SupplierRate> { supplierRate };

            Assert.AreEqual(1, supplierRate.SupplierRateId);
            Assert.AreEqual(10, supplierRate.Rate);
            Assert.AreEqual(DateTime.Today, supplierRate.StartDate);
            Assert.IsNotNull(supplierRate.EndDate);
            Assert.AreEqual(DateTime.Today.AddDays(30), supplierRate.EndDate);
            Assert.IsNotNull(supplierRate.Supplier);
            Assert.AreEqual(1, supplierRate.Supplier.SupplierId);
            Assert.AreEqual("SuppplierTest", supplierRate.Supplier.Name);
        }
    }
}