namespace KinopoiskWeb.ViewModels.Movie
{
    public class AddCommentVM
    {
        public Guid MovieId { get; set; }
        public string UserId { get; set; }
        public string CommentText { get; set; }
    }

}
