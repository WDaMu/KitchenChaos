// Func:
// 1. player进行交互时为player提供一个指定的kitchenObject
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO containedKitchenObjectSO;

    public event EventHandler OnInteractWithContainer;

    public override void PickAndPlace(Player player)
    {
        if (!player.HasGrabbedObject())
        {
            OnInteractWithContainer?.Invoke(this, EventArgs.Empty);

            Transform kitchenObject = Instantiate(containedKitchenObjectSO.pfKitchenObject);
            player.GrabObject(kitchenObject);

            OnObjectPickedHandler();
        }
        else
        {
            Debug.Log("玩家手里已经有物品了");
        }
    }
}
