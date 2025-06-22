using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class Player : MonoBehaviour, IKitchenObjectOwner
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private Transform grabPoint;
    private Transform grabbedObject;

    public event EventHandler OnWalking;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    private bool isWalking = false;
    private BaseCounter selectedCounter;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Player instance already exists");
            Destroy(gameObject);
        }
        selectedCounter = null;
    }
    private void Start()
    {
        GameInput.Instance.onProcessPerformed += GameInput_onProcessPerformed;
        GameInput.Instance.onPickAndPlacePerformed += GameInput_onPickAndPlacePerformed;
    }

    private void GameInput_onPickAndPlacePerformed(object sender, EventArgs e)
    {
        selectedCounter?.PickAndPlace(this);
    }

    private void GameInput_onProcessPerformed(object sender, EventArgs e)
    {
        selectedCounter?.Process(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleCounterSelect();
    }
    private void HandleCounterSelect()
    {
        float interactionDistance = 1f;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, interactionDistance))
        {
            if (hitInfo.transform.TryGetComponent(out BaseCounter counter))
            {
                if (counter != selectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

    }
    private void HandleMovement()
    {
        Vector3 moveDir = GameInput.Instance.GetMovementInputNormalized();

        // 无论是否移动，都更新朝向
        Vector3 lookDir = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        if (lookDir != Vector3.zero)
        {
            transform.forward = lookDir;
        }

        isWalking = moveDir != Vector3.zero;

        if (IsWalking())
        {
            OnWalking?.Invoke(this, EventArgs.Empty);
        }

        // 移动前检测Player是否有Collider
        float playerHeight = 1.8f;
        float playerRadius = 0.7f;
        float moveDistance = moveSpeed * Time.deltaTime;
        Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
        bool canMoveX = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
        bool canMoveZ = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

        if (!canMoveX && !canMoveZ)
        {
            return;
        }
        if (canMoveX && canMoveZ)
        {
            // 什么都不做
        }
        else if (canMoveX)
        {
            moveDir.z = 0;
        }
        else
        {
            moveDir.x = 0;
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    private void SetSelectedCounter(BaseCounter counter)
    {
        selectedCounter = counter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = counter
        });
        // Debug.Log("Selected Counter Changed: " + selectedCounter);
    }

    public bool HasGrabbedObject()
    {
        return grabbedObject != null;
    }
    public Transform GetGrabbedObject()
    {
        return grabbedObject;
    }

    public void GrabObject(Transform obj)
    {
        grabbedObject = obj;

        // 配置抓取的物体：
        // 1.KitchenObject的IOwner设置为Player
        // 2.设置KitchenObject的Parent及Position
        KitchenObject kitchenObject = grabbedObject.GetComponent<KitchenObject>();
        kitchenObject.SetIOwner(this);
        kitchenObject.SetTransform();
    }
    public void ClearGrabbedObject()
    {
        grabbedObject = null;
    }

    public bool HasPlate()
    {
        return grabbedObject != null && grabbedObject.TryGetComponent<PlateKitchenObject>(out _); // ?
    }


    public Transform GetTransform()
    {
        return transform;
    }

    public Vector3 GetHoldPosition()
    {
        return grabPoint.position;
    }

}
