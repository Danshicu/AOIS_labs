using Laba7_AOIS;

Processor processor = new Processor(30, 8);
processor.DisplayMemory();
processor.Sort();
string functionToSearch = "(x*y*n)";
string searchedFunction = processor.BooleanFunctionSearch(functionToSearch);
if (searchedFunction == String.Empty)
{
    Console.WriteLine("Function wasn't found");
}
else
{
    Console.WriteLine($"Boolean function is : {searchedFunction}");
}