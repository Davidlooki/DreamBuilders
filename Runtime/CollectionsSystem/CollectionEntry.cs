using System;
using List = System.Collections.Generic.List<string>;
using UnityEngine;

public abstract class CollectionEntry : ScriptableObject, ICollectionEntry
{
    #region Fields

    [field: SerializeField] public Sprite Icon { get; set; } = null;
    [field: SerializeField] public int Id { get; set; } = 0;
    [field: SerializeField] public string Name { get; set; } = string.Empty;

    [field: SerializeField, TextArea(10, int.MaxValue)]
    public string Description { get; set; } = string.Empty;

    [field: SerializeField] public List Tags { get; set; } = new List();

    #endregion

    #region Unity Methods

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(Name))
            Name = name;
    }
#endif

    #endregion

    #region Custom Methods

    #endregion
}