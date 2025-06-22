// 统一接口实现KitchenObject的Parent
using UnityEngine;

public interface IKitchenObjectOwner
{
    public Transform GetTransform();

    public Vector3 GetHoldPosition();
}
