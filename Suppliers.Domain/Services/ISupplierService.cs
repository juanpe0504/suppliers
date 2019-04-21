namespace Suppliers.Domain.Services
{
    using Suppliers.Domain.Models;
    using System;
    using System.Collections.Generic;

    public interface ISupplierService
    {
        IList<SupplierDTO> GetAll();

        SupplierDTO Get(int supplierId);

        SupplierRateDTO AddSupplierRate(int supplierId, decimal rate, DateTime startDate, DateTime? endDate);

        IList<SupplierDTO> FindSuppliersBy(int supplierId, decimal rate);
    }
}
