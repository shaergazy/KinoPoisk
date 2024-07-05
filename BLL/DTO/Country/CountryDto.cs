using System.ComponentModel.DataAnnotations;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;


namespace BLL.DTO.Country
{
    public class Base
    {
        /// <summary>
        /// Name of Country
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Code of Country
        /// </summary>
        [MaxLength(2)]
        public string ShortName { get; set; }
    }
    public class IdHasBase : Base, IIdHas<int>
    {
        public int Id { get; set; }
    }

    public class AddCountryDto : Base 
    {
        public IFormFile? Flag { get; set; }
    }
    public class EditCountryDto : IdHasBase 
    {
        public IFormFile? Flag { get; set; }
    }
    public class DeleteCountryDto : IdHasBase { }
    public class ListCountryDto : IdHasBase
    {       
        /// <summary>
        /// Link to flag
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string FlagLink { get; set; }
    }
    public class GetCountryDto : ListCountryDto { }
}
