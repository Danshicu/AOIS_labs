namespace Laba7_AOIS;
using Laba3_AOIS;
public class Processor
{
    private string[] Memory { set; get; }
    private readonly int _bitsCount;
    private readonly Random _random = new Random();

    public Processor(int wordsCount, int bitsCount)
    {
        this._bitsCount = bitsCount;
        Memory = new string[wordsCount];
        FillMemory();
    }

    private void FillMemory()
    {
        for (int i=0; i<Memory.Length; i++)
        {
            string newValue = null!;
            for (int index = 0; index < _bitsCount; index++)
            {
                newValue += _random.Next(0, 2).ToString();
            }

            Memory[i] = newValue;
        }
    }

    public void DisplayMemory()
    {
        for (int index = 0; index<Memory.Length; index++)
        {
            Console.WriteLine($"Word №{index+1}: {Memory[index]}");
        }
        Console.Write($"Shortest word is : {FindMinWord(Memory.ToList())}   ");
        Console.WriteLine($"Biggest word is : {FindMaxWord(Memory.ToList())}");
    }

    private char Inverse(char incomeValue)
    {
        return incomeValue == '1' ? '0' : '1';
    }

    private char CompareDigits(char first, char second)
    {
        if (first == second)
        {
            if (first == '1')
            {
                return '1';
            }
        }
        return '0';
    }

    private int GetInt(char value)
    {
        return value == '1' ? 1 : 0;
    }

    private char GreaterTriggerValue(char prevGreaterValue, char aValue, char sValue, char prevLowerValue)
    {
        char firstComparing = CompareDigits(Inverse(aValue), sValue);
        char secondComparing = CompareDigits(firstComparing, Inverse(prevLowerValue));
        if (prevGreaterValue == '1' || secondComparing == '1')
        {
            return '1';
        }
        else
        {
            return '0';
        }
    }
    
    private char LowerTriggerValue(char prevLowerValue, char aValue, char sValue, char prevGreaterValue)
    {
        char firstComparing = CompareDigits(aValue, Inverse(sValue));
        char secondComparing = CompareDigits(firstComparing, Inverse(prevGreaterValue));
        if (prevLowerValue == '1' || secondComparing == '1')
        {
            return '1';
        }
        else
        {
            return '0';
        }
    }

    private int Compare(string word, string compareWith)
    {
        char prevLowerValue = '0', prevGreaterValue = '0';
        for (int i = 0; i < _bitsCount; i++)
        {
            char greaterValue = GreaterTriggerValue(prevGreaterValue, compareWith[i], word[i], prevLowerValue);
            char lowerValue = LowerTriggerValue(prevLowerValue, compareWith[i], word[i], prevGreaterValue);
            prevLowerValue = lowerValue;
            prevGreaterValue = greaterValue;
        }

        if (GetInt(prevGreaterValue) > GetInt(prevLowerValue))
        {
            return 1;
        }
        else
        {
            if (GetInt(prevGreaterValue) < GetInt(prevLowerValue))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
    private string FindMinWord(List<string> memory)
    {
        int iteration = memory.Count-1;
        string minimalWord = memory[iteration];
        while (iteration >= 0)
        {
            int compareResult = Compare(memory[iteration], minimalWord);
            if (compareResult >= 0)
            {
                iteration -= 1;
                continue;
            }

            minimalWord = memory[iteration];
            iteration -= 1;
        }

        return minimalWord;
    }
    
    private string FindMaxWord(List<string> memory)
    {
        int iteration = memory.Count-1;
        string maximalWord = memory[iteration];
        while (iteration >= 0)
        {
            int compareResult = Compare(memory[iteration], maximalWord);
            if (compareResult <= 0)
            {
                iteration -= 1;
                continue;
            }

            maximalWord = memory[iteration];
            iteration -= 1;
        }

        return maximalWord;
    }
    
    public void SortMinToMax()
    {
        string[] newMemory = new string[Memory.Length];
        List<string> existingMemory = Memory.ToList();
        for (int round = 0; round < newMemory.Length; round++)
        {
            string minimal = FindMinWord(existingMemory);
            newMemory[round] = minimal;
            existingMemory.Remove(minimal);
        }

        Memory = newMemory;
    }

    public void SortMaxToMin()
    {
        SortMinToMax();
        var reversedMemory = Memory.ToList();
        reversedMemory.Reverse();
        Memory = reversedMemory.ToArray();
    }

    public void Sort()
    {
        SortMinToMax();
        DisplayMemory();
        SortMaxToMin();
        DisplayMemory();
    }

    public string BooleanFunctionSearch(string function)
    {
        TableCreator table = new TableCreator(function);
        string comparingTo = table.GetResultString();
        if (comparingTo.Length == _bitsCount)
        {
           foreach (var t in Memory)
                   {
                       int compareResult = Compare(t, comparingTo);
                       if (compareResult == 0)
                       {
                           return t;
                       }
                   } 
        }
        return String.Empty;
    }

}