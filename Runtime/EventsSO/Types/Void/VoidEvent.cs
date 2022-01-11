using UnityEngine;

[CreateAssetMenu(fileName = "New Void Event", menuName = "DreamBuilders/Game Events/Void Event")]
public class VoidEvent : BaseGameEvent<Void>
{
    public void Raise() => Raise(new Void());
}