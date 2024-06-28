using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class GenreDto
    {
        public class Base
        {
            [Required]
            public string Name { get; set; }
        }
        public class IdHasBase : Base, IIdHas<int>
        {
            public int Id {  get; set; }
        }
    }
}
