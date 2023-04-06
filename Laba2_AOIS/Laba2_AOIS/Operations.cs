namespace Laba2_AOIS
{
    public static class Operations
    {
        private static readonly Dictionary<string, string> operations = new()
        {
            {"+","+"},
            {"|","+"},
            {"&","*"},
            {"*","*"},
            {"->","-"},
            {"=>","-"},
            {"!","!"},
            {"=","="},
            {"==","="},
            {"(", "("}
    };

        public static readonly List<string> keys = new()
        {
            "|", "&", "->", "=>", "=="
        };

        private static readonly Dictionary<char, int> priorities = new()
        {
            {'!', 5},
            {'*', 4},
            {'+', 3},
            {'-', 2},
            {'=', 1},
            {'(', 0}
        };

        public static int GetPriority(this char symbol)
        {
            if (IsOperation(symbol.ToString()))
            {
                return priorities[symbol];
            }

            throw new ArgumentException($"{symbol} is not operation");
        }
        
        public static string GetOperationKey(this string operation)
        {
            if (IsOperation(operation))
            {
                return operations[operation];
            }

            throw new Exception("Invalid operation");
            return null;
        }

        public static bool IsOperation(string operation)
        {
            return operations.ContainsKey(operation);
        }
        
        public static char Conjunction(char first, char second)
        {
            if (first != second) return '0';
            return first == '1' ? '1' : '0';
        }

        public static char Disjunction(char first, char second)
        {
            if (first != second) return '1';
            return first=='0' ? '0' : '1';
        }

        public static char Inverse(char first)
        {
            return first == '0' ? '1' : '0';
        }
        
    }
}