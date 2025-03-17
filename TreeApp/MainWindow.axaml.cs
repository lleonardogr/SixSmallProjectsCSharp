using Avalonia.Controls;
using Avalonia.Interactivity;
using Trees;

namespace TreeApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        //DrawBinaryNode();
        DrawNaryNode();
    }
    
    public void DrawBinaryNode()
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

    public void DrawNaryNode()
    {
        // Build a test tree.
        // A
        //         |
        //     +---+---+
        // B   C   D
        //     |       |
        //    +-+      +
        // E F      G
        //    |        |
        //    +      +-+-+
        // H      I J K
        var node_a = new NaryNode<string>("A");
       var node_b = new NaryNode<string>("B");
        var node_c = new NaryNode<string>("C");
        var node_d = new NaryNode<string>("D");
        var node_e = new NaryNode<string>("E");
        var node_f = new NaryNode<string>("F");
        var node_g = new NaryNode<string>("G");
        var node_h = new NaryNode<string>("H");
        var node_i = new NaryNode<string>("I");
        var node_j = new NaryNode<string>("J");
        var node_k = new NaryNode<string>("K");

        node_a.AddChild(node_b);
        node_a.AddChild(node_c);
        node_a.AddChild(node_d);
        node_b.AddChild(node_e);
        node_b.AddChild(node_f);
        node_d.AddChild(node_g);
        node_e.AddChild(node_h);
        node_g.AddChild(node_i);
        node_g.AddChild(node_j);
        node_g.AddChild(node_k);

        // Draw the tree.
        node_a.ArrangeAndDrawSubtree(MainCanvas, 10, 10);
    }
}