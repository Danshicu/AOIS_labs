using Laba5_AOIS;

namespace Laba4_AOIS;

public class SummatorsHandlers
{
    private int _variablesCount;

    public void MakeD8421Table(string[] inputVars, string[] outputVars)
    {
        string y1Result = null!;
        string y2Result = null!;
        string y3Result = null!;
        string y4Result = null!;
        _variablesCount = outputVars.Length;
        Console.WriteLine("Таблица истинности для D8421+2");
        Console.WriteLine($" |{inputVars[0]} |{inputVars[1]} |{inputVars[2]} |{inputVars[3]} |{outputVars[0]} |{outputVars[1]} |{outputVars[2]} |{outputVars[3]}");
        Console.WriteLine("--------------------------------");
        for (int i = 0; i < 16; i++)
        {
            char[] values = GetValuesFromInt(i);
            char[] output = GetD8421Result(i);
            if (i >= 10)
            {
                for (int index = 0; index < output.Length; index++)
                {
                    output[index] = '-';
                }
            }
            Console.WriteLine(MakeD8421Note(values, output));
            if (output[0] == '1')
            {
                y1Result += $"{MakeSdnfSet(values, inputVars)}V";
            }
            if (output[1] == '1')
            {
                y2Result += $"{MakeSdnfSet(values, inputVars)}V";
            }
            if (output[2] == '1')
            {
                y3Result += $"{MakeSdnfSet(values, inputVars)}V";
            }
            if (output[3] == '1')
            {
                y4Result += $"{MakeSdnfSet(values, inputVars)}V";
            }
        }

        if (y1Result.Length > 0)
        {
            y1Result = y1Result.Remove(y1Result.Length - 1, 1);
        }
        if (y2Result.Length > 0)
        {
            y2Result = y2Result.Remove(y2Result.Length - 1, 1);
        }
        if (y3Result.Length > 0)
        {
            y3Result = y3Result.Remove(y3Result.Length - 1, 1);
        }
        if (y4Result.Length > 0)
        {
            y4Result = y4Result.Remove(y4Result.Length - 1, 1);
        }
        Console.WriteLine($"Функция Y1 В СДНФ: {y1Result}");
        Console.WriteLine($"Функция Y2 В СДНФ: {y2Result}");
        Console.WriteLine($"Функция Y3 В СДНФ: {y3Result}");
        Console.WriteLine($"Функция Y4 В СДНФ: {y4Result}");
        SdnfHandler handler = new SdnfHandler();
        handler.SetExpression(y1Result);
        Console.WriteLine($"Минимизированная функция Y1: {handler.MinimizeWithCalculation()}");
        handler.SetExpression(y2Result);
        Console.WriteLine($"Минимизированная функция Y2: {handler.MinimizeWithCalculation()}");
        handler.SetExpression(y3Result);
        Console.WriteLine($"Минимизированная функция Y3: {handler.MinimizeWithCalculation()}");
        handler.SetExpression(y4Result);
        Console.WriteLine($"Минимизированная функция Y4: {handler.MinimizeWithCalculation()}");
    }
    
    public void MakeSummatorTable(char[] vars)
    {
        string result = null!;
        string carry = null!;
        _variablesCount = vars.Length;
        string[] usingVars = new string[_variablesCount];
        for (int varIndex = 0; varIndex < _variablesCount; varIndex++)
        {
            usingVars[varIndex] = vars[varIndex].ToString();
        }
        Console.WriteLine("Таблица истинности для ОДС");
        Console.WriteLine($" | {vars[0]} | {vars[1]} | {vars[2]} | S | Pi |");
        Console.WriteLine("-----------------------");
        for (int i = 0; i < 8; i++)
        {
            var values = GetValuesFromInt(i);
            var operationResults = GetResult(values);
            Console.WriteLine(MakeSummatorNote(values, operationResults));
            if (operationResults.Item1 == '1')
            {
                result += $"{MakeSdnfSet(values, usingVars)}V";
            }
            if (operationResults.Item2 == '1')
            {
                carry += $"{MakeSdnfSet(values, usingVars)}V";
            }
        }

        if (result.Length > 0)
        {
           result = result.Remove(result.Length - 1, 1);
        }

        if (carry.Length > 0)
        {
            carry = carry.Remove(carry.Length - 1, 1);
        }
        Console.WriteLine($"Функция результата(S): {result}");
        Console.WriteLine($"Функция остатка(P): {carry}");
        SdnfHandler resultHandler = new SdnfHandler();
        resultHandler.SetExpression(result);
        Console.WriteLine($"Минимизированная функция результата: {resultHandler.MinimizeWithCalculation()}");
        resultHandler.SetExpression(carry);
        Console.WriteLine($"Минимизированная функция остатка: {resultHandler.MinimizeWithCalculation()}");
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
    
    private Tuple<char, char> GetResult(char[] values)
    {
        char firstSubRes = Operations.XOR(values[0], values[1]);
        char resultToDraw = Operations.XOR(firstSubRes, values[2]);
        char firstOperator = Operations.Conjunction(firstSubRes, values[2]);
        char secondOperator = Operations.Conjunction(values[0], values[1]);
        char resultToTransit = Operations.XOR(firstOperator, secondOperator);
        return new Tuple<char, char>(resultToDraw, resultToTransit);
    }

    private string MakeSummatorNote(char[] values, Tuple<char, char> results)
    {
        string resultString = null!;
        foreach (var value in values)
        {
            resultString += $" | {value}";
        }

        resultString += $" | {results.Item1}";
        resultString += $" | {results.Item2}";
        return resultString;
    }

    private string MakeD8421Note(char[] inputValues, char[] outputValues)
    {
        string resultString = null!;
        foreach (var value in inputValues)
        {
            resultString += $" | {value}";
        }

        foreach (var value in outputValues)
        {
            resultString += $" | {value}";
        }
        return resultString;
    }

    private string MakeSdnfSet(char[] values, string[] vars)
    {
        string result = "(";
        for (int i = 0; i < _variablesCount; i++)
        {
            if (values[i] == '0')
            {
                result += $"!{vars[i]}&";
            }
            else
            {
                result += $"{vars[i]}&";
            }
        }

        if (result.Length > 1)
        {
            result = result.Remove(result.Length - 1, 1);
        }

        result += ')';

        return result;
    }

    private char[] GetD8421Result(int result)
    {
        result += 2;
        return GetValuesFromInt(result);
    }

}