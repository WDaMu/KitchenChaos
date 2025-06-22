using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform pfIcon;

    private void Update()
    {
        UpdateIconUI();
    }
    private void UpdateIconUI()
    {
        foreach (Transform child in iconContainer)
        {
            Destroy(child.gameObject);
        }
        List<KitchenObjectSO> foodsInPlateSO = plateKitchenObject.GetFoodsInPlateSO();
        foreach (KitchenObjectSO foodSO in foodsInPlateSO)
        {
            Transform icon = Instantiate(pfIcon, iconContainer);
            icon.GetChild(0).GetComponent<Image>().sprite = foodSO.sprite;
        }
    }

}
