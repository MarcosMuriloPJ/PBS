public class Program
{
    public static void Main()
    {
        #region Input examples
        string[,] input = { { "B","G" }, { "A","B" }, { "A","C" }, { "C","H" }, { "E","F" }, { "B","D" }, { "C","E" } }; // Cenário perfeito
        // string[,] input = { { "A","B" }, { "C","D" }, { "A","C" }, { "A","E" } }; // Exceção E1 (Mais de 2 filhos)
        // string[,] input = { { "A","B" }, { "B","D" }, { "A","C" }, { "D","A" } }; // Exceção E2 (Ciclo presente)
        // string[,] input = { { "B","D" }, { "D","E" }, { "A","B" }, { "C","F" } }; // Exceção E3 (Raízes múltiplas)
        // string[,] input = { }; // Exceção E4 'Qualquer outro erro'
        #endregion

        try
        {
            Console.WriteLine("Inicializando...");
            var tree = BuildTree(input);

            PrintTree(tree);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    public static Node BuildTree(string[,] input)
    {
        var nodes = new Dictionary<string, Node>();
        var recursionStack = new HashSet<Node>();

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

        foreach (var node in nodes.Values)
        {
            if (HasCycle(node, recursionStack))
                throw new InvalidOperationException("Ciclo presente");
        }

        Console.WriteLine("Construção de árvore finalizada...");
        return rootNodes.FirstOrDefault();
    }

    private static bool HasCycle(Node node, HashSet<Node> recursionStack)
    {
        if (recursionStack.Contains(node))
            return true;

        recursionStack.Add(node);

        foreach (var child in node.Children)
        {
            if (HasCycle(child, recursionStack))
                return true;
        }

        recursionStack.Remove(node);
        return false;
    }

    public static void PrintTree(Node tree, string indent = "")
    {
        Console.WriteLine($"{indent}└─ {tree.Root}");

        foreach (var child in tree.Children)
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
