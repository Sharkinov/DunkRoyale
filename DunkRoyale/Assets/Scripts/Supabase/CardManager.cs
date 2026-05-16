using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CardManager : MonoBehaviour
{
    [Header("References")]
    private GameDataService gameDataService;

    [Header("Card UI Slots")]
    public Image[] cardImages;
    public Text[] cardCosts;

    [Header("Placeholder")]
    public Sprite defaultSprite;

    [HideInInspector]
    public UserDeckItem[] loadedCards;

    void Start()
    {
        gameDataService = GetComponent<GameDataService>();
        StartCoroutine(gameDataService.GetPlayerStats(
            onSuccess: (cards) => {
                loadedCards = cards;
                Debug.Log($"Loaded {cards.Length} player cards!");
                DisplayCards();
            },
            onError: () => {
                Debug.LogError("Failed to load player cards");
            }
        ));
    }

    void DisplayCards()
    {
        int cardsToShow = Mathf.Min(cardImages.Length, loadedCards.Length);

        for (int i = 0; i < cardImages.Length; i++)
        {
            if (i < cardsToShow)
            {
                // show placeholder while loading
                cardImages[i].sprite = defaultSprite;

                // load real card image from web_url
                StartCoroutine(LoadImageFromUrl(
                    loadedCards[i].card.web_url,
                    cardImages[i]
                ));

                if (cardCosts[i] != null)
                    cardCosts[i].text = loadedCards[i].card.gatorade_cost.ToString();
            }
        }
    }

    IEnumerator LoadImageFromUrl(string url, Image targetImage)
    {
        if (string.IsNullOrEmpty(url))
        {
            targetImage.sprite = defaultSprite;
            yield break;
        }

        using (var request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                targetImage.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
            else
            {
                Debug.LogWarning($"Failed to load image: {url}");
                targetImage.sprite = defaultSprite;
            }
        }
    }
}