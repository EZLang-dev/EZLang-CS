namespace EZLang.Compiler;

public struct Position
{
    public int Line;
    public int Column;

    public Position() : this(0, 0)
    {
        
    }

    public Position(int line, int column)
    {
        this.Line = line;
        this.Column = column;
    }

    public override string ToString()
    {
        return "[L: " + Line + ", C: " + Column + "]";
    }
}