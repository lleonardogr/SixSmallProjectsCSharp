// See https://aka.ms/new-console-template for more information

using binary_node1;

Console.WriteLine("Hello to binary app!");

var test_node = new BinaryNode<string>("Root");
test_node.AddLeftChild("A");
test_node.AddRightChild("B");
test_node.Left?.AddLeftChild("C");
test_node.Left?.AddRightChild("D");
test_node.Right?.AddRightChild("E");
test_node.Right?.Right?.AddRightChild("F");

Console.WriteLine(test_node.ToString());
Console.WriteLine(test_node.Left?.ToString());
Console.WriteLine(test_node.Right?.ToString());
Console.WriteLine(test_node.Left?.Left?.ToString());
Console.WriteLine(test_node.Left?.Right?.ToString());
Console.WriteLine(test_node.Right?.Right?.ToString());
Console.WriteLine(test_node.Right?.Right?.Right?.ToString());


