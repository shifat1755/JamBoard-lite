using CDB.Models;
using CDB.Models.Drawing;

namespace CDB.Services
{
    public interface IDrawingService
    {
        public Task AddState(string DrawingName, string data);
        public Task UndoAction(string drawingName, string data);
        public Task<Response> Create(CreateViewModel model);
        public Task<List<string>> GetDrawing(string name);

    }
}
