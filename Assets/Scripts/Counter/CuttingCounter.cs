using UnityEngine;
using System;
public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;
    private int cutCount;

    public static event EventHandler OnAnyCut; // 用于音频系统
    public event EventHandler<OnCuttingStartedEventArgs> OnCuttingStarted;
    public event EventHandler OnCuttingEnded;
    public class OnCuttingStartedEventArgs : EventArgs
    {
        public float progressNormalized; // 切割进度，0~1
    }
    public override void Process(Player player)
    {
        Transform kitchenObject = GetKitchenObject();
        if (kitchenObject != null && !player.HasGrabbedObject())
        {
            KitchenObjectSO kitchenObjectSO = kitchenObject.GetComponent<KitchenObject>().GetKitchenObjectSO();
            if (kitchenObjectSO.canCut)
            {
                Debug.Log("切菜");
                cutCount++;

                CuttingRecipeSO recipe = FindRecipe(kitchenObjectSO);

                OnCuttingStarted?.Invoke(this, new OnCuttingStartedEventArgs
                {
                    progressNormalized = (float)cutCount / recipe.cutsNeeded
                });
                OnAnyCut?.Invoke(this, EventArgs.Empty); // 播放切割音效

                // 达到切割次数后，生成切割后的物体
                if (cutCount >= recipe.cutsNeeded)
                {
                    cutCount = 0;
                    SpawnCuttedObject(recipe.output.pfKitchenObject);

                    OnCuttingEnded?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public override void PickAndPlace(Player player)
    {
        // 将菜板上切好的物体直接放入盘子中
        if (TryPutObjectOnPlate(player))
        {
            OnObjectPickedHandler();
            return;
        }
        // 如果还未切割完成就拿起，重置进度
        if (cutCount != 0)
        {
            cutCount = 0;
            OnCuttingEnded?.Invoke(this, EventArgs.Empty);
        }
        // 放置与拿起
        Transform kitchenObject = GetKitchenObject();
        if (kitchenObject != null && !player.HasGrabbedObject())
        {
            Debug.Log("拿起物品");
            player.GrabObject(kitchenObject);
            SetUpKitchenObject(null);

            OnObjectPickedHandler();
        }
        else if (kitchenObject == null && player.HasGrabbedObject() && CanPlace(player.GetGrabbedObject()))
        {
            Debug.Log("放置物品");
            SetUpKitchenObject(player.GetGrabbedObject());
            player.ClearGrabbedObject();

            OnObjectDroppedHandler();
        }
        else if (kitchenObject != null && player.HasGrabbedObject())
        {
            Debug.Log("桌子上已经有东西了");
        }
    }

    //生成切割后的物品
    private void SpawnCuttedObject(Transform spawnedObject)
    {
        // 销毁切之前的物体
        Destroy(GetKitchenObject().gameObject);
        // 生成切割后的物体
        Transform kitchenObject = Instantiate(spawnedObject);
        SetUpKitchenObject(kitchenObject);
    }

    private CuttingRecipeSO FindRecipe(KitchenObjectSO kitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipes)
        {
            if (recipe.input == kitchenObjectSO)
            {
                return recipe;
            }
        }
        Debug.Log("没有对应的切菜配方");
        return null;
    }

    private bool CanPlace(Transform kitchenObject)
    {
        KitchenObjectSO kitchenObjectSO = kitchenObject.GetComponent<KitchenObject>().GetKitchenObjectSO();

        foreach (CuttingRecipeSO recipe in cuttingRecipes)
        {
            if (recipe.input == kitchenObjectSO)
            {
                return true;
            }
        }
        Debug.Log("该KitchenObject不能被放置在这个柜台上");
        return false;
    }



}
