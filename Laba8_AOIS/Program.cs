using Laba8_AOIS;

Processor processor = new Processor(16);
processor.DisplayMemory();
Console.WriteLine($"Searching word is : {processor.GetWordAt(1)}");
processor.Diagonalize();
Console.WriteLine($"Searching word is : {processor.GetWordAt(10)}");
processor.DisplayMemory();
processor.Normalize();
processor.DisplayMemory();
processor.ReWriteWordAt(10, "0010010010010011");
Console.WriteLine($"Searching word is : {processor.GetWordAt(10)}");
//processor.SortMinToMax();
processor.DisplayMemory();
processor.SearchByCorrespondence("0000000000000000");
Console.WriteLine($"Searching function is : {processor.GetFunction(1, 16, "f5")}");
processor.Summarize("001");
processor.DisplayMemory();
