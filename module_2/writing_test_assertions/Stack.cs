namespace writing_test_assertions;

using System;

public class Stack
{
    private List<object> _items = new List<object>();

    public void Push(object item)
    {
        _items.Add(item);
    }

    public object Pop()
    {
        if (IsEmpty()) throw new InvalidOperationException("Stack is empty");
        var top = _items[_items.Count - 1];
        _items.RemoveAt(_items.Count - 1);
        return top;
    }

    public object Peek()
    {
        if (IsEmpty()) throw new InvalidOperationException("Stack is empty");
        return _items[_items.Count - 1];
    }

    public bool IsEmpty()
    {
        return _items.Count == 0;
    }
}