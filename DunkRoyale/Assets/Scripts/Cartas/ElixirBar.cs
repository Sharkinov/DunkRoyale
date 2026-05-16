using UnityEngine;
using UnityEngine.UI;

public class ElixirBar : MonoBehaviour
{
    [Header("Imagen barra")]
    public Image elixirImage;

    [Header("Config")]
    public float maxElixir = 10f;
    public float recargaAmount = 1f;
    private float currentElixir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     currentElixir = maxElixir;
     UpdateBar();   
    }

    // Update is called once per frame
    void Update()
    {
        if(currentElixir < maxElixir)
        {
            currentElixir += recargaAmount * Time.deltaTime;
            currentElixir = Mathf.Clamp(currentElixir, 0f, maxElixir);
            UpdateBar();
        }
    }
    void UpdateBar()
    {
        elixirImage.fillAmount = currentElixir / maxElixir;
    }
    //Esto es para que se gaste jeje
    public bool TrySpend(float cost)
    {
        if (currentElixir < cost) return false;
        currentElixir -= cost;
        UpdateBar();
        return true;
    }
}
