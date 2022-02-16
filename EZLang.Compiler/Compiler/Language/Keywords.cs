using EZLang.Compiler.Lexing;

namespace EZLang.Compiler.Language;

public static class Keywords
{
    /// <summary>
    /// Get the type of word
    /// </summary>
    /// <param name="word">The word</param>
    /// <returns>The type</returns>
    public static TokenType GetType(string word)
    {
        if (word == "package") return TokenType.IncludeKwd;
        if (word == "function") return TokenType.FunctionKwd;
        if (word == "program") return TokenType.ProgramKwd;
        if (word == "globals") return TokenType.GlobalsKwd;
        if (word == "if") return TokenType.IfKwd;
        if (word == "else") return TokenType.ElseKwd;
        if (word == "for") return TokenType.ForKwd;

        return TokenType.Identifier;
    }
}