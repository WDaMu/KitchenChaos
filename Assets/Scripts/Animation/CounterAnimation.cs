using System;
using System.ComponentModel;
using UnityEngine;

public class CounterAnimation : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        containerCounter.OnInteractWithContainer += ContainerCounter_OnInteractWithContainer;
    }

    private void ContainerCounter_OnInteractWithContainer(object sender, EventArgs e)
    {
        OpenContainer();
    }

    public void OpenContainer()
    {
        animator.SetTrigger("OpenClose");
    }
}
