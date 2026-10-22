using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.Results;

namespace Ticketing.Application.Features.Tickets.Commands.ConfirmMessage
{
    public class ConfirmMessageCommandHandler(ITicketMessageRepository ticketMessageRepository) : IRequestHandler<ConfirmMessageCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ConfirmMessageCommand request, CancellationToken cancellationToken)
        {
            var result = await ticketMessageRepository.ConfirmAgentMessageAsync(request.MessageId, request.AdminUserId, cancellationToken);


            return result ? Result<bool>.Success(true) : Result<bool>.Failure(ApplicationErrors.TicketMessageNotFound(request.MessageId));
        }
    }



}
