using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using Avalonia;
using Trees;

namespace TreeApp;

public partial class SortedTrees : Window
{
    private BinaryNode<int>? _rootNode;
    
    public SortedTrees()
    {
        InitializeComponent();
    }
    
    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        // Initialize with an empty tree
        ResetTree();
    }
    
    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (int.TryParse(ValueTextBox.Text, out int value))
        {
            if (_rootNode == null)
            {
                _rootNode = new BinaryNode<int>(value);
            }
            else
            {
                AddNodeToTree(_rootNode, value);
            }
            
            RedrawTree();
            ValueTextBox.Text = string.Empty;
        }
    }
    
    private void RemoveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (int.TryParse(ValueTextBox.Text, out int value) && _rootNode != null)
        {
            _rootNode = RemoveNodeFromTree(_rootNode, value);
            RedrawTree();
            ValueTextBox.Text = string.Empty;
        }
    }
    
    private void FindButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (int.TryParse(ValueTextBox.Text, out int value) && _rootNode != null)
        {
            var found = FindNodeInTree(_rootNode, value);
            if (found != null)
            {
                // Highlight the found node
                MainCanvas.Children.Clear();
                found.DrawWithHighlight(MainCanvas, 10, 10);
            }
            else
            {
                // Node not found, just redraw the tree without highlighting
                RedrawTree();
            }
            ValueTextBox.Text = string.Empty;
        }
    }
    
    private void ResetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ResetTree();
    }
    
    private void ResetTree()
    {
        _rootNode = null;
        MainCanvas.Children.Clear();
    }
    
    private void RedrawTree()
    {
        MainCanvas.Children.Clear();
        if (_rootNode != null)
        {
            _rootNode.ArrangeAndDrawSubtree(MainCanvas, 10, 10);
        }
    }
    
    private void AddNodeToTree(BinaryNode<int> node, int value)
    {
        if (value < node.Value)
        {
            if (node.Left == null)
            {
                node.AddLeft(new BinaryNode<int>(value));
            }
            else
            {
                AddNodeToTree(node.Left as BinaryNode<int>, value);
            }
        }
        else if (value > node.Value)
        {
            if (node.Right == null)
            {
                node.AddRight(new BinaryNode<int>(value));
            }
            else
            {
                AddNodeToTree(node.Right as BinaryNode<int>, value);
            }
        }
        // If value is equal to node.Value, do nothing (no duplicates)
    }
    
    private BinaryNode<int>? RemoveNodeFromTree(BinaryNode<int> root, int value)
    {
        if (root == null)
            return null;
            
        if (value < root.Value)
        {
            root.Left = RemoveNodeFromTree(root.Left as BinaryNode<int>, value);
        }
        else if (value > root.Value)
        {
            root.Right = RemoveNodeFromTree(root.Right as BinaryNode<int>, value);
        }
        else
        {
            // Node with only one child or no child
            if (root.Left == null)
                return root.Right as BinaryNode<int>;
            else if (root.Right == null)
                return root.Left as BinaryNode<int>;
                
            // Node with two children
            // Get the inorder successor (smallest in the right subtree)
            root.Value = FindMinValue(root.Right as BinaryNode<int>);
            
            // Delete the inorder successor
            root.Right = RemoveNodeFromTree(root.Right as BinaryNode<int>, root.Value);
        }
        
        return root;
    }
    
    private int FindMinValue(BinaryNode<int> node)
    {
        int minValue = node.Value;
        while (node.Left != null)
        {
            minValue = (node.Left as BinaryNode<int>).Value;
            node = node.Left as BinaryNode<int>;
        }
        return minValue;
    }
    
    private BinaryNode<int>? FindNodeInTree(BinaryNode<int> node, int value)
    {
        if (node == null)
            return null;
            
        if (node.Value == value)
            return node;
            
        if (value < node.Value)
            return FindNodeInTree(node.Left as BinaryNode<int>, value);
            
        return FindNodeInTree(node.Right as BinaryNode<int>, value);
    }

    private void DrawBinaryNode()
    {
        // Build a test tree.
        //         A
        //        / \
        //       /   \
        //      /     \
        //     B       C
        //    / \     / \
        //   D   E   F   G
        //      / \     /
        //     H   I   J
        //            / \
        //           K   L
        var node_a = new BinaryNode<string>("A");
        var node_b = new BinaryNode<string>("B");
        var node_c = new BinaryNode<string>("C");
        var node_d = new BinaryNode<string>("D");
        var node_e = new BinaryNode<string>("E");
        var node_f = new BinaryNode<string>("F");
        var node_g = new BinaryNode<string>("G");
        var node_h = new BinaryNode<string>("H");
        var node_i = new BinaryNode<string>("I");
        var node_j = new BinaryNode<string>("J");
        var node_k = new BinaryNode<string>("K");
        var node_l = new BinaryNode<string>("L");

        node_a.AddLeft(node_b);
        node_a.AddRight(node_c);
        node_b.AddLeft(node_d);
        node_b.AddRight(node_e);
        node_c.AddLeft(node_f);
        node_c.AddRight(node_g);
        node_e.AddLeft(node_h);
        node_e.AddRight(node_i);
        node_g.AddLeft(node_j);
        node_j.AddLeft(node_k);
        node_j.AddRight(node_l);

        // Arrange and draw the tree.
        node_a.ArrangeAndDrawSubtree(MainCanvas, 10, 10);
    }
}

// Extension method for highlighting found nodes
public static class BinaryNodeExtensions
{
    public static void DrawWithHighlight(this BinaryNode<int> node, Canvas canvas, int xOffset, int yOffset)
    {
        // First arrange the tree
        node.ArrangeSubtree(xOffset, yOffset);
        
        // Then draw with highlighting
        DrawSubtreeWithHighlight(node, canvas, node);
    }
    
    private static void DrawSubtreeWithHighlight(BinaryNode<int> root, Canvas canvas, BinaryNode<int> highlightNode)
    {
        if (root == null) return;
        
        // Draw the node
        var nodeRect = new Rect(
            root.Center.X - 15, 
            root.Center.Y - 15,
            30, 
            30
        );
        
        // Use different fill color for the highlighted node
        var fillBrush = root == highlightNode ? new SolidColorBrush() { Color = Colors.Yellow } : new SolidColorBrush() { Color = Colors.White };
        var strokeBrush = new SolidColorBrush() { Color = Colors.Black };
        
        // Draw the ellipse using extension method
        canvas.DrawEllipse(nodeRect, fillBrush, strokeBrush, 2);
        
        // Draw the text using extension method
        canvas.DrawLabel(
            nodeRect,
            root.Value.ToString(),
            null,
            new SolidColorBrush() { Color = Colors.Black },
            Avalonia.Layout.HorizontalAlignment.Center,
            Avalonia.Layout.VerticalAlignment.Center,
            12,
            0
        );
        
        // Draw lines to children
        if (root.Left != null)
        {
            var left = root.Left;
            canvas.DrawLine(
                new Avalonia.Point(root.Center.X, root.Center.Y + 15),
                new Avalonia.Point(left.Center.X, left.Center.Y - 15),
                strokeBrush,
                1
            );
            DrawSubtreeWithHighlight(left as BinaryNode<int>, canvas, highlightNode);
        }
        
        if (root.Right != null)
        {
            var right = root.Right;
            canvas.DrawLine(
                new Avalonia.Point(root.Center.X, root.Center.Y + 15),
                new Avalonia.Point(right.Center.X, right.Center.Y - 15),
                strokeBrush,
                1
            );
            DrawSubtreeWithHighlight(right as BinaryNode<int>, canvas, highlightNode);
        }
    }
}
