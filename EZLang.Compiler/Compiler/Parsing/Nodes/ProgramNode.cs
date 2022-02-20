namespace EZLang.Compiler.Parsing.Nodes;

public class ProgramNode : Node
{
    public ProgramNode()
    {
        this.Type = NodeType.Program;
        this.CanHaveChildren = true;
    }
}