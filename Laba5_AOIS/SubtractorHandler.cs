namespace Laba5_AOIS;

public class SubtractorHandler
{
    private int _variablesCount;
    private string[]? _functionResults;
    public void MakeSubtractorTable(string[] inputVars, string[] outputVars, string[] functionHandler, string incomeSignal)
    {
        char signal = '0';
        _functionResults = new string[functionHandler.Length];
        _variablesCount = outputVars.Length;
        Console.WriteLine("Таблица истинности для вычитателя");
        Console.WriteLine($" |{inputVars[0]} |{inputVars[1]} |{inputVars[2]} | {incomeSignal} |{outputVars[0]} |{outputVars[1]} |{outputVars[2]} |{functionHandler[0]} |{functionHandler[1]} |{functionHandler[2]} ");
        Console.WriteLine("------------------------------------------");
        for (int i = 0; i < 8; i++)
        {
            for (int differentSignals =0; differentSignals < 2; differentSignals++)
            {
                char[] values = GetValuesFromInt(i);
                char[] output = Subtract(i, signal);
                char[] result = GetResult(values, output);
                Console.WriteLine(MakeSubtractorNote(values, output, signal, result));
                signal = GetNextSignal(signal);
                AddFunctions(result, values, outputVars, incomeSignal);
            }
        }

        for (int i = 0; i < _functionResults.Length; i++)
        {
            if (_functionResults[i].Length > 1)
            {
                _functionResults[i] = _functionResults[i].Remove(_functionResults[i].Length - 1, 1);
            }
        }

        foreach (var function in _functionResults)
        {
            Console.WriteLine($"Полученная функция: {function}");
        }

        SdnfHandler handler = new SdnfHandler();

        for (int i = 0; i < _functionResults.Length; i++)
        {
            var function = _functionResults[i];
            handler.SetExpression(function);  
            Console.WriteLine($"Минимизированная функция H{i+1}: {handler.MinimizeWithCalculation()}");
        }
        
    }
    
    
    private char[] GetValuesFromInt(int value)
    {
        char[] values = new char[_variablesCount];
        for (int index = _variablesCount-1; index >= 0; index--)
        {
            values[index] = value % 2 != 0 ? '1' : '0';
            value /= 2;
        }

        return values;
    }

    private char[] Subtract(int value, char signal)
    {
        if (IsSubtractable(signal))
        {
            value -= 1;
            if (value < 0)
            {
                value = -7;
            }
        }
        

        return GetValuesFromInt(value);
    }
    
    private string MakeSubtractorNote(char[] inputValues, char[] outputValues, char signalValue, char[] result)
    {
        string resultString = null!;
        foreach (var value in inputValues)
        {
            resultString += $" | {value}";
        }

        resultString += $" | {signalValue}";

        foreach (var value in outputValues)
        {
            resultString += $" | {value}";
        }

        foreach (var value in result)
        {
            resultString += $" | {value}";
        }
        
        return resultString;
    }

    private bool IsSubtractable(char signal)
    {
        return signal == '1';
    }

    private char GetNextSignal(char currentSignal)
    {
        return currentSignal == '1' ? '0' : '1';
    }

    private char[] GetResult(char[] inputValues, char[] outputValues)
    {
        char[] result = new char[inputValues.Length];
        for (int i = 0; i < inputValues.Length; i++)
        {
            result[i] = Operations.XOR(inputValues[i], outputValues[i]);
        }

        return result;
    }
    
    private string MakeSdnfSet(char[] values, string[] vars, string signal)
    {
        string result = "(";
        for (int i = 0; i < _variablesCount; i++)
        {
            if (values[i] == '0')
            {
                result += $"!{vars[2-i]}&";
            }
            else
            {
                result += $"{vars[2-i]}&";
            }
        }

        result += $"{signal}&";

        if (result.Length > 1)
        {
            result = result.Remove(result.Length - 1, 1);
        }

        result += ')';

        return result;
    }

    private void AddFunctions(char[] result, char[] outputValues, string[] outputNames, string signal)
    {
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] == '1')
            {
                _functionResults![i] += $"{MakeSdnfSet(outputValues, outputNames, signal)}V";
            }
        }
    }
}