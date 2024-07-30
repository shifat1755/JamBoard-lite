
using CDB.Data;
using CDB.Models;
using CDB.Models.Entities;
namespace CDB.Services
{
    public class DrawingService
    {
        private readonly DrawingContext _dbContext;

        public DrawingService(DrawingContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task enterUser(string connectionId, string drawingId)
        {
            var user = new UserBasedData
            {
                ConnectionId = connectionId
            };
            await _dbContext.UserBasedData.AddAsync(user);

            var drawingBoard = await _dbContext.Drawings.FindAsync(drawingId);
            if (drawingBoard != null)
            {
                drawingBoard.ConnectionId.Add(connectionId);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task updateUserActivity(string connectionId, string data)
        {
            var user = await _dbContext.UserBasedData.FindAsync(connectionId);
            if (user != null)
            {
                user.Data.Add(data);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UndoAction(string connectionId)
        {
            var user = await _dbContext.UserBasedData.FindAsync(connectionId);
            if (user != null)
            {
                user.Data.RemoveAt(user.Data.Count - 1);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
