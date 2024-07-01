using System.ComponentModel.DataAnnotations;
using DAL.Interfaces;


namespace BLL.DTO
{
    public class CountryDto
    {
        public class Base
        {
            /// <summary>
            /// Id of Country
            /// </summary>
            public int Id { get; set; }

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

            /// <summary>
            /// Link to flag
            /// </summary>
            [Required]
            [MaxLength(256)]
            public string FlagLink { get; set; }
        }
        public class IdHasBase : Base, IIdHas<int>
        {
            public int Id { get; set; }
        }

        public class Add : Base { }
        public class Edit : IdHasBase { }
        public class Delete : IdHasBase { }
        public class Get : IdHasBase { }
    }
}
