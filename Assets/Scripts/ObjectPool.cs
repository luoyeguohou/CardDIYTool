using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    private readonly Stack<T> _pool;

    private readonly Func<T> _createFunc;
    private readonly Action<T> _onGet;
    private readonly Action<T> _onRelease;

    public int Count => _pool.Count;

    public ObjectPool(
        Func<T> createFunc,
        Action<T> onGet = null,
        Action<T> onRelease = null,
        int initialCount = 0)
    {
        _pool = new Stack<T>();

        _createFunc = createFunc;
        _onGet = onGet;
        _onRelease = onRelease;

        for (int i = 0; i < initialCount; i++)
        {
            _pool.Push(_createFunc());
        }
    }

    public T Get()
    {
        T obj;

        if (_pool.Count > 0)
        {
            obj = _pool.Pop();
        }
        else
        {
            obj = _createFunc();
        }

        _onGet?.Invoke(obj);

        return obj;
    }

    public void Release(T obj)
    {
        _onRelease?.Invoke(obj);

        _pool.Push(obj);
    }

    public void Clear()
    {
        _pool.Clear();
    }
}