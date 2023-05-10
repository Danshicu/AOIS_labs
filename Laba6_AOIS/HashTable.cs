namespace Laba6_AOIS;

public class HashNode
{
    public int HashKey { get; }
    private string HashData { get; }
    public HashNode? NextNode { get; set; }
    
    public HashNode(int key, string data)
    {
        HashKey = key;
        HashData = data;
        NextNode = null;
    }
    
    public string Out => $"Key:  {HashKey}  Data:  {HashData}";
}

public class HashTable
{
    private readonly List<HashNode?> _nodes;
    private readonly int _size;

    public HashTable(int size)
    {
        _size = size;
        _nodes = new List<HashNode?>(_size);
    }

    private int GetValueForKey(string key)
    {
        if (key.Length == 0)
        {
            return 0;
        }

        if (key.Length < 2)
        {
            return key[0];
        }

        int totalCode = 0;
        for (int index = 0; index < key.Length; index++)
        {
            totalCode += key[index] * (54 ^ index);
        }

        return totalCode;
    }

    private int HashFunction(string key) => GetValueForKey(key) % _size;

    public void ShowTable()
    {
        foreach (var node in _nodes)
        {
            Console.WriteLine(node!.Out);
        }
    }

    public void AddNode(string key, string data)
    {
        if (_nodes.Count < _size)
        {
            int hashIndex = HashFunction(key);
            var existingNodes = FindNodesByKey(key);
            if (existingNodes.Count == 0)
            {
                _nodes.Add(new HashNode(hashIndex, data));
            }
            else
            {
                HashNode lastNode =existingNodes[^1];
                Console.WriteLine($"Collision with node: {lastNode.Out}");
                HashNode newNode = new HashNode(hashIndex, data);
                _nodes.Add(newNode);
                lastNode.NextNode = newNode;
            }
        }
        else
        {
            Console.WriteLine("TABLE IS FULL, CAN'T ADD NEW RECORD");
        }

    }

    public void FindNode(string key)
    {
        List<HashNode> foundNodes = FindNodesByKey(key);
        if (foundNodes.Count == 0)
        {
            Console.WriteLine("CANT FIND NODES WITH SUCH KEY");
        }
        else
        {
            foreach (var node in foundNodes)
            {
                Console.WriteLine($"Found node: {node.Out}");
            }
        }

    }

    private List<HashNode> FindNodesByKey(string key)
    {
        List<HashNode> foundNodes = new List<HashNode>();
        int intKey = HashFunction(key);
        foreach (var t in _nodes)
        {
            var currentNode = t;
            if (currentNode!.HashKey == intKey)
            {
                foundNodes.Add(currentNode);
                while (currentNode.NextNode != null)
                {
                    currentNode = currentNode.NextNode;
                    foundNodes.Add(currentNode);
                }

                break;
            }
        }
        return foundNodes;
    }

    public void DeleteNode(string key)
    {
        List<HashNode?> nodesToDelete = FindNodesByKey(key)!;
        foreach (var deletingNode in nodesToDelete)
        {
            _nodes.Remove(deletingNode);
        }

        if (nodesToDelete.Count == 0)
        {
            Console.WriteLine("NO NODES WITH THIS KEY TO DELETE");
        }
    }
    
    
}