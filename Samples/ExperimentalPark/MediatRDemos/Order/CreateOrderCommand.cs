using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatRDemos.Order
{
    public class CreateOrderCommand:IRequest<string>
    {
        public string OrderName { set; get; }
    }

    public class CreateOrderCommandHandle : IRequestHandler<CreateOrderCommand, string>
    {
        public Task<string> Handle(CreateOrderCommand request,CancellationToken cancellationToken)
        {
            var s = $"CreateOrderCommandHandler:Create Order ${request.OrderName}";
            return Task.FromResult(s);
        }
    }
}
