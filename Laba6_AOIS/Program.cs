// See https://aka.ms/new-console-template for more information

using Laba6_AOIS;

Console.WriteLine("\nHello, World!");
HashTable table = new HashTable(30);
table.AddNode("ab", "some");
table.AddNode("ab", "interesting");
table.AddNode("not", "collision");
table.AddNode("ab", "info");
table.FindNode("not");
table.ShowTable();
table.DeleteNode("not");
table.ShowTable();
table.AddNode("SOMEINFO","XZ");
table.DeleteNode("ab");
table.DeleteNode("th");
table.FindNode("SOMEINFO");
table.FindNode("XZ");
table.AddNode("SOMEINFO", "NEW");
table.AddNode("SOMEINFO", "INFO");
table.ShowTable();