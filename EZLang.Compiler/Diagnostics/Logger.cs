using EZLang.Compiler;

namespace EZLang.Diagnostics;

public static class Logger
{
    public static List<Message> Messages = new List<Message>();

    public static void Throw(ErrorType type, string message, Position? pos = null)
    {
        Message msg = type.ToMessage(message, pos);
        Messages.Add(msg);
        msg.Print();
    }
}