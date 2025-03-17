using System.Collections.Generic;
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

    private const double NODE_RADIUS = 10; // Radius of a node’s circle
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

    // Position the node's subtree.
    private void ArrangeSubtree(double xmin, double ymin)
    {
        double cx, cy;

        // Calculate cy, the Y coordinate for this node.
        // This doesn't depend on the children.
        cy = ymin + NODE_RADIUS;

        // If the node has no children, just place it here and return.
        if (Children.Count == 0)
        {
            cx = xmin + NODE_RADIUS;
            Center = new Point(cx, cy);
            SubtreeBounds = new Rect(xmin, ymin, 2 * NODE_RADIUS, 2 * NODE_RADIUS);
            return;
        }

        // Set child_xmin and child_ymin to the
        // start position for child subtrees.
        double child_xmin = xmin;
        double child_ymin = ymin + 2 * NODE_RADIUS + Y_SPACING;

        // Set ymax equal to the largest Y position used.
        double ymax = ymin + 2 * NODE_RADIUS;

        // Position the child subtrees.
        foreach (NaryNode<T> child in Children)
        {
            // Position this child subtree.
            child.ArrangeSubtree(child_xmin, child_ymin);

            // Update child_xmin to allow room for the subtree
            // and space between the subtrees.
            child_xmin = child.SubtreeBounds.Right + X_SPACING;

            // Update the subtree bottom ymax.
            if (ymax < child.SubtreeBounds.Bottom)
                ymax = child.SubtreeBounds.Bottom;
        }

        // Set xmax equal to child_xmin minus the horizontal
        // spacing we added after the last subtree.
        double xmax = child_xmin - X_SPACING;

        // Use xmin, ymin, xmax, and ymax to set our subtree bounds.
        SubtreeBounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);

        // Center this node over the subtree bounds.
        cx = (SubtreeBounds.X + SubtreeBounds.Right) / 2;
        cy = ymin + NODE_RADIUS;
        Center = new Point(cx, cy);
    }

    // Draw the subtree's links.
    private void DrawSubtreeLinks(Canvas canvas)
    {
        // If we have exactly one child, just draw to it.
        if (Children.Count == 1)
        {
            NaryNode<T> child = Children[0];
            canvas.DrawLine(Center, child.Center, new SolidColorBrush(Colors.Green), 1);
            child.DrawSubtreeLinks(canvas);
        }
        else if (Children.Count > 0)
        {
            // Else if we have more than one child,
            // draw vertical and horizontal branches.

            // Find the Y coordinate of the center
            // halfway to the children.
            double ymid = (Center.Y + Children[0].Center.Y) / 2;

            // Draw the vertical line to the center line.
            canvas.DrawLine(Center, new Point(Center.X, ymid), new SolidColorBrush(Colors.Green), 1);

            // Draw the horizontal center line over the children.
            int last_child = Children.Count - 1;
            canvas.DrawLine(
                new Point(Children[0].Center.X, ymid),
                new Point(Children[last_child].Center.X, ymid),
                new SolidColorBrush(Colors.Green), 1);

            // Draw lines from the center line to the children.
            foreach (NaryNode<T> child in Children)
            {
                canvas.DrawLine(
                    new Point(child.Center.X, ymid),
                    new Point(child.Center.X, child.Center.Y),
                    new SolidColorBrush(Colors.Green), 1);
            }

            // Recursively draw child subtree links.
            foreach (NaryNode<T> child in Children)
            {
                child.DrawSubtreeLinks(canvas);
            }
        }

        // Outline the subtree for debugging.
        //canvas.DrawRectangle(SubtreeBounds, null, Brushes.Red, 1);
    }

    // Draw the subtree's nodes.
    private void DrawSubtreeNodes(Canvas canvas)
    {
        // Draw the node.
        double x0 = Center.X - NODE_RADIUS;
        double y0 = Center.Y - NODE_RADIUS;
        double x1 = Center.X + NODE_RADIUS;
        double y1 = Center.Y + NODE_RADIUS;
        Rect rect = new Rect(x0, y0, x1 - x0, y1 - y0);
        canvas.DrawEllipse(rect, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black), 1);

        Label label = canvas.DrawLabel(rect, Value, null, new SolidColorBrush(Colors.Red),
            HorizontalAlignment.Center,
            VerticalAlignment.Center,
            12, 0);

        // Draw the descendants' nodes.
        foreach (NaryNode<T> child in Children)
        {
            child.DrawSubtreeNodes(canvas);
        }
    }

    public void ArrangeAndDrawSubtree(Canvas canvas, double xmin, double ymin)
    {
        // Position the tree.
        ArrangeSubtree(xmin, ymin);

        // Draw the links.
        DrawSubtreeLinks(canvas);

        // Draw the nodes.
        DrawSubtreeNodes(canvas);
    }
}