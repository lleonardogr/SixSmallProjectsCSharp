namespace nary_node1;

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
    
    public override string? ToString()
    {
        var children = Children.Select(c => c.Value).ToList();
        return $"{Value}: {string.Join(", ", children)}";
    }
}