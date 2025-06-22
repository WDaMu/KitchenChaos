using System;
using UnityEngine;

public class CuttingCounterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start()
    {
        cuttingCounter.OnCuttingStarted += CuttingCounter_OnCuttingStarted;
    }

    private void CuttingCounter_OnCuttingStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("Cut");
    }
}
