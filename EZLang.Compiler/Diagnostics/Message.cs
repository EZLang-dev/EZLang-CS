using EZLang.Compiler;

namespace EZLang.Diagnostics;

public readonly struct Message
{
    public readonly MessageType Type;
    public readonly string MessageContent;
    public readonly Position Position;
    public readonly ErrorType ErrorType;

    public Message(MessageType type, string message, Position pos, ErrorType errorType)
    {
        this.Type = type;
        this.MessageContent = message;
        this.Position = new Position();
        this.Position = pos;
        this.ErrorType = errorType;
    }

    public void Print()
    {
        if(ErrorType.ShowsPosition)
            PrintMessage($"{Position} {ErrorType.Kind}: {MessageContent}");
        else
            PrintMessage($"{ErrorType.Kind}: {MessageContent}");
    }

    private void PrintMessage(string s)
    {
        switch(Type)
        {
            case(MessageType.Info):
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(s);
                Console.ResetColor();
                break;
            case(MessageType.Warning):
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(s);
                Console.ResetColor();
                break;
            case(MessageType.Error):
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(s);
                Console.ResetColor();
                break;
            case(MessageType.Fatal):
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(s);
                Console.ResetColor();
                break;
        }
    }
}

public enum MessageType
{
    Info,
    Warning,
    Error,
    Fatal
}