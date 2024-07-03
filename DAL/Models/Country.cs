using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FlagLink { get; set; }
        public bool IsOwnPicture { get; set; }
    }
}
