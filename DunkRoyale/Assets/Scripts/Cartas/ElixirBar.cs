using UnityEngine;
using UnityEngine.UI;

public class ElixirBar : MonoBehaviour
{
    [Header("Imagen barra")]
    public Image elixirImage;

    [Header("Config")]
    public float maxElixir = 10f;
    public float recargaAmount = 0.1f;
    private float currentElixir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Text")]
    public Text elixirText;
    void Start()
    {
     currentElixir = maxElixir;
     UpdateBar();   
    }

    // Update is called once per frame
    private bool recharging = true;

    void Update()
    {
        if (!recharging) return;
        if(currentElixir < maxElixir)
        {
            currentElixir += recargaAmount * Time.deltaTime;
            currentElixir = Mathf.Clamp(currentElixir, 0f, maxElixir);
            UpdateBar();
        }
    }

    public void StopRecharge()
    {
        recharging = false;
    }
    void UpdateBar()
    {
        elixirImage.fillAmount = currentElixir / maxElixir;
        if (elixirText != null)
        {
            elixirText.text = $"{Mathf.FloorToInt(currentElixir)}/{Mathf.FloorToInt(maxElixir)}";
        }
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
