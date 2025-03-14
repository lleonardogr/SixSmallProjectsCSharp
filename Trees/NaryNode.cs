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
        var sb = new StringBuilder();
        sb.Append($"{Value}: ");
        foreach (var child in Children)
        {
            sb.Append($"{child.Value} ");
        }
        sb.Append("\n");
        foreach (var child in Children)
        {
            sb.Append(child.ToString());
        }
        return sb.ToString();
    }
}