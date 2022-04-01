using UnityEngine;

[CreateAssetMenu(fileName = "NewVoidEvent", menuName = "DreamBuilders/Game Events/void Event")]
public class VoidEvent : BaseGameEvent<Void>
{
    public void Raise() => Raise(new Void());
}