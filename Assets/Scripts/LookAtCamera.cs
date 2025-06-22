using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Start()
    {
        UpdateLookAt();
    }

    public void UpdateLookAt()
    {
        // 现在访问Camera.main不会再便利场景中的所有GameObject，不会产生性能问题 
        // x值不变
        Camera mainCamera = Camera.main;
        Vector3 lookAtForward = mainCamera.transform.forward;
        if (mainCamera != null)
        {
            transform.forward = lookAtForward;
        }
    }
}
