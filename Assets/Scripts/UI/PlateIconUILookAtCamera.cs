using System;
using UnityEngine;

public class PlateIconUILookAtCamera : LookAtCamera
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    private bool needToUpdate = false;

    private void Start()
    {
        plateKitchenObject.OnIOwnerChanged += PlateKitchenObject_OnIOwnerChanged;
    }

    private void Update()
    {
        if (needToUpdate)
        {
            UpdateLookAt();
        }
    }

    private void PlateKitchenObject_OnIOwnerChanged(object sender, Transform e)
    {
        if (e.tag == "Player")
        {
            needToUpdate = true;
        }
        else
        {
            needToUpdate = false;
        }
    }
}
