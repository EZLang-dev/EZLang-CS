using EZLang.Compiler.Lexing;
using OpenAbility.Debug;

namespace EZLang.Compiler.Parsing.Nodes;

public abstract class Node
{
    public readonly Dictionary<string, Node> Children = new Dictionary<string, Node>();

    protected bool CanHaveChildren = true;
    private static readonly Logger Logger = Logger.GetLogger("Node");

    public NodeType Type = NodeType.Undefined;

    public static readonly Node Null = new NullNode();

    public Node? Parent;
    public string Identifier = "NODE_NULL";
    public int Level = 0;

    public Node Remove(string name)
    {
        if (CanHaveChildren)
        {
            Node child = Children[name];
            child.Parent = null;
            child.Identifier = "";
            Children.Remove(name);
        }
        else
            Logger.Error("Cannot remove child nodes of node of type " + Type);
        return this;
    }

    public virtual Node Add(Node node, string? name = null)
    {

        name ??= "NODE_" + Guid.NewGuid().ToString();

        if (CanHaveChildren)
        {
            Children.Add(name, node);
            node.Parent = this;
            node.Identifier = name;
        }
        else
            Logger.Error("Cannot assign child nodes to node of type " + Type);
        return this;
    }

    public Node? ClimbToParent(NodeType type)
    {
        if (Parent != null && Parent.Type != type)
            return Parent.ClimbToParent(type);
        return Parent;
    }
    
    public Node? ClimbToTopNode(NodeType type)
    {
        if (Parent != null && Parent.Type == type)
            return Parent.ClimbToTopNode(type);
        return this;
    }

    protected virtual string GetTreeName()
    {
        return Type.ToString();
    }
    
    public void PrintPretty(string indent = "", bool last = true)
    {
        Console.Write(indent);
        if (last)
        {
            Console.Write("\\-");
            indent += "  ";
        }
        else
        {
            Console.Write("|-");
            indent += "| ";
        }
        Console.WriteLine((!Identifier.StartsWith("NODE_") ? Identifier + ": ": "") + GetTreeName());

        for (int i = 0; i < Children.Count; i++)
            Children.Values.ToArray()[i].PrintPretty(indent, i == Children.Count - 1);
    }
}