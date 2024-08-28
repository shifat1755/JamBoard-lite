using System.ComponentModel.DataAnnotations;

namespace CDB.Models.Entities
{
    public class DrawingState
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        public string DrawingName { get; set; }
    }
}
