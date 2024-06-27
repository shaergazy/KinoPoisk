namespace Common.Extensions;

public static class CharExtension
{
    public static bool IsUpper(this char x)
    {
        return char.IsUpper(x);
    }

    public static char ToLower(this char x)
    {
        return char.ToLower(x);
    }
}
