using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Commands.ConfirmMessage
{
    public class ConfirmMessageCommand : IRequest<Result<bool>>
    {
        public Guid MessageId {get; set;}
        public Guid AdminUserId { get; set; }
    }
}
