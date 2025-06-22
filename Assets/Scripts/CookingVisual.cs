using System;
using UnityEngine;

public class CookingVisual : MonoBehaviour
{
    [SerializeField] private GameObject cookingVisual;
    [SerializeField] private GameObject cookingParticleSystem;
    private StoveCounter stoveCounter;

    private ParticleSystem particleSystemComponent;
    public void Awake()
    {
        stoveCounter = GetComponent<StoveCounter>();
        particleSystemComponent = cookingParticleSystem.GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        stoveCounter.onCookingStarted += stoveCounter_onCookingStarted;   
        stoveCounter.onCookingEnded += stoveCounter_onCookingEnded;   
    }

    private void stoveCounter_onCookingStarted(object sender, EventArgs e)
    {
        Show();
    }
    private void stoveCounter_onCookingEnded(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        cookingVisual.SetActive(true);

        cookingParticleSystem.SetActive(true);
        particleSystemComponent.Play();
    }

    public void Hide()
    {
        cookingVisual.SetActive(false);

        particleSystemComponent.Stop();
        cookingParticleSystem.SetActive(false);
    }
}
