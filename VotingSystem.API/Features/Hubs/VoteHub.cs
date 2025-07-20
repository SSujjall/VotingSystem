using Microsoft.AspNetCore.SignalR;

namespace VotingSystem.API.Features.Hubs
{
    public class VoteHub : Hub
    {
        public async Task JoinPollGroup(string pollId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"poll-{pollId}");
        }

        public async Task LeavePollGroup(string pollId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"poll-{pollId}");
        }
    }
}
