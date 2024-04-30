using FastFoodTotem.Api.Controllers.Base;
using FastFoodTotem.Application.Shared.BaseResponse;
using FastFoodTotem.Application.UseCases.Order.CreateOrder;
using FastFoodTotem.Application.UseCases.Order.GetAllOrders;
using FastFoodTotem.Application.UseCases.Order.GetOrderById;
using FastFoodTotem.Application.UseCases.Order.GetOrderByStatus;
using FastFoodTotem.Application.UseCases.Order.GetOrderPaymentStatus;
using FastFoodTotem.Application.UseCases.Order.GetPendingOrders;
using FastFoodTotem.Application.UseCases.Order.UpdateOrder;
using FastFoodTotem.Application.UseCases.Order.ReceiveOrderPayment;
using FastFoodTotem.Domain.Enums;
using FastFoodTotem.Domain.Validations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodTotem.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{ver:apiVersion}/order")]
    [Produces("application/json")]
    public class OrderController : BaseController
    {
        private readonly IMediator _mediator;

        public OrderController(IValidationNotifications validationNotifications, IMediator mediator)
            : base(validationNotifications)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="createOrderRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Id of the new order created</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<CreateOrderResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<CreateOrderResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest createOrderRequest, CancellationToken cancellationToken)
        {
            createOrderRequest.PaymentAccessToken = Request.Headers["Authorization"];
            var data = await _mediator.Send(createOrderRequest, cancellationToken);
            return await Return(new ApiBaseResponse<CreateOrderResponse>() { Data = data });
        }

        /// <summary>
        /// Update order status.
        /// </summary>
        /// <param name="updateOrderRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Id and new status of the updated order</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<UpdateOrderResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<UpdateOrderResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpPatch]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest updateOrderRequest, CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(updateOrderRequest, cancellationToken);
            return await Return(new ApiBaseResponse<UpdateOrderResponse>() { Data = data });
        }

        /// <summary>
        /// Get order by id.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The order requested</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<GetOrderByIdResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<GetOrderByIdResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetOrderByIdRequest(orderId), cancellationToken);
            return await Return(new ApiBaseResponse<GetOrderByIdResponse>() { Data = data });
        }

        /// <summary>
        /// Get all orders.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The order requested</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetAllOrdersRequest(), cancellationToken);
            return await Return(new ApiBaseResponse<GetAllOrdersResponse>() { Data = data });
        }

        /// <summary>
        /// Get all orders in a specific status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<GetOrderByStatusResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<GetOrderByStatusResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpGet("filterByStatus/{status}")]
        public async Task<IActionResult> GetOrderByStatus([FromRoute] OrderStatus status, CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetOrderByStatusRequest(status), cancellationToken);
            return await Return(new ApiBaseResponse<GetOrderByStatusResponse>() { Data = data });
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<GetPendingOrdersResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<GetPendingOrdersResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpGet("getPendingOrders")]
        public async Task<IActionResult> GetPendingOrders(CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetPendingOrdersRequest(), cancellationToken);
            return await Return(new ApiBaseResponse<GetPendingOrdersResponse>() { Data = data });
        }

        /// <summary>
        /// Get order payment status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<GetOrderPaymentStatusResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<GetOrderPaymentStatusResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpGet("paymentStatus/{orderId}")]
        public async Task<IActionResult> GetOrderPaymentStatus([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetOrderPaymentStatusRequest(orderId), cancellationToken);
            return await Return(new ApiBaseResponse<GetOrderPaymentStatusResponse>() { Data = data });
        }

        /// <summary>
        /// Process payment webhook
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderPayedRequestDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBaseResponse<ReceiveOrderPaymentResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiBaseResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiBaseResponse<ReceiveOrderPaymentResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiBaseResponse))]
        [HttpPost("payment/{orderId}")]
        public async Task<IActionResult> ReceiveOrderPayment([FromRoute] int orderId, [FromBody] ReceiveOrderPaymentRequest orderPayedRequestDto, CancellationToken cancellationToken)
        {
            orderPayedRequestDto.OrderId = orderId;
            var data = await _mediator.Send(orderPayedRequestDto, cancellationToken);
            return await Return(new ApiBaseResponse<ReceiveOrderPaymentResponse>() { Data = data });
        }
    }
}
