using Laba6_AOIS;

HashTable table = new HashTable(3);
table.AddNode("power", "the amount of energy transferred per unit time");
table.AddNode("power", "the amount of energy converted per unit time");
table.AddNode("Power", "the rate at which electrical energy is transferred");
table.AddNode("watt", "the unit of power");
table.ShowTable();
table.FindNode("nothing");
table.DeleteNode("not");
table.AddNode("force","influence that causes the motion of an object with mass to change its velocity");
table.DeleteNode("Power");
table.ShowTable();
table.FindNode("power");
table.AddNode("velocity", "the directional speed of an object in motion");
table.AddNode("speed", "quantity that refers to how fast an object is moving");
table.ShowTable();
table.FindNode("velocity");