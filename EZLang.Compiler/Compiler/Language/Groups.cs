using EZLang.Compiler.Lexing;

namespace EZLang.Compiler.Language;

public static class Groups
{
    public static readonly TokenType[] GROUP_LITERALS = new[]
    {
        TokenType.String,
        TokenType.Number
    };
    
    public static bool InGroup<T>(T value, T[] group)
    {
        return group.Contains(value);
    }
}