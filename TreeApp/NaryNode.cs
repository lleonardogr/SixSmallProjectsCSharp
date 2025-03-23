using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using TreeApp;

namespace Trees;

public class NaryNode<T>
{
    public T Value { get; set; }
    public List<NaryNode<T>> Children { get; set; }
    
    public bool IsLeaf => Children.Count == 0;
    public bool IsTwig => Children.Count > 0;

    private const double BOX_HALF_WIDTH = 80 / 2;
    private const double BOX_HALF_HEIGHT = 40 / 2;
    private const double X_SPACING = 20; // Horizontal distance between neighboring subtrees
    private const double Y_SPACING = 20; // Horizontal distance between parent and child subtree
    internal Point Center { get; private set; }
    internal Rect SubtreeBounds { get; private set; }
    
    public NaryNode(T value)
    {
        Value = value;
        Children = new List<NaryNode<T>>();
    }

    public void AddChild(T value) => Children.Add(new NaryNode<T>(value));

    public void AddChild(NaryNode<T> child) => Children.Add(child);

    public override string ToString()
    {
        const string space = " ";
        var sb = new StringBuilder();
        sb.AppendLine($"{Value}:");

        return ToString(0);
    }

    public string ToString(int level)
    {
        level = level + 1;
        const string space = " ";
        var sb = new StringBuilder();
        sb.AppendLine($"{Value}:");
        foreach (var child in Children)
        {
            sb.AppendLine(space.PadLeft(level) + child.ToString(level));
        }

        return sb.ToString().Trim();

    }

    public NaryNode<T> FindNode(T target)
    {
        if (Value.Equals(target)) return this;
        foreach (var child in Children)
        {
            var result = child.FindNode(target);
            if (result != null) return result;
        }

        return null;
    }
    public string TraversePreOrder()
    {
        var sb = new StringBuilder();
        sb.Append(Value);
        if (Children.Count > 0)
        {
            foreach (var child in Children)
            {
                sb.Append(child.TraversePreOrder());
            }
        }

        return sb.ToString();
    }
    public string TraversePostOrder()
    {
        var sb = new StringBuilder();
        if (Children.Count > 0)
        {
            foreach (var child in Children)
            {
                sb.Append(child.TraversePostOrder());
            }
        }

        sb.Append(Value);
        return sb.ToString();
    }

    public string TraverseBreadthFirst()
    {
        var sb = new StringBuilder();
        var queue = new Queue<NaryNode<T>>();
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sb.Append(current.Value);
            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }
        
        

        return sb.ToString();
    }
    
    #region Drawing

/// <summary>
    /// Arranges this node and its subtrees in a hierarchical layout.
    /// Special handling for leaf nodes: positioned vertically below parent.
    /// </summary>
private void ArrangeSubtree(double xmin, double ymin)
{
    double cx, cy;

    // Calculate the Y position for this node
    cy = ymin + BOX_HALF_HEIGHT;

    // Handle leaf nodes (nodes with no children)
    if (Children.Count == 0)
    {
        // For a leaf node, simply position it at the minimum coordinates
        cx = xmin + BOX_HALF_WIDTH;
        Center = new Point(cx, cy);
        // Set bounds to just contain this node
        SubtreeBounds = new Rect(xmin, ymin, 2 * BOX_HALF_WIDTH, 2 * BOX_HALF_HEIGHT);
        return;
    }

    // Count how many leaf and non-leaf children we have
    var leafCount = 0;
    var nonLeafCount = 0;
    foreach (var child in Children)
    {
        if (child.IsLeaf) leafCount++;
        else nonLeafCount++;
    }

    // Calculate starting positions for child subtrees
    var child_xmin = xmin;
    var child_ymin = ymin + 2 * BOX_HALF_HEIGHT + Y_SPACING;
    var ymax = ymin + 2 * BOX_HALF_HEIGHT;
    var leafYMin = child_ymin;
    
    // Process non-leaf children first (horizontally)
    foreach (var child in Children.Where(child => !child.IsLeaf))
    {
        child.ArrangeSubtree(child_xmin, child_ymin);
            
        // Move the next child's position
        child_xmin = child.SubtreeBounds.Right + X_SPACING;
            
        // Track lowest point
        if (ymax < child.SubtreeBounds.Bottom)
            ymax = child.SubtreeBounds.Bottom;
    }
    
    // Reserve space for the non-leaf children before placing leaf nodes
    var leafXMin = nonLeafCount > 0 ? child_xmin : xmin;
    
    // If we have leaf nodes, arrange them vertically
    if (leafCount > 0)
    {
        // Center the vertical stack of leaves
        var leafX = leafXMin + BOX_HALF_WIDTH;
        var currentLeafY = leafYMin;
        
        foreach (var child 
                 in Children.Where(child => child.IsLeaf))
        {
            // Position leaf vertically
            child.Center = new Point(leafX, currentLeafY + BOX_HALF_HEIGHT);
            child.SubtreeBounds = new Rect(
                leafX - BOX_HALF_WIDTH, currentLeafY,
                2 * BOX_HALF_WIDTH, 2 * BOX_HALF_HEIGHT);
                
            // Move down for next leaf
            currentLeafY += 2 * BOX_HALF_HEIGHT + Y_SPACING;
                
            // Update max y coordinate
            if (ymax < currentLeafY)
                ymax = currentLeafY;
        }
        
        // Adjust xmax to include leaf nodes width
        child_xmin = Math.Max(child_xmin, leafXMin + 2 * BOX_HALF_WIDTH);
    }

    // Calculate bounds and center position
    var xmax = child_xmin;
    SubtreeBounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
    cx = (SubtreeBounds.X + SubtreeBounds.Right) / 2;
    Center = new Point(cx, cy);
}

/// <summary>
/// Draws connection lines between this node and its children.
/// Special handling for vertically stacked leaf nodes.
/// </summary>
private void DrawSubtreeLinks(Canvas canvas)
{
    if (Children.Count == 0) return;
    
    // Group children by leaf/non-leaf
    var leafChildren = new List<NaryNode<T>>();
    List<NaryNode<T>> nonLeafChildren = new List<NaryNode<T>>();
    
    foreach (var child in Children)
    {
        if (child.IsLeaf)
            leafChildren.Add(child);
        else
            nonLeafChildren.Add(child);
    }
    
    // Handle non-leaf children (horizontal arrangement)
    if (nonLeafChildren.Count > 0)
    {
        var ymid = (Center.Y + nonLeafChildren[0].Center.Y) / 2;
        canvas.DrawLine(Center, new Point(Center.X, ymid), new SolidColorBrush(Colors.Green), 1);
        
        var lastChild = nonLeafChildren.Count - 1;
        if (lastChild > 0) // Multiple non-leaf children
        {
            canvas.DrawLine(
                new Point(nonLeafChildren[0].Center.X, ymid),
                new Point(nonLeafChildren[lastChild].Center.X, ymid),
                new SolidColorBrush(Colors.Green), 1);
        }
        
        foreach (NaryNode<T> child in nonLeafChildren)
        {
            canvas.DrawLine(
                new Point(child.Center.X, ymid),
                new Point(child.Center.X, child.Center.Y),
                new SolidColorBrush(Colors.Green), 1);
                
            child.DrawSubtreeLinks(canvas);
        }
    }
    
    // Handle leaf children (vertically stacked)
    if (leafChildren.Count > 0)
    {
        // Find the vertical line position
        var leafX = leafChildren[0].Center.X;
        
        // Draw vertical line from parent to the middle of leaf stack
        var midY = (leafChildren[0].Center.Y + leafChildren[leafChildren.Count - 1].Center.Y) / 2;
        
        if (nonLeafChildren.Count == 0)
        {
            // Draw direct line to middle if only leaf children
            canvas.DrawLine(Center, new Point(leafX, midY), new SolidColorBrush(Colors.Purple), 1.5);
        }
        else
        {
            // If we have both types, connect to the horizontal line
            var nonLeafYmid = (Center.Y + nonLeafChildren[0].Center.Y) / 2;
            canvas.DrawLine(
                new Point(leafX, nonLeafYmid),
                new Point(leafX, midY),
                new SolidColorBrush(Colors.Purple), 1.5);
        }
        
        // Draw vertical line connecting all leaf nodes
        canvas.DrawLine(
            new Point(leafX, leafChildren[0].Center.Y),
            new Point(leafX, leafChildren[leafChildren.Count - 1].Center.Y),
            new SolidColorBrush(Colors.Purple), 1.5);
        
        // Draw horizontal lines to each leaf
        foreach (NaryNode<T> leaf in leafChildren)
        {
            canvas.DrawLine(
                new Point(leafX, leaf.Center.Y),
                leaf.Center,
                new SolidColorBrush(Colors.Purple), 1.5);
        }
    }
}

/// <summary>
/// Draws the visual representation of nodes in the tree.
/// - Leaf nodes: Light green fill
/// - Non-leaf nodes: Pink fill
/// - Twig nodes: Blue border
/// </summary>
private void DrawSubtreeNodes(Canvas canvas)
{
    // Calculate the bounding rectangle for this node
    var x0 = Center.X - BOX_HALF_WIDTH;
    var y0 = Center.Y - BOX_HALF_HEIGHT;
    var width = BOX_HALF_WIDTH * 2;
    var height = BOX_HALF_HEIGHT * 2;
    var rect = new Rect(x0, y0, width, height);

    // Select styling based on node type
    Brush fillBrush;
    Brush strokeBrush;
    double strokeThickness = 1;

    if (IsLeaf)
    {
        // Leaf node - light green fill
        fillBrush = new SolidColorBrush(Color.Parse("#E0FFE0"));
        strokeBrush = new SolidColorBrush(Colors.Black);
    }
    else
    {
        // Non-leaf node - pink fill
        fillBrush = new SolidColorBrush(Color.Parse("#FFE0E0")); // Pink
        
        if (IsTwig)
        {
            // Twig node - blue border with thicker stroke
            strokeBrush = new SolidColorBrush(Colors.Blue);
            strokeThickness = 2;
        }
        else
        {
            // Branch node - standard border
            strokeBrush = new SolidColorBrush(Colors.Black);
        }
    }

    // Draw the node
    canvas.DrawRectangle(rect, fillBrush, strokeBrush, strokeThickness);

    // Draw the value
    canvas.DrawLabel(rect, Value, null, new SolidColorBrush(Colors.Red),
        HorizontalAlignment.Center,
        VerticalAlignment.Center,
        12, 0);

    // Draw children
    foreach (NaryNode<T> child in Children)
    {
        child.DrawSubtreeNodes(canvas);
    }
}

/// <summary>
/// Main method that positions the tree and draws it on the canvas.
///
/// Theory: N-ary tree visualization follows three sequential steps:
/// 1. Position calculation - determining where each node should be placed
/// 2. Drawing connections - showing parent-child relationships
/// 3. Drawing nodes - visualizing the actual data elements
///
/// This sequence ensures proper layering (connections behind nodes)
/// </summary>
/// <param name="canvas">The canvas to draw on</param>
/// <param name="xmin">Starting X coordinate for the tree</param>
/// <param name="ymin">Starting Y coordinate for the tree</param>
public void ArrangeAndDrawSubtree(Canvas canvas, double xmin, double ymin)
{
    // Step 1: Calculate positions for all nodes in the tree
    ArrangeSubtree(xmin, ymin);

    // Step 2: Draw all connections between nodes
    DrawSubtreeLinks(canvas);

    // Step 3: Draw all nodes with their values
    DrawSubtreeNodes(canvas);
}

#endregion
}
