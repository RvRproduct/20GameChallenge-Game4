using UnityEngine;

public abstract class Factory : ObjectPool
{

    protected override void Awake()
    {
        base.Awake();
    }
    public abstract IProduct GetProduct(string poolTag, Vector3 position, Quaternion rotation);
}
