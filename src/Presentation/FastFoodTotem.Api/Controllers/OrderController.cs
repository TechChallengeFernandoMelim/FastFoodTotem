using FastFoodTotem.Api.Controllers.Base;
using FastFoodTotem.Application.Shared.BaseResponse;
using FastFoodTotem.Application.UseCases.Order.CreateOrder;
using FastFoodTotem.Application.UseCases.Order.DeleteUserData;
using FastFoodTotem.Application.UseCases.Order.GetAllOrders;
using FastFoodTotem.Application.UseCases.Order.GetOrderById;
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
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetAllOrdersRequest(), cancellationToken);
            return await Return(new ApiBaseResponse<GetAllOrdersResponse>() { Data = data });
        }

        /// <summary>
        /// Delete user data
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The order requested</returns>
        [HttpDelete("DeleteUserData/{cpf}")]
        public async Task<IActionResult> DeleteUserData([FromRoute] string cpf, CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new DeleteUserDataRequest(cpf), cancellationToken);
            return await Return(new ApiBaseResponse<DeleteUserDataResponse>() { });
        }
    }
}
