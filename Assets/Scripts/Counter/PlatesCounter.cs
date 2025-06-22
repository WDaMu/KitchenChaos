// Func:
// 生成盘子
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private Transform pfPlate;
    private List<Transform> plates;
    private float timer;
    private const int MAX_PLATE_COUNT = 5;
    private float SPAWN_PALTE_INTERVAL = 1.5f;
    private float PLATE_DISTANCE = 0.07f;
    private int plateCount;
    private void Awake()
    {
        plates = new List<Transform>();
    }
    private void Update()
    {
        if (plateCount > MAX_PLATE_COUNT)
        {
            return;
        }
        if (timer < SPAWN_PALTE_INTERVAL)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            SpawnPlate();
        }
    }
    public override void PickAndPlace(Player player)
    {
        // 玩家拿起盘子
        if (!player.HasGrabbedObject() && plates.Count > 0)
        {
            Transform topPlate = GetTopPlate();
            player.GrabObject(topPlate);

            OnObjectPickedHandler();
        }
    }

    private void SpawnPlate()
    {
        Vector3 position = GetHoldPosition();
        position.y += plateCount * PLATE_DISTANCE;
        plateCount++;

        Transform plate = Instantiate(pfPlate, position, Quaternion.identity, transform);
        plates.Add(plate);

        SetKitchenObject(plate);
        KitchenObject kitchenObject = plate.GetComponent<KitchenObject>();
        kitchenObject.SetIOwner(this);
    }


    private Transform GetTopPlate()
    {
        int count = plates.Count;
        Transform topPlate = plates[count - 1];
        plates.RemoveAt(count - 1);
        plateCount--;
        return topPlate;
    }
}
