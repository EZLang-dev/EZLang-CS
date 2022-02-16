namespace EZLang.Compiler.Lexing;

public struct Token
{
    public TokenType Type = TokenType.Null;
    public string Value = "";
    public Position Position;

    public static readonly Token NULL = new Token(new Position());

    public Token(Position position, TokenType type = TokenType.Null, string value = "")
    {
        this.Type = type;
        this.Value = value;
        this.Position = position;
    }

    public override string ToString()
    {
        return $"[{Type}, '{Value}']";
    }
}