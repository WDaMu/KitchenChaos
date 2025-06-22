using UnityEngine;
using System;

public class ClearCounter : BaseCounter
{
    public override void PickAndPlace(Player player)
    {
        if (TryPutObjectOnPlate(player))
        {
            return;
        }

        Transform kitchenObject = GetKitchenObject();
        // 拾取桌子上的物品
        if (kitchenObject != null && !player.HasGrabbedObject())
        {
            player.GrabObject(kitchenObject);
            SetUpKitchenObject(null);

            OnObjectDroppedHandler();
        }
        // 放置物品到桌子上
        else if (kitchenObject == null && player.HasGrabbedObject())
        {
            SetUpKitchenObject(player.GetGrabbedObject());
            player.ClearGrabbedObject();

            OnObjectDroppedHandler();
        }
        // 桌子上已经有东西了
        else if (kitchenObject != null && player.HasGrabbedObject())
        {
            Debug.Log("桌子上已经有东西了，不能放置玩家手中的物品");
        }
    }
}
