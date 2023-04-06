namespace Laba2_AOIS
{
    public static class Calculator
    {
        private static string? calculatingExpression;
        private static readonly Stack<char> varsStack = new Stack<char>();
        private static readonly Stack<char> operationsStack = new Stack<char>();
        
        
        public static int Calculate(string? expression)
        {
            calculatingExpression = expression;
            ParseString();
            var result = varsStack.Pop();
            varsStack.Clear();
            operationsStack.Clear();
            if (result == '1')
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private static void ParseString()
        {
            bool bracketOnTop = false;
            if (calculatingExpression != null)
                foreach (var currentChar in calculatingExpression)
                {
                    if (currentChar == '(')
                    {
                        operationsStack.Push(currentChar);
                        bracketOnTop = true;
                        continue;
                    }

                    if (char.IsDigit(currentChar))
                    {
                        varsStack.Push(currentChar);
                        continue;
                    }

                    if (Operations.IsOperation(currentChar.ToString()))
                    {
                        if (bracketOnTop)
                        {
                            operationsStack.Push(currentChar);
                            bracketOnTop = false;
                        }
                        else
                        {
                            if (operationsStack.Count > 0)
                            {
                                if (BiggerPriority(operationsStack.Peek(), currentChar))
                                {
                                    CompleteOperation(operationsStack.Pop());
                                }
                            }

                            operationsStack.Push(currentChar);
                        }

                        continue;
                    }

                    if (currentChar == ')')
                    {
                        while (operationsStack.Peek() != '(')
                        {
                            CompleteOperation(operationsStack.Pop());
                        }

                        operationsStack.Pop();
                        if (operationsStack.Peek() == '!')
                        {
                            CompleteOperation(operationsStack.Pop());
                        }

                        continue;
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
                    varsStack.Push(Operations.Conjunction(varsStack.Pop(), varsStack.Pop()));
                    break;
                }
                
                case '+':
                {
                    varsStack.Push(Operations.Disjunction(varsStack.Pop(), varsStack.Pop()));
                    break;
                }
                
                case '!':
                {
                    varsStack.Push(Operations.Inverse(varsStack.Pop()));
                    break;
                }
            }
        }
    }
}