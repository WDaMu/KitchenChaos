using System;
using System.Collections;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private Transform fridMeat;
    [SerializeField] private Transform burnedMeat;
    private const string MEAT = "Meat";

    private float timer;
    private float FRYING_TIME = 3f;
    private float MAX_FRYING_TIME = 7f;

    public event EventHandler onCookingStarted;
    public event EventHandler onCookingEnded;
    public event EventHandler onMeatFried; // Meat熟了


    private enum FryingState
    {
        Empty,
        Frying,
        Fried,
        Burned
    }
    private FryingState currentState = FryingState.Empty;

    private IEnumerator fryingCoroutine()
    {
        NextState(); // 协程开始时进入烹饪状态
        timer = 0f;
        // 烹饪
        while (timer < FRYING_TIME)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        NextState();
        while (timer < MAX_FRYING_TIME)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        NextState();
    }
    private void NextState()
    {
        switch (currentState)
        {
            // 进入烹饪状态
            case FryingState.Empty:
                currentState = FryingState.Frying;
                break;
            // 进入熟肉状态
            case FryingState.Frying:
                currentState = FryingState.Fried;

                // 生成熟肉
                Destroy(GetKitchenObject().gameObject); // 删除生肉
                Transform friedMeatTransform = Instantiate(fridMeat);
                onMeatFried?.Invoke(this, EventArgs.Empty);
                SetUpKitchenObject(friedMeatTransform);
                break;
            // 进入烧焦状态
            case FryingState.Fried:
                currentState = FryingState.Burned;
                // 烧焦
                Destroy(GetKitchenObject().gameObject); // 删除熟肉
                Transform burnedMeatTransform = Instantiate(burnedMeat);
                SetUpKitchenObject(burnedMeatTransform);
                break;
            // 进入空状态
            case FryingState.Burned:
                currentState = FryingState.Empty;
                EndFrying(); // 停止烹饪
                break;
        }
        Debug.Log("当前状态: " + currentState);
    }

    private void StartFrying()
    {
        StartCoroutine("fryingCoroutine");
        onCookingStarted?.Invoke(this, EventArgs.Empty);

    }
    private void EndFrying()
    {
        StopCoroutine("fryingCoroutine"); // 停止协程
        onCookingEnded?.Invoke(this, EventArgs.Empty);
        currentState = FryingState.Empty;
        Debug.Log("烹饪结束或被打断");

    }

    public override void PickAndPlace(Player player)
    {
        // 尝试将烹饪好的Meat放置到玩家手中的盘子上
        if (TryPutObjectOnPlate(player))
        {
            OnObjectDroppedHandler();
            EndFrying();
            return;
        }

        Transform kitchenObject = GetKitchenObject();
        // 将玩家手中的肉放置到炉子上
        if (kitchenObject == null && player.HasGrabbedObject())
        {
            if (player.GetGrabbedObject().GetComponent<KitchenObject>().GetKitchenObjectName() == MEAT)
            {
                SetUpKitchenObject(player.GetGrabbedObject());
                player.ClearGrabbedObject();

                StartFrying(); // 开始烹饪

                OnObjectDroppedHandler();
            }
            else
            {
                Debug.Log("玩家手上的不是Meat");
                return;
            }
        }

        // 从炉子上拿起肉
        else if (kitchenObject != null && !player.HasGrabbedObject())
        {
            if (currentState != FryingState.Empty || currentState != FryingState.Burned)
            {
                EndFrying();
            }

            player.GrabObject(kitchenObject);

            SetUpKitchenObject(null);

            OnObjectPickedHandler();
        }


        else if (kitchenObject != null && player.HasGrabbedObject())
        {
            Debug.Log("炉子上已经有东西了");
        }
    }

    public float GetNormalizeTimer()
    {
        if (currentState == FryingState.Frying)
        {
            return timer / FRYING_TIME;
        }
        else if (currentState == FryingState.Fried)
        {
            return (timer - FRYING_TIME) / (MAX_FRYING_TIME - FRYING_TIME);
        }
        else
        {
            Debug.LogError("不应在其他状态下调用GetNormalizeTimer()方法");
            return 0f;
        }
    }

}
