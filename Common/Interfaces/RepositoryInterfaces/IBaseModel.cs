namespace Common.Interfaces
{
    public interface IBaseModel<T>
        where T : IEquatable<T>
    {
        public T Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }

}

