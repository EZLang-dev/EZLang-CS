using OpenAbility.Debug;

namespace EZLang.Compiler.Parsing.Nodes;

public abstract class Node
{
    public Dictionary<string, Node> Children = new Dictionary<string, Node>();

    protected bool canHaveChildren = true;
    private static readonly Logger logger = Logger.GetLogger("Node");

    public NodeType Type = NodeType.Undefined;

    public static readonly Node NULL = new NullNode();

    public Node Parent;
    public string Identifier = "NODE_NULL";

    public Node Remove(string name)
    {
        if (canHaveChildren)
        {
            Node child = Children[name];
            child.Parent = null;
            child.Identifier = "";
            Children.Remove(name);
        }
        else
            logger.Error("Cannot remove child nodes of node of type " + Type);
        return this;
    }

    public virtual Node Add(Node node, string? name = null)
    {

        name ??= "NODE_" + Guid.NewGuid().ToString();

        if (canHaveChildren)
        {
            Children.Add(name, node);
            node.Parent = this;
            node.Identifier = name;
        }
        else
            logger.Error("Cannot assign child nodes to node of type " + Type);
        return this;
    }

    public virtual string GetTreeName()
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