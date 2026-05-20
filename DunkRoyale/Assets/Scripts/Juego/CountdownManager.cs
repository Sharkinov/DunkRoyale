using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownManager : MonoBehaviour
{
    public GameObject countdownPanel;
    public Image panelBackground;
    public GameObject uno;
    public GameObject dos;
    public GameObject tres;
    public GameObject areyouready;
    public GameTimer gameTimer;

    private Color morado = new Color32(85, 37, 131, 255);
    private Color amarillo = new Color32(253, 185, 39, 255);

    void Start()
    {
        StartCoroutine(RunCountdown());
    }

    IEnumerator RunCountdown()
    {
        countdownPanel.SetActive(true);

        yield return null; // esperar un frame

        uno.SetActive(false);
        dos.SetActive(false);
        tres.SetActive(false);
        areyouready.SetActive(false);

        panelBackground.color = amarillo;
        yield return StartCoroutine(AnimateIn(uno));
        yield return new WaitForSeconds(1f);
        uno.SetActive(false);

        panelBackground.color = morado;
        yield return StartCoroutine(AnimateIn(dos));
        yield return new WaitForSeconds(0.3f);
        dos.SetActive(false);

        panelBackground.color = amarillo;
        yield return StartCoroutine(AnimateIn(tres));
        yield return new WaitForSeconds(0.3f);
        tres.SetActive(false);

        panelBackground.color = morado;
        yield return StartCoroutine(AnimateIn(areyouready));
        yield return new WaitForSeconds(1.5f);

        countdownPanel.SetActive(false);
        gameTimer.StartGame();
    }

    IEnumerator AnimateIn(GameObject obj)
    {
        obj.SetActive(true);
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 targetScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scale = Mathf.Sin(t * Mathf.PI * 0.5f) * 1.2f;
            if (t > 0.7f)
                scale = Mathf.Lerp(1.2f, 1f, (t - 0.7f) / 0.3f);
            obj.transform.localScale = targetScale * scale;
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }
}