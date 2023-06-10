namespace Laba3_AOIS
{
    public static class Calculator
    {
        private static string? _calculatingExpression;
        private static readonly Stack<char> VarsStack = new Stack<char>();
        private static readonly Stack<char> OperationsStack = new Stack<char>();
        
        
        public static int Calculate(string? expression)
        {
            _calculatingExpression = expression;
            ParseString();
            var result = VarsStack.Pop();
            VarsStack.Clear();
            OperationsStack.Clear();
            if (result == '1')
            {
                return 1;
            }

            return 0;
        }

        private static void ParseString()
        {
            bool bracketOnTop = false;
            if (_calculatingExpression != null)
                foreach (var currentChar in _calculatingExpression)
                {
                    if (currentChar == '(')
                    {
                        OperationsStack.Push(currentChar);
                        bracketOnTop = true;
                        continue;
                    }

                    if (char.IsDigit(currentChar))
                    {
                        VarsStack.Push(currentChar);
                        continue;
                    }

                    if (Operations.IsOperation(currentChar.ToString()))
                    {
                        if (bracketOnTop)
                        {
                            OperationsStack.Push(currentChar);
                            bracketOnTop = false;
                        }
                        else
                        {
                            if (OperationsStack.Count > 0)
                            {
                                if (BiggerPriority(OperationsStack.Peek(), currentChar))
                                {
                                    CompleteOperation(OperationsStack.Pop());
                                }
                            }

                            OperationsStack.Push(currentChar);
                        }

                        continue;
                    }

                    if (currentChar == ')')
                    {
                        while (OperationsStack.Peek() != '(')
                        {
                            CompleteOperation(OperationsStack.Pop());
                        }

                        OperationsStack.Pop();
                        if (OperationsStack.Count > 1)
                        {
                            if (OperationsStack.Peek() == '!')
                            {
                                CompleteOperation(OperationsStack.Pop());
                            }
                        }
                    }
                }
        }

        private static bool BiggerPriority(char operation1, char operation2)
        {
            return operation1.GetPriority()>operation2.GetPriority();
        }

        private static void CompleteOperation(char operation)
        {
            switch (operation)
            {
                case '*':
                {
                    VarsStack.Push(Operations.Conjunction(VarsStack.Pop(), VarsStack.Pop()));
                    break;
                }
                
                case '+':
                {
                    VarsStack.Push(Operations.Disjunction(VarsStack.Pop(), VarsStack.Pop()));
                    break;
                }
                
                case '!':
                {
                    VarsStack.Push(Operations.Inverse(VarsStack.Pop()));
                    break;
                }
            }
        }
    }
}