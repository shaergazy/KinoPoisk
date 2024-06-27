namespace Common.Exceptions;

public sealed class InnerException : Exception
{
    public IDictionary<string, IEnumerable<string>> Errors { get; set; }

    public InnerException(IDictionary<string, IEnumerable<string>> errors)
        : base(errors.Values.FirstOrDefault()?.FirstOrDefault())
    {
        Errors = errors;
    }

    public InnerException(string message)
        : this(new Dictionary<string, IEnumerable<string>>
        {
            {"Alert", new [] {message, } },
        })
    { }

    public InnerException(string message, string modelProperty)
        : this(new Dictionary<string, IEnumerable<string>>
        {
            {modelProperty, new [] {message, } }
        })
    { }
}
