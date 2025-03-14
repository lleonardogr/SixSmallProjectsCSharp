using System.Text;

namespace Trees;

public class NaryNode<T>
{
    public T Value { get; set; }
    public List<NaryNode<T>> Children { get; set; }
    
    public NaryNode(T value)
    {
        Value = value;
        Children = new List<NaryNode<T>>();
    }
    
    public void AddChild(T value) => Children.Add(new NaryNode<T>(value));

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
}