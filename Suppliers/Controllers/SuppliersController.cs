namespace Suppliers.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Suppliers.API.ErrorHandling;
    using Suppliers.API.RequestModels;
    using Suppliers.Domain.Exceptions;
    using Suppliers.Domain.Models;
    using Suppliers.Domain.Services;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net;

    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorResult), (int)HttpStatusCode.InternalServerError)]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET api/suppliers
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SupplierDTO>), (int)HttpStatusCode.OK)]
        public IActionResult Get()
        {
            return Ok(_supplierService.GetAll());
        }

        // GET api/suppliers/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SupplierDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)HttpStatusCode.NotFound)]
        public IActionResult Get(int id)
        {
            var supplier = _supplierService.Get(id);

            if (supplier == null)
            {
                throw new NotFoundException($"The supplier of id {id} does not exists");
            }

            return Ok(supplier);
        }

        // GET api/suppliers/{id}/search?
        [HttpGet("{id}/search")]
        [ProducesResponseType(typeof(IEnumerable<SupplierDTO>), (int)HttpStatusCode.OK)]
        public IActionResult Get(int id, [FromQuery]SearchSupplier queryparams)
        {
            return Ok(_supplierService.FindSuppliersBy(id, queryparams.Rate));
        }

        // POST api/suppliers/{id}/supplierRate
        [HttpPost("{id}/supplierRate")]
        [ProducesResponseType(typeof(SupplierRateDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), (int)HttpStatusCode.NotFound)]
        public IActionResult AddSupplierRate(int id, [FromBody]SupplierRateDTO request)
        {
            if (request == null)
            {
                throw new ValidationException("The request can not be null");
            }
            
            return Ok(_supplierService
                    .AddSupplierRate(id, request.Rate, request.StartDate, request.EndDate));
        }
    }
}
