using UnityEngine;

public interface IBox
{
    public void BoxOpen();
    public void OpenEffectEvent();
    public void BoxDestroy();
    public void InteractiveParticle(GameObject particle);
}
