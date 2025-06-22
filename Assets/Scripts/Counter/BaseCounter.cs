using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectOwner
{
    [SerializeField] private Transform kitchenObjectPosition; // 放置橱柜物品的位置
    private Transform kitchenObject; // 橱柜上放置的物品

    public static event EventHandler OnAnyObjectDropped;
    public static event EventHandler OnAnyObjectPicked;

    // 实现橱柜的交互功能，player为进行交互的对象
    public virtual void Process(Player player) // 加工
    {
        Debug.Log("Process方法未实现");
    }
    public abstract void PickAndPlace(Player player); // 拾取或放置
    // 尝试将Counter上的KitchenObject放置到玩家手中的Plate中
    public bool TryPutObjectOnPlate(Player player)
    {
        if (kitchenObject == null)
        {
            return false;
        }
        // 将Counter上的KitchenObject放置到玩家手中的Plate中
        if (player.HasPlate())
        {
            PlateKitchenObject plateOnPlayer = player.GetGrabbedObject().GetComponent<PlateKitchenObject>();
            // 手里有盘子并且桌面上有物品
            if (kitchenObject != null)
            {
                if (plateOnPlayer.TryAddFood(kitchenObject))
                {
                    SetKitchenObject(null);
                    Debug.Log("放置物品到盘子上");

                    return true;
                }

            }
            // 手里有盘子但桌面上没有物品
            return false; // 执行常规的放置操作
        }
        // 将玩家手中的KitchenObject放置到Counter上的Plate中
        if (player.HasGrabbedObject()
            && kitchenObject.TryGetComponent<PlateKitchenObject>(out PlateKitchenObject plateOnCounter))
        {
            Transform kitchenObject = player.GetGrabbedObject();
            if (plateOnCounter.TryAddFood(kitchenObject))
            {
                player.ClearGrabbedObject();
                Debug.Log("玩家手中的物品放置到Counter上的盘子上");
                return true;
            }

        }
        return false;
    }

    public Transform GetKitchenObject()
    {
        return kitchenObject;
    }
    protected void SetUpKitchenObject(Transform kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject == null)
        {
            return;
        }
        kitchenObject.GetComponent<KitchenObject>().SetIOwner(this);
        kitchenObject.GetComponent<KitchenObject>().SetTransform();
    }

    protected virtual void SetKitchenObject(Transform kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public void OnObjectDroppedHandler()
    {
        OnAnyObjectDropped?.Invoke(this, EventArgs.Empty);
    }
    public void OnObjectPickedHandler()
    {
        OnAnyObjectDropped?.Invoke(this, EventArgs.Empty);
    }


    public Vector3 GetHoldPosition()
    {
        return kitchenObjectPosition.position;
    }

    public Transform GetTransform()
    {
        return transform;
    }

}
