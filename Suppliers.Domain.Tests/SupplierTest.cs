namespace Tests
{
    using NUnit.Framework;
    using Suppliers.Domain.Models;
    using System.Collections.Generic;

    public class SupplierTest
    {
        [Test]
        public void Supplier_NewInstanceShouldCreateCorrectlyClean()
        {
            var supplier = new Supplier();
            Assert.AreEqual(0, supplier.SupplierId);
            Assert.IsNull(supplier.Name);
            Assert.IsNull(supplier.SupplierRates);
        }

        [Test]
        public void Supplier_NewInstanceShouldCreateCorrectlyWithoutRates()
        {
            var supplier = new Supplier
            {
                SupplierId = 1,
                Name = "SupplierTest"
            };
            Assert.AreEqual(1, supplier.SupplierId);
            Assert.AreEqual("SupplierTest", supplier.Name);
            Assert.IsNull(supplier.SupplierRates);
        }

        [Test]
        public void Supplier_NewInstanceShouldCreateCorrectlyWithEmptyListRates()
        {
            var supplier = new Supplier
            {
                SupplierId = 1,
                Name = "SupplierTest",
                SupplierRates = new List<SupplierRate>()
            };

            Assert.AreEqual(1, supplier.SupplierId);
            Assert.AreEqual("SupplierTest", supplier.Name);
            Assert.IsNotNull(supplier.SupplierRates);
            Assert.IsEmpty(supplier.SupplierRates);
        }

        [Test]
        public void Supplier_NewInstanceShouldCreateCorrectlyWithListRates()
        {
            var supplier = new Supplier
            {
                SupplierId = 1,
                Name = "SupplierTest",
                SupplierRates = new List<SupplierRate>() { new SupplierRate() }
            };

            Assert.AreEqual(1, supplier.SupplierId);
            Assert.AreEqual("SupplierTest", supplier.Name);
            Assert.IsNotNull(supplier.SupplierRates);
            Assert.IsNotEmpty(supplier.SupplierRates);
            Assert.AreEqual(1, supplier.SupplierRates.Count);
        }
    }
}