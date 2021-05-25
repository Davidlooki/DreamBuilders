using UnityEngine;

/// <summary>
/// Implement this interface on any MonoBehaviour that you'd like to be considered selectable by SelectionBox.
/// </summary>
/// <returns></returns>
public interface IBoxSelectable
{
    bool Selected { get; set; }

    bool PreSelected { get; set; }

    //This property doesn't actually need to be implemented, as this interface should already be placed on a MonoBehaviour, which will
    //already have it. Defining it here only allows us access to the transform property by casting through the selectable interface.
    Transform Transform { get; }
}