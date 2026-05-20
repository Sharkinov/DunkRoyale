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

        yield return null;

        Vector3 scaleUno = uno.transform.localScale;
        Vector3 scaleDos = dos.transform.localScale;
        Vector3 scaleTres = tres.transform.localScale;
        Vector3 scaleReady = areyouready.transform.localScale;

        uno.SetActive(false);
        dos.SetActive(false);
        tres.SetActive(false);
        areyouready.SetActive(false);

        // Audio 3,2,1
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayOnetwo();

        panelBackground.color = amarillo;
        yield return StartCoroutine(AnimateIn(uno, scaleUno));
        yield return new WaitForSeconds(0.3f);
        uno.SetActive(false);

        panelBackground.color = morado;
        yield return StartCoroutine(AnimateIn(dos, scaleDos));
        yield return new WaitForSeconds(0.3f);
        dos.SetActive(false);

        panelBackground.color = amarillo;
        yield return StartCoroutine(AnimateIn(tres, scaleTres));
        yield return new WaitForSeconds(0.3f);
        tres.SetActive(false);

        // Audio are you ready
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayReadyForThis();

        panelBackground.color = morado;
        yield return StartCoroutine(AnimateIn(areyouready, scaleReady));
        yield return new WaitForSeconds(1.5f);

        countdownPanel.SetActive(false);
        gameTimer.StartGame();
    }
    IEnumerator AnimateIn(GameObject obj, Vector3 targetScale)
    {
        obj.SetActive(true);
        obj.transform.localScale = Vector3.zero;
        yield return null; // esperar un frame en zero

        float duration = 0.5f;
        float elapsed = 0f;

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