using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectOwner IOwner;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    public string GetKitchenObjectName()
    {
        return kitchenObjectSO.kitchenObjectName;
    }

    public void SetTransform(Vector3 offest = default)
    {
        transform.parent = IOwner.GetTransform();
        transform.position = IOwner.GetHoldPosition() + offest;
    }
    public void SetIOwner(IKitchenObjectOwner IOwner)
    {
        this.IOwner = IOwner;
    }
    public IKitchenObjectOwner GetOwner()
    {
        return IOwner;
    }
}
