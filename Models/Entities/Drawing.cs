using System.ComponentModel.DataAnnotations;

namespace CDB.Models.Entities
{
    public class UserBasedData
    {
        [Key]
        public string ConnectionId { get; set; }
        public List<string> Data { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Drawing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> ConnectionId { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
