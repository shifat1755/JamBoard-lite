
using CDB.Data;
using CDB.Models;
using CDB.Models.Drawing;
using CDB.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace CDB.Services
{
    public class DrawingService:IDrawingService
    {
        private readonly DrawingContext _dbContext;

        public DrawingService(DrawingContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddState(string DrawingName, string data)
        {
            var state = new DrawingState()
            {
                Value = data,
                DrawingName = DrawingName
            };
            await _dbContext.DrawingStates.AddAsync(state);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UndoAction(string drawingName, string data )
        {
            var drawindState = await _dbContext.DrawingStates
                .FirstOrDefaultAsync(i => i.DrawingName == drawingName && i.Value == data);
            _dbContext.DrawingStates.Remove(drawindState);
            await _dbContext.SaveChangesAsync(); 

        }
        public async Task<Response> Create(CreateViewModel model)
        {
            var response= new Response();
            var Exists=await _dbContext.Drawings.FirstOrDefaultAsync(i=>i.Name == model.Name);
            if (Exists != null)
            {
                response.message = "Drawing name has to be unique.";
                response.success = false;
                return response;
            }
            try
            {
                var data = new Drawing()
                {
                    Name = model.Name,
                    UserName = model.UserName,
                };
                await _dbContext.Drawings.AddAsync(data);
                await _dbContext.SaveChangesAsync();
                response.success=true;
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
                response.success=false;

            }
            return response;
        }

        public async Task<List<string>> GetDrawing(string name)
        {
            var drawing=await _dbContext.Drawings
                .Include(x=>x.states)
                .FirstAsync(x=>x.Name==name);
            List<string>data=new List<string>();
            foreach (var cId in drawing.states) {
                data.Add(cId.Value);
            }
            return data;
            
        }


    }
}
