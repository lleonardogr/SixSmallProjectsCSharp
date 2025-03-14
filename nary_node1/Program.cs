// See https://aka.ms/new-console-template for more information

using nary_node1;

Console.WriteLine("Hello, NaryNode!");

var nary_node = new NaryNode<string>("Root");
nary_node.AddChild("A");
nary_node.AddChild("B");
nary_node.AddChild("C");
nary_node.Children[0].AddChild("D");
nary_node.Children[0].AddChild("E");
nary_node.Children[2].AddChild("F");
nary_node.Children[0].Children[0].AddChild("G");
nary_node.Children[2].Children[0].AddChild("H");
nary_node.Children[2].Children[0].AddChild("I");

Console.WriteLine(nary_node);
Console.WriteLine(nary_node.Children[0]);
Console.WriteLine(nary_node.Children[1]);
Console.WriteLine(nary_node.Children[2]);
Console.WriteLine(nary_node.Children[0].Children[0]);
Console.WriteLine(nary_node.Children[0].Children[1]);
Console.WriteLine(nary_node.Children[2].Children[0]);
Console.WriteLine(nary_node.Children[0].Children[0].Children[0]);
Console.WriteLine(nary_node.Children[2].Children[0].Children[0]);
Console.WriteLine(nary_node.Children[2].Children[0].Children[1]);
