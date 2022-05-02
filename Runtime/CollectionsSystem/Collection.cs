using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Collection<T> : ScriptableObject, IList<T>, IReadOnlyList<T>,
                                      ICollection where T : ScriptableObject, ICollectionEntry
{
    [SerializeField] private int _initialId = 0;

    public T this[int index]
    {
        get => _entries[index];
        set => Replace(index, value);
    }

    [SerializeField] protected List<T> _entries = new();

    public int Count => _entries.Count;
    public bool IsReadOnly { get; }
    public bool IsSynchronized { get; }
    public object SyncRoot { get; }
    public bool IsFixedSize { get; }

    public void Add(T t) => _entries.Add(t);
    public void Insert(int index, T t) => _entries.Insert(index, t);
    public bool TryGetAtIndex(int index, out T t) => t = _entries?[index];
    public bool Contains(T t) => _entries.Contains(t);
    public int IndexOf(T t) => _entries.IndexOf(t);
    public void Replace(int index, T t) => _entries[index] = t;
    public bool Remove(T t) => _entries.Remove(t);
    public void RemoveAt(int index) => _entries.RemoveAt(index);
    public void Clear() => _entries.Clear();

    // protected virtual void OnAddObject(T t) { }
    //
    // protected virtual void OnRemoveObject(T t) { }
    //
    // protected virtual void OnCollectionChange() { }


    public void CopyTo(Array array, int index)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (array is not T[] t)
            throw new ArgumentException();

        ((ICollection<T>) this).CopyTo(t, index);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (array is not T[] t)
            throw new ArgumentException();

        ((ICollection<T>) this).CopyTo(t, arrayIndex);
    }

    public void Swap(int indexA, int indexB)
    {
        T a = _entries[indexA];
        Replace(indexA, _entries[indexB]);
        Replace(indexB, a);
    }

    public bool TryGetById(int id, out T t)
    {
        for (int i = 0; i < _entries.Count; i++)
            if (TryGetAtIndex(i, out t) && t.Id == id)
                return true;

        t = default;
        return default;
    }

    public bool TryGetByName(string name, out T t)
    {
        for (int i = 0; i < _entries.Count; i++)
            if (TryGetAtIndex(i, out t) && t.Name == name)
                return true;

        t = default;
        return false;
    }

    public int GetFirstAvaliableId()
    {
        if (_entries.Count <= 0)
            return _initialId;

        int freeId = _initialId;
        bool found = false;
        while (!found)
        {
            found = true;

            if (_entries.All(t => t.Id != freeId)) continue;

            found = false;
            freeId++;
        }

        return freeId;
    }

    public int GetNextId()
    {
        if (_entries.Count <= 0)
            return _initialId;

        int maxId = _initialId;
        for (int i = 0; i < _entries.Count; i++)
            if (TryGetAtIndex(i, out T asset) && asset.Id > maxId)
                maxId = asset.Id;

        return maxId + 1;
    }

    public bool ContainsDuplicateId()
    {
        for (int i = 0; i < _entries.Count - 1; i++)
        {
            T asset1 = _entries[i];

            for (int j = i + 1; j < _entries.Count; j++)
                if (asset1.Id == _entries[j].Id)
                    return true;
        }

        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}