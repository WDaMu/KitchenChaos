using System;
using System.ComponentModel;
using UnityEngine;

public class CounterSelectedVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;

    private GameObject counterSelectedVisual;
    private void Awake()
    {
        counterSelectedVisual = transform.GetChild(0).gameObject;
    }
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCountedChanged;
        
        if (counter is ContainerCounter)
        {
            ContainerCounter containerCounter = (ContainerCounter)counter;
            //containerCounter.OnInteractWithContainer += ContainerCounter_OnInteractWithContainer;
        }

    }

    private void Show()
    {
        counterSelectedVisual.SetActive(true);
    }
    private void Hide()
    {
        counterSelectedVisual.SetActive(false);
    }

    private void Player_OnSelectedCountedChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == counter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void ContainerCounter_OnInteractWithContainer(object sender, EventArgs e)
    {
        Hide();
    }
}
