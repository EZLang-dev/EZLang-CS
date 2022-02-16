using EZLang.Compiler.Lexing;
using EZLang.Compiler.Parsing;
using OpenAbility.Debug;

namespace EZLang.Compiler;

public class CompilerManager
{
    private static readonly Logger LOGGER = Logger.GetLogger("Compiler");
    public void Compile(string program)
    {
        LOGGER.Log("Compiling...");
        LOGGER.Log("Lexing...");
        Lexer lexer = new Lexer();
        lexer.Lex(program);
        foreach (var token in lexer.Tokens)
        {
            LOGGER.Log(token.ToString());
        }

        Parser parser = new Parser();
        parser.Parse(lexer.Tokens);
        foreach (var node in parser.Nodes)
        {
            node.PrintPretty();
        }
        
        Logger.Save();
    }
}