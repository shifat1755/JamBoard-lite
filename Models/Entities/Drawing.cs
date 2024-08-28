using System.ComponentModel.DataAnnotations;

namespace CDB.Models.Entities
{
    public class Drawing
    {
        [Key]
        public string Name { get; set; }
        public string UserName { get; set; }
        public ICollection<DrawingState> states { get; set; } = new List<DrawingState>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
