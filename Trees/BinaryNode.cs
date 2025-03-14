using System.Text;

namespace Trees;

public class BinaryNode<T>
{
    public T Value { get; set; }
    public BinaryNode<T>? Left { get; set; }
    public BinaryNode<T>? Right { get; set; }

    public BinaryNode(T value)
    {
        Value = value;
        Left = null;
        Right = null;
    }

    public void AddLeftChild(T value) => Left = new BinaryNode<T>(value);
    public void AddRightChild(T value) => Right = new BinaryNode<T>(value);

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
        if (Value.Equals(target)) return this;
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
}