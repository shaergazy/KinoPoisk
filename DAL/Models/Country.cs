namespace DAL.Models
{
    public class Country : TranslatableEntity
    {
        public string? ShortName { get; set; }
        public string? FlagLink { get; set; }
        public bool IsOwnPicture { get; set; }
    } 
}
