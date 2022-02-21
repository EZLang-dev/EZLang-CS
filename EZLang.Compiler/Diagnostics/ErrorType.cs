using EZLang.Compiler;

namespace EZLang.Diagnostics;


public enum ErrorKind
{
    UnexpectedCharacter,
    InvalidToken,
    UnexpectedToken,
    UnknownError,
    EndOfFileException,
    WorkInProgressWarning,
    UnsupportedError,
    UnexpectedTokenGroup,
    DebugMessage
}

public enum ErrorSeverity
{
    Message,
    Warning,
    Error,
    Fatal
}


public class ErrorType
{
    public ErrorKind Kind;
    public bool ShowsPosition;
    public MessageType Severity;

    public ErrorType(ErrorKind type, MessageType severity, bool showsPosition)
    {
        Kind = type;
        ShowsPosition = showsPosition;
        Severity = severity;
    }



    public Message ToMessage(string message, Position? pos = null)
    {
        if (pos != null)
            return new Message(Severity, message, (Position) pos, this);
        return new Message(Severity, message, new Position(-1, -1), this);
    }


    public static readonly ErrorType UnexpectedCharacter =
        new ErrorType(ErrorKind.UnexpectedCharacter, MessageType.Error, true);

    public static readonly ErrorType InvalidToken =
        new ErrorType(ErrorKind.InvalidToken, MessageType.Error, true);

    public static readonly ErrorType UnexpectedToken =
        new ErrorType(ErrorKind.UnexpectedToken, MessageType.Error, true);

    public static readonly ErrorType UnknownError =
        new ErrorType(ErrorKind.UnknownError, MessageType.Error, true);

    public static readonly ErrorType EndOfFileException =
        new ErrorType(ErrorKind.EndOfFileException, MessageType.Error, true);

    public static readonly ErrorType WorkInProgressWarning =
        new ErrorType(ErrorKind.WorkInProgressWarning, MessageType.Warning, true);

    public static readonly ErrorType UnsupportedError =
        new ErrorType(ErrorKind.UnsupportedError, MessageType.Error, true);

    public static readonly ErrorType UnexpectedTokenGroup =
        new ErrorType(ErrorKind.UnexpectedTokenGroup, MessageType.Error, true);

    public static readonly ErrorType DebugMessage =
        new ErrorType(ErrorKind.DebugMessage, MessageType.Info, false);
}