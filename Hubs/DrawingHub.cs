using Microsoft.AspNetCore.SignalR;
using CDB.Services;
using Microsoft.AspNetCore.Mvc;
namespace CDB.Hubs
{
    public class DrawingHub : Hub
    {
        private readonly IDrawingService _drawingService;
        public DrawingHub(IDrawingService drawingService)
        {
            _drawingService = drawingService;
        }

        private static readonly Dictionary<string, HashSet<string>> GroupMembers = new();

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];

            if (!string.IsNullOrEmpty(drawingId))
            {   
                await Groups.AddToGroupAsync(connectionId, drawingId);

                lock (GroupMembers)
                {
                    if (!GroupMembers.ContainsKey(drawingId))
                    {
                        GroupMembers[drawingId] = new HashSet<string>();
                    }
                    GroupMembers[drawingId].Add(connectionId);
                }

                await Clients.OthersInGroup(drawingId).SendAsync("UserJoined", connectionId);
                bool isSuccess = await evokeBoardRequest(drawingId,connectionId);
                if (!isSuccess) {
                    var data = await _drawingService.GetDrawing(drawingId);
                    await Clients.Client(connectionId).SendAsync("dbData", data);
                }
            }

            await base.OnConnectedAsync();
        }

        public async Task BoardData(string toConnection, string fromId, string stringifiedPath)
        {
            await Clients.Client(toConnection).SendAsync("ReceiveData", fromId,stringifiedPath);
        }

        public async Task<bool> evokeBoardRequest(string boardId, string connectionID)
        {
            string? oneOtherUser = null;
            if(GroupMembers.ContainsKey(boardId) && GroupMembers[boardId].Count > 1)
            {
                foreach(string user in GroupMembers[boardId])
                {
                    if (user == connectionID) { continue; }
                    oneOtherUser = user;
                    break;
                }
                await Clients.Client(oneOtherUser).SendAsync("boardRequest",connectionID);
                return true;
            }
            return false;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;

            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];
            if (!string.IsNullOrEmpty(connectionId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, drawingId);
            }
            lock (GroupMembers)
            {
                GroupMembers[drawingId].Remove(connectionId);
                if(GroupMembers[drawingId].Count == 0)
                {
                    GroupMembers.Remove(drawingId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddState(string ddata)
        {
            string connectionId = Context.ConnectionId;
            string drawingId = Context.GetHttpContext().Request.Query["drawingId"];
            if (!string.IsNullOrEmpty(drawingId))
            {
                await _drawingService.AddState(drawingId, ddata);
                try
                {
                    await Clients.OthersInGroup(drawingId).SendAsync("ReceiveData", connectionId, ddata);

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
                await Clients.OthersInGroup(drawingId).SendAsync("Undo", connectionId);
                await _drawingService.UndoAction(drawingId,state);
            }
        }
    }
}
