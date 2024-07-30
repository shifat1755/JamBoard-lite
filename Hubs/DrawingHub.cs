using Microsoft.AspNetCore.SignalR;
using CDB.Services;
using System.Threading.Tasks;
using System;

namespace CDB.Hubs
{
    public class DrawingHub : Hub
    {
        private readonly DrawingService _drawingService;
        public DrawingHub(DrawingService drawingService)
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
                await _drawingService.enterUser(connectionId, drawingId);
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

        public async Task UserActivity(string data)
        {
            string connectionId = Context.ConnectionId;
            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];
            if (!string.IsNullOrEmpty(drawingId))
            {
                await Clients.Group(drawingId).SendAsync("ReceiveData", connectionId, data);
                await _drawingService.updateUserActivity(connectionId, data);
            }
        }

        public async Task Undo()
        {
            string connectionId = Context.ConnectionId;
            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];
            if (!string.IsNullOrEmpty(drawingId))
            {
                await Clients.Group(drawingId).SendAsync("Undo", connectionId);
                await _drawingService.UndoAction(connectionId);
            }
        }
    }
}
