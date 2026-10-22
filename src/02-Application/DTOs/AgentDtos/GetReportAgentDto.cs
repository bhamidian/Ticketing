namespace Ticketing.Application.DTOs.AgentDtos
{
    public class GetReportAgentDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public int RespondedTicketsCount { get; set; }
    }
}
