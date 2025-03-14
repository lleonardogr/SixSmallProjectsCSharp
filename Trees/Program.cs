﻿// See https://aka.ms/new-console-template for more information

using Trees;

Console.WriteLine("Hello, World!");

var test_node = new BinaryNode<string>("Root");
test_node.AddLeftChild("A");
test_node.AddRightChild("B");
test_node.Left?.AddLeftChild("C");
test_node.Left?.AddRightChild("D");
test_node.Right?.AddRightChild("E");
test_node.Right?.Right?.AddRightChild("F");

var texto = test_node.ToString();
Console.WriteLine(texto);

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

Console.WriteLine(nary_node.ToString());

FindValue(test_node, "Root");
FindValue(test_node, "E");
FindValue(test_node, "F");
FindValue(test_node, "Q");

// Find F in the B subtree.
//FindValue(b, "F");

FindValue(test_node, "F"); 

void FindValueNary(NaryNode<string> root, string target)
{
    NaryNode<string> node = root.FindNode(target);
    if (node == null)
        Console.WriteLine(string.Format("Value {0} not found", target));
    else
        Console.WriteLine(string.Format("Found {0}", node.Value));
}

void FindValue<T>(BinaryNode<T> root, T target)
{
    BinaryNode<T> node = root.FindNode(target);
    if (node == null)
        Console.WriteLine(string.Format("Value {0} not found", target));
    else
        Console.WriteLine(string.Format("Found {0}", node.Value));
}