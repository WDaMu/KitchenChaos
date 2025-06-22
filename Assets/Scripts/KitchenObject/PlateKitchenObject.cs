using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> canPickedFoods;
    [SerializeField] private Transform hanburger;
    private List<KitchenObjectSO> foodsInPlateSO;
    private bool canAddFood = true;

    public event EventHandler<Transform> OnIOwnerChanged;

    private void Awake()
    {
        foodsInPlateSO = new List<KitchenObjectSO>();
    }


    public bool TryAddFood(Transform food)
    {
        if (!canAddFood)
        {
            Debug.Log("盘子已满");
            return false;
        }
        if (food.TryGetComponent<KitchenObject>(out KitchenObject foodObject))
        {
            KitchenObjectSO foodSO = foodObject.GetKitchenObjectSO();
            if (!canPickedFoods.Contains(foodSO))
            {
                Debug.Log("不能将该KitchenObject放入盘子");
                return false;
            }
            if (foodsInPlateSO.Contains(foodSO))
            {
                Debug.Log("不能重复放入");
                return false;
            }
            else
            {
                Transform foodTransform = GetChildWithName(hanburger, foodSO.kitchenObjectName);
                foodTransform.gameObject.SetActive(true);

                foodsInPlateSO.Add(foodSO);

                Destroy(food.gameObject);

                // 每次放入food后检测材料是否已齐全，如齐全，生成汉堡并清空盘子
                if (foodsInPlateSO.Count == 5)
                {
                    canAddFood = false;
                }
                return true;
            }
        }
        else
        {
            Debug.LogError("没有获取到KitchenObject组件");
            return false;
        }
    }

    public List<KitchenObjectSO> GetFoodsInPlateSO()
    {
        return foodsInPlateSO;
    }

    private Transform GetChildWithName(Transform parent, string name)
    {   
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }

    void OnTransformParentChanged()
    {
        OnIOwnerChanged?.Invoke(this, transform.parent);
    }
}
