using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using TreeApp;
using Point = System.Drawing.Point;

namespace Trees;

public class BinaryNode<T>
{
    #region Properties
    
    public T Value { get; set; }
    public BinaryNode<T>? Left { get; set; }
    public BinaryNode<T>? Right { get; set; }

    public Avalonia.Point Center { get; private set; }
    public Rect SubtreeBounds { get; private set; }

    #endregion

    #region Constants
    
    private const double NODE_RADIUS = 10;
    private const double X_SPACING = 20; 
    private const double Y_SPACING = 20;
    
    #endregion
    
    #region Constructors
    
    public BinaryNode(T value)
    {
        Value = value;
        Left = null;
        Right = null;
    }
    
    #endregion

    #region Methods
    
    public void AddLeftChild(T value) => Left = new BinaryNode<T>(value);
    public void AddRightChild(T value) => Right = new BinaryNode<T>(value);

    internal void AddLeft(BinaryNode<T> child)
    {
        Left = child;
    }

    internal void AddRight(BinaryNode<T> child)
    {
        Right = child;
    }
    
    public override string? ToString()
    {
        return ToString(0);
    }

    public string ToString(int level)
    {
        level = level + 1;
        const string space = " ";
        var sb = new StringBuilder();
            sb.AppendLine($"{Value}:");
            if(Left != null)
                sb.AppendLine(space.PadLeft(level) + Left?.ToString(level));
            else
                sb.AppendLine(space.PadLeft(level)+"None");
            if(Right != null)
                sb.Append(space.PadLeft(level) + Right?.ToString(level));
            else
                sb.AppendLine(space.PadLeft(level)+"None");
            
        return sb.ToString().Trim();
    }
    
    public BinaryNode<T>? FindNode(T target)
    {
        if (Value != null && Value.Equals(target)) return this;
        if (Left != null)
        {
            var result = Left.FindNode(target);
            if (result != null) return result;
        }
        if (Right != null)
        {
            var result = Right.FindNode(target);
            if (result != null) return result;
        }
        return null;
    }
    
    #region Traverses
    
    public string TraverseInOrder()
    {
        var sb = new StringBuilder();
        if (Left != null)
            sb.Append(Left.TraverseInOrder());
        sb.Append(Value);
        if (Right != null)
            sb.Append(Right.TraverseInOrder());
        return sb.ToString();
    }
    
    public string TraversePreOrder()
    {
        var sb = new StringBuilder();
        sb.Append(Value);
        if (Left != null)
            sb.Append(Left.TraversePreOrder());
        if (Right != null)
            sb.Append(Right.TraversePreOrder());
        return sb.ToString();
    }
    
    public string TraversePostOrder()
    {
        var sb = new StringBuilder();
        if (Left != null)
            sb.Append(Left.TraversePostOrder());
        if (Right != null)
            sb.Append(Right.TraversePostOrder());
        sb.Append(Value);
        return sb.ToString();
    }
    
    public string TraverseBreadthFirst()
    {
        var sb = new StringBuilder();
        var queue = new Queue<BinaryNode<T>>();
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sb.Append(current.Value);
            if (current.Left != null)
                queue.Enqueue(current.Left);
            if (current.Right != null)
                queue.Enqueue(current.Right);
        }
        return sb.ToString();
    }
    
    #endregion
    
    #region Drawing
    
        private void ArrangeSubtree(double xmin, double ymin)
        {
            var cy = ymin + NODE_RADIUS;
            
            if ((Left == null) && (Right == null))
            {
                var cx = xmin + NODE_RADIUS;
                Center = new Avalonia.Point(cx, cy);
                SubtreeBounds = new Rect(xmin, ymin, 2 * NODE_RADIUS, 2 * NODE_RADIUS);
                return;
            }

            var childXmin = xmin;
            var childYmin = ymin + 2 * NODE_RADIUS + Y_SPACING;

            if (Left != null)
            {

                Left.ArrangeSubtree(childXmin, childYmin);
                childXmin = Left.SubtreeBounds.Right;

                if (Right != null) childXmin += X_SPACING;
            }

            Right?.ArrangeSubtree(childXmin, childYmin);

            if ((Left != null) && (Right != null))
            {
                // Two children. Center this node over the child nodes.
                // Use the subtree bounds to set our subtree bounds.
                var cx = (Left.Center.X + Right.Center.X) / 2;
                Center = new Avalonia.Point(cx, cy);
                var xmax = Right.SubtreeBounds.Right;
                var ymax = Math.Max(Left.SubtreeBounds.Bottom, Right.SubtreeBounds.Bottom);
                SubtreeBounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
            }
            else if (Left != null)
            {
                // We have only a left child.
                var cx = Left.Center.X;
                Center = new Avalonia.Point(cx, cy);
                var xmax = Left.SubtreeBounds.Right;
                var ymax = Left.SubtreeBounds.Bottom;
                SubtreeBounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
            }
            else
            {
                // We have only a right child.
                if (Right != null)
                {
                    var cx = Right.Center.X;
                    Center = new Avalonia.Point(cx, cy);
                }

                var xmax = Right.SubtreeBounds.Right;
                var ymax = Right.SubtreeBounds.Bottom;
                SubtreeBounds = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
            }
        }

        private void DrawSubtreeLinks(Canvas canvas)
        {
            // Draw the subtree's links.
            if (Left != null)
            {
                Left.DrawSubtreeLinks(canvas);
                canvas.DrawLine(Center, Left.Center, new SolidColorBrush(){Color=Colors.Black} , 1);
            }

            if (Right != null)
            {
                Right.DrawSubtreeLinks(canvas);
                canvas.DrawLine(Center, Right.Center, new SolidColorBrush(){Color=Colors.Black}, 1);
            }

            // Outline the subtree for debugging.
            //canvas.DrawRectangle(SubtreeBounds, null, Brushes.Red, 1);
        }

        // Draw the subtree's nodes.
        private void DrawSubtreeNodes(Canvas canvas)
        {
            // Draw the node.
            var x0 = Center.X - NODE_RADIUS;
            var y0 = Center.Y - NODE_RADIUS;
            var rect = new Rect(
                Center.X - NODE_RADIUS,
                Center.Y - NODE_RADIUS,
                2 * NODE_RADIUS,
                2 * NODE_RADIUS);
            canvas.DrawEllipse(rect, new SolidColorBrush(){Color=Colors.White}, new SolidColorBrush(){Color=Colors.Green}, 1);

            var label = canvas.DrawLabel(
                rect, Value, null, new SolidColorBrush(){Color=Colors.Red},
                HorizontalAlignment.Center,
                VerticalAlignment.Center,
                12, 0);

            // Draw the descendants' nodes.
            if (Left != null) Left.DrawSubtreeNodes(canvas);
            if (Right != null) Right.DrawSubtreeNodes(canvas);
        }

        // Position and draw the subtree.
        public void ArrangeAndDrawSubtree(Canvas canvas, double xmin, double ymin)
        {
            // Position the tree.
            ArrangeSubtree(xmin, ymin);

            // Draw the links.
            DrawSubtreeLinks(canvas);

            // Draw the nodes.
            DrawSubtreeNodes(canvas);
        }
   
    
    #endregion
    
    #endregion
}