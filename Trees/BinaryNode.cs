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
        return $"{Value}: {(Left == null  ? "null" : Left.Value)} " +
               $"{(Right == null  ? "null" : Right.Value)}";
    }
    
    
}