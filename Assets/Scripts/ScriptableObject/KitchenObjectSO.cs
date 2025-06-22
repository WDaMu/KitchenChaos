using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/KitchenObject", fileName = "KitchenObject")]
public class KitchenObjectSO : ScriptableObject
{
    public string kitchenObjectName;
    public bool canCut;
    public Transform pfKitchenObject;
    public Sprite sprite;
}
