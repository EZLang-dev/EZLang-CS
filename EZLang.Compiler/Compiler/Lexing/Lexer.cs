using System.Text;
using EZLang.Compiler.Language;
using EZLang.Diagnostics;

namespace EZLang.Compiler.Lexing;

public class Lexer
{
    public List<Token> Tokens = new List<Token>();
    private char[] Code;
    private int ptr;

    private Position pos;


    private bool EndOfFile()
    {
        return EndOfFileAt(0);
    }

    private bool EndOfFileAt(int offset)
    {
        return ptr + offset >= Code.Length;
    }

    private char Current()
    {
        return Peek(0);
    }

    private char Peek(int amt)
    {
        if (EndOfFileAt(amt))
        {
            return '\0';
        }

        return Code[ptr + amt];
    }

    private void Step(int amt)
    {
        for (int i = 0; i < amt; i++)
        {
            ptr++;
            pos.Column++;
            if (Current() == '\n')
            {
                pos.Column = 0;
                pos.Line++;
            }
        }
    }

    public void Lex(string code)
    {
        Tokens = new List<Token>();
        Code = code.ToCharArray();
        ptr = 0;
        pos = new Position();

        while (!EndOfFile())
        {
            char c = Current();
            
            if(char.IsLetter(c)){ LexWord(); continue; }
            if(char.IsDigit(c)) { LexNumber(); continue; }
            if(c == '"') { LexString(); continue; }

            if (char.IsWhiteSpace(c)) { Step(1); continue;}
            if (c == ';') { Logger.Throw(ErrorType.UnsupportedError, "Semicolon newlines are not supported"); Step(1); continue;}
            if (c == '\n') { AddAndStep(TokenType.Newline); continue;}
            
            if(c == '(') { AddAndStep(TokenType.OpenParenthesis); continue;}
            if(c == ')') { AddAndStep(TokenType.CloseParenthesis); continue;}
            if(c == '{') { AddAndStep(TokenType.OpenBracket); continue;}
            if(c == '}') { AddAndStep(TokenType.CloseBracket); continue;}
            if(c == '.') { AddAndStep(TokenType.Accessor); continue;}
            if(new char[] { '*', '+', '-', '/' }.Contains(c)) { AddAndStep(TokenType.Operation, c.ToString()); continue;}

            if (c == '=')
            {
                if (Peek(1) == '=')
                {
                    Step(1);
                    AddAndStep(TokenType.Operation, "==");
                }
                else
                {
                    AddAndStep(TokenType.Set);
                }
                continue;
            }
            
            Logger.Throw(ErrorType.UnexpectedCharacter, c.ToString(), pos);
            Step(1);
        }
    }

    private void AddAndStep(TokenType type, string value = "")
    {
        Tokens.Add(new Token(pos, type, value));
        Step(1);
    }

    private void LexNumber()
    {
        string numstring = "";

        while (char.IsDigit(Current()))
        {
            numstring += Current();
            Step(1);
        }
        
        Tokens.Add(new Token(pos, TokenType.Number, numstring));
    }

    private void LexWord()
    {
        string word = "";
        while (char.IsLetterOrDigit(Current()))
        {
            word += Current();
            Step(1);
        }
        
        Tokens.Add(new Token(pos, Keywords.GetType(word), word));
    }

    private void LexString()
    {
        string value = "";
        Step(1);
        while (Current() != '"')
        {
            value += Current();
            Step(1);
        }
        
        AddAndStep(TokenType.String, value);
    }
}