public class Program
{
    public static void Main()
    {
        #region Exemplos de entrada
        string[,] input = { { "A","B" }, { "A","C" }, { "B","G" }, { "C","H" }, { "E","F" }, { "B","D" }, { "C","E" } };
        // string[,] input = { { "B","D" }, { "D","E" }, { "A","B" }, { "C","F" }, { "E","G" }, { "A","C" } };
        // string[,] input = {  { "A","C" },  { "B","C" },  { "B","D" },  { "D","E" } };
        #endregion

        try
        {
            var root = BuildTree(input);

            Console.WriteLine("Resultado:");
            PrintTree(root);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    public static Node BuildTree(string[,] input)
    {
        var nodes = new Dictionary<string, Node>();

        for (int i = 0; i < input.GetLength(0); i++)
        {
            var parent = input[i, 0];
            var child = input[i, 1];

            Node parentNode;
            Node childNode;

            if (!nodes.TryGetValue(parent, out parentNode))
            {
                parentNode = new Node(parent);
                nodes[parent] = parentNode;
            }

            if (!nodes.TryGetValue(child, out childNode))
            {
                childNode = new Node(child);
                nodes[child] = childNode;
            }

            if (parentNode.Children.Count >= 2)
                throw new InvalidOperationException("Mais de 2 filhos");

            parentNode.Children.Add(childNode);
        }

        var rootNodes = nodes.Values.Except(nodes.Values.SelectMany(n => n.Children)).ToList();
        if (rootNodes.Count > 1)
            throw new InvalidOperationException("Raízes múltiplas");

        return rootNodes.FirstOrDefault();
    }

    public static void PrintTree(Node node, string indent = "")
    {
        Console.WriteLine($"{indent}└─ {node.Root}");

        foreach (var child in node.Children)
        {
            PrintTree(child, indent + "   ");
        }
    }
}

public class Node
{
    public string Root { get; set; }
    public List<Node> Children { get; set; }

    public Node(string root)
    {
        Root = root;
        Children = new List<Node>();
    }
}
