using Microsoft.AspNetCore.Mvc;
using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Application.Services.Interfaces.Validation;
using OrderManager.API.Contracts.Requests;
using OrderManager.API.Contracts.Responses;
using OrderManager.API.Domain.Enums;
using OrderManager.API.Domain.Exceptions;

namespace OrderManager.API.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class DeliveryController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IDeliveryService _deliveryService;

        private readonly IValidationService _validationService;

        public DeliveryController(IValidationService validationService, ILabelService labelService, IDeliveryService deliveryService)
        {
            _labelService = labelService;
            _deliveryService = deliveryService;
            _validationService = validationService;
        }

        [HttpPost("create")]
        public IActionResult CreateDelivery([FromBody] CreateDeliveryRequest request)
        {
            try
            {
                var delivery = _deliveryService.CreateDelivery(
                    request.Recipient,
                    request.Address,
                    request.Weight,
                    request.ShippingType);

                var response = new DeliveryResponse
                {
                    Recipient = delivery.Recipient.Name,
                    Address = delivery.DeliveryAddress.Value,
                    Weight = delivery.PackageWeight.Value,
                    ShippingType = delivery.ShippingType.ToDisplayName(),
                    ShippingCost = delivery.ShippingCost,
                    IsFreeShipping = delivery.IsFreeShipping,
                    Label = _labelService.GenerateShippingLabel(delivery),
                    Summary = _labelService.GenerateOrderSummary(delivery)
                };

                return Ok(response);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
            }
        }

        [HttpPost("apply-promotional-discount")]
        public IActionResult ApplyPromotionalDiscount([FromBody] CreateDeliveryRequest request)
        {
            try
            {
                ValidateRequest(request);

                var originalDelivery = _deliveryService.CreateDelivery(
                    request.Recipient,
                    request.Address,
                    request.Weight,
                    request.ShippingType);

                var discountedDelivery = _deliveryService.ApplyPromotionalDiscount(originalDelivery);

                var response = new PromotionalDiscountResponse
                {
                    OriginalCost = originalDelivery.ShippingCost,
                    DiscountedCost = discountedDelivery.ShippingCost,
                    Savings = originalDelivery.ShippingCost - discountedDelivery.ShippingCost,
                    DiscountApplied = originalDelivery.ShippingCost != discountedDelivery.ShippingCost,
                    FinalDelivery = new DeliveryResponse
                    {
                        Recipient = discountedDelivery.Recipient.Name,
                        Address = discountedDelivery.DeliveryAddress.Value,
                        Weight = discountedDelivery.PackageWeight.Value,
                        ShippingType = discountedDelivery.ShippingType.ToDisplayName(),
                        ShippingCost = discountedDelivery.ShippingCost,
                        IsFreeShipping = discountedDelivery.IsFreeShipping,
                        Label = _labelService.GenerateShippingLabel(discountedDelivery),
                        Summary = _labelService.GenerateOrderSummary(discountedDelivery)
                    }
                };

                return Ok(response);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
            }
        }

        [HttpGet("shipping-types")]
        public IActionResult GetShippingTypes()
        {
            var shippingTypes = Enum.GetValues<ShippingType>()
                .Select(st => new
                {
                    Code = st.ToCode(),
                    Name = st.ToDisplayName(),
                    Value = st.ToString()
                })
                .ToList();

            return Ok(shippingTypes);
        }

        private void ValidateRequest(CreateDeliveryRequest request)
        {
            _validationService.ValidateRecipient(request.Recipient);
            _validationService.ValidateAddress(request.Address);
            _validationService.ValidateWeight(request.Weight);
            _validationService.ValidateShippingType(request.ShippingType);
        }
    }
}
