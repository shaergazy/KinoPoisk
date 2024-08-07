﻿using DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Person
{
    public class Base
    {
        /// <summary>
        /// Name of Country
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Code of Country
        /// </summary>
        [MaxLength(2)]
        public string LastName { get; set; }

        /// <summary>
        /// BirthDate
        /// </summary>
        public DateTime BirthDate { get; set; }
    }
    public class IdHasBase : Base, IIdHas<int>
    {
        public int Id { get; set; }
    }

    public class AddPersonDto : Base { }
    public class EditPersonDto : IdHasBase { }
    public class DeletePersonDto : IdHasBase { }
    public class GetPersonDto : IdHasBase { }
    public class ListPersonDto : IdHasBase { }
}
