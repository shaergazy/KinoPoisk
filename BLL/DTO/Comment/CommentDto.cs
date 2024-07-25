using DAL.Interfaces;

namespace BLL.DTO.Comment
{
    public class Base
    {
        public string Text { get; set; }
    }

    public class IdHasBase : Base, IIdHas<int>
    {
        public int Id { get; set; }
    }

    public class AddCommentDto : Base { }
    public class EditCommentDto : IdHasBase { }
    public class DeleteCommentDto : IdHasBase { }
    public class GetCommentDto : IdHasBase
    {
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
    public class ListCommentDto : IdHasBase { }
}
