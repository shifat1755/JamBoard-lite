using Microsoft.AspNetCore.SignalR;
using CDB.Services;
namespace CDB.Hubs
{
    public class DrawingHub : Hub
    {
        private readonly IDrawingService _drawingService;
        public DrawingHub(IDrawingService drawingService)
        {
            _drawingService = drawingService;
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];
            if (!string.IsNullOrEmpty(drawingId))
            {
                await Groups.AddToGroupAsync(connectionId, drawingId);
                await Clients.Group(drawingId).SendAsync("UserJoined", connectionId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];
            if (!string.IsNullOrEmpty(connectionId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, drawingId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddState(string data)
        {
            string connectionId = Context.ConnectionId;
            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];
            if (!string.IsNullOrEmpty(drawingId))
            {
                await _drawingService.AddState(drawingId, data);
                try
                {
                    await Clients.OthersInGroup(drawingId).SendAsync("ReceiveData", connectionId, data);

                }
                catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
                }
            }
        }

        public async Task Undo(string state)
        {
            string connectionId = Context.ConnectionId;
            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];
            if (!string.IsNullOrEmpty(drawingId))
            {
                await Clients.Group(drawingId).SendAsync("Undo", connectionId);
                await _drawingService.UndoAction(drawingId,state);
            }
        }
    }
}
