﻿using FastFoodTotem.Api.Controllers.Base;
using FastFoodTotem.Application.ApplicationServicesInterfaces;
using FastFoodTotem.Application.Dtos.Requests.Order;
using FastFoodTotem.Application.Dtos.Responses;
using FastFoodTotem.Application.Dtos.Responses.Order;
using FastFoodTotem.Domain.Enums;
using FastFoodTotem.Domain.Validations;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodTotem.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{ver:apiVersion}/order")]
    [Produces("application/json")]
    public class OrderController : BaseController
    {
        private readonly IOrderApplicationService _orderApplicationService;

        public OrderController(
            IOrderApplicationService orderApplicationService, 
            IValidationNotifications validationNotifications)
            : base(validationNotifications)
        {
            _orderApplicationService = orderApplicationService ?? throw new ArgumentNullException(nameof(orderApplicationService));
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="orderCreateRequestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Id of the new order created</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<OrderCreateResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<OrderCreateResponseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequestDto orderCreateRequestDto, CancellationToken cancellationToken)
        {
            return await Return(await _orderApplicationService.CreateAsync(orderCreateRequestDto, cancellationToken));
        }

        /// <summary>
        /// Update order status.
        /// </summary>
        /// <param name="orderUpdateRequestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Id and new status of the updated order</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<OrderUpdateResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<OrderUpdateResponseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpPatch]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateRequestDto orderUpdateRequestDto, CancellationToken cancellationToken)
        {
            return await Return(await _orderApplicationService.UpdateAsync(orderUpdateRequestDto, cancellationToken));
        }

        /// <summary>
        /// Get order by id.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The order requested</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<OrderGetByIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<OrderGetByIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            return await Return(await _orderApplicationService.GetByIdAsync(orderId, cancellationToken));
        }

        /// <summary>
        /// Get all orders.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The order requested</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return await Return(await _orderApplicationService.GetAllAsync(cancellationToken));
        }

        /// <summary>
        /// Get all orders in a specific status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<OrderGetAllResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<OrderGetAllResponseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpGet("filterByStatus/{status}")]
        public async Task<IActionResult> GetOrderByStatus([FromRoute] OrderStatus status, CancellationToken cancellationToken)
        {
            return await Return(await _orderApplicationService.GetOrderByStatus(status, cancellationToken));
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<OrderGetAllResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<OrderGetAllResponseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpGet("getPendingOrders")]
        public async Task<IActionResult> GetPendingOrders(CancellationToken cancellationToken)
        {
            return await Return(await _orderApplicationService.GetPendingOrders(cancellationToken));
        }

        /// <summary>
        /// Get payment status by orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<OrderGetAllResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<OrderGetAllResponseDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpGet("paymentStatus/{orderId}")]
        public async Task<IActionResult> GetOrderPaymentStatus([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            return await Return(await _orderApplicationService.GetOrderPaymentAsync(orderId, cancellationToken));
        }
    }
}
