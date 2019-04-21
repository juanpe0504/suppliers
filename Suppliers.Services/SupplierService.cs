namespace Suppliers.Services
{
    using Suppliers.Domain.Exceptions;
    using Suppliers.Domain.Models;
    using Suppliers.Domain.Providers;
    using Suppliers.Domain.Services;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SupplierService : ISupplierService
    {
        private readonly ISupplierDao _supplierDao;

        public SupplierService(ISupplierDao supplierDao)
        {
            _supplierDao = supplierDao;
        }

        public IList<SupplierDTO> GetAll()
        {
            return _supplierDao.GetAll();
        }

        public SupplierDTO Get(int supplierId)
        {
            return _supplierDao.Get(supplierId);
        }

        public IList<SupplierDTO> FindSuppliersBy(int supplierId, decimal rate)
        {
            return _supplierDao.FindSuppliersBy(supplierId, rate);
        }

        public SupplierRateDTO AddSupplierRate(int supplierId, decimal rate, DateTime startDate, DateTime? endDate)
        {
            if (endDate.HasValue && startDate >= endDate.Value)
            {
                throw new ValidationException("The end date must be greater than start date");
            }

            SupplierDTO supplier = Get(supplierId);

            if (supplier == null)
            {
                throw new NotFoundException($"The supplier of id {supplierId} does not exists");
            }

            ValidateOverLapping(startDate, endDate, supplier.SupplierRates);

            var supplierRate = new SupplierRateDTO
            {
                Rate = rate,
                StartDate = startDate,
                EndDate = endDate
            };

            return _supplierDao.SaveSupplierRate(supplier.SupplierId, supplierRate);
        }

        #region Privates

        private void ValidateOverLapping(DateTime startDate, DateTime? endDate, IList<SupplierRateDTO> supplierRates)
        {
            foreach (var item in supplierRates)
            {
                if (IsOverlapped(item.StartDate, item.EndDate, startDate, endDate))
                {
                    throw new ValidationException("The supplied date period was overlapped by an existing periods, try another period");
                }
            }
        }

        private bool IsOverlapped(
            DateTime startDate, DateTime? endDate,
            DateTime startDate2, DateTime? endDate2)
        {
            if (!endDate2.HasValue && !endDate.HasValue)
            {
                return true;
            }

            if (!endDate2.HasValue)
            {
                return startDate2 < endDate.Value;
            }

            if (!endDate.HasValue)
            {
                return startDate < endDate2.Value;
            }

            return startDate < endDate2.Value && startDate2 < endDate.Value;
        }

        #endregion
    }
}
