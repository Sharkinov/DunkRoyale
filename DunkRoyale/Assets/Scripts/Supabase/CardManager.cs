using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CardManager : MonoBehaviour
{
    [Header("References")]
    private GameDataService gameDataService;

    [Header("Las 4 cartas principales")]
    public Button[] cardButtons;
    public Text[] cardCosts;

    [Header("Placeholder")]
    public Sprite defaultSprite;

    [Header("Warning en pantalla")]
    public GameObject warningPanel;
    public float   warningDuration = 2f;

    [HideInInspector]
    public UserDeckItem[] loadedCards;

    [HideInInspector]
    public int selectedCardIndex = -1;

    private Image[] cardImages;
    public int[] visibleSlots;
    private Queue<int> drawQueue = new Queue<int>();
    private float        warningTimer = 0f;

    void Start()
    {
        cardImages = new Image[cardButtons.Length];
        visibleSlots = new int[cardButtons.Length];

        for (int i = 0; i < cardButtons.Length; i++)
        {
            cardImages[i] = cardButtons[i].GetComponent<Image>();
        }

        if (warningPanel != null)
            warningPanel.gameObject.SetActive(false);

        gameDataService = GetComponent<GameDataService>();

        StartCoroutine(gameDataService.GetPlayerStats(
            onSuccess: (cards) => {
                loadedCards = cards;
                Debug.Log($"Loaded {cards.Length} player cards!");
                InitializeDeck();
            },
            onError: () => {
                Debug.LogError("Failed to load player cards");
            }
        ));
    }

    void Update()
    {
        if (warningTimer > 0f)
        {
            warningTimer -= Time.deltaTime;
            if (warningTimer <= 0f && warningPanel != null)
                warningPanel.gameObject.SetActive(false);
        }
    }

    void InitializeDeck()
    {
        var indices = new List<int>();
        for (int i=0; i<loadedCards.Length; i++) indices.Add(i);
        Shuffle(indices);
        drawQueue.Clear();
        foreach (var idx in indices) drawQueue.Enqueue(idx);
        for (int i = 0; i < cardButtons.Length; i++)
            cardButtons[i].onClick.RemoveAllListeners();
        for (int slot = 0; slot < cardButtons.Length; slot++)
        {
            visibleSlots[slot] = -1;
            DrawIntoSlot(slot);
        }
    }

    void OnCardClicked(int slotIndex)
    {
        if (visibleSlots[slotIndex] == -1) return;
        // Si clickeas la misma carta, deselecciona
        if (selectedCardIndex == slotIndex)
        {
            selectedCardIndex = -1;
            GridManager.Instance.HideZones();
            return;
        }

        selectedCardIndex = slotIndex;
        GridManager.Instance.ShowZones();
    }

    public bool ConsumeSelectedCard()
    {
        if (selectedCardIndex == -1) return false;
        int usedCardIndex = visibleSlots[selectedCardIndex];
        drawQueue.Enqueue(usedCardIndex);
        int consumedSlot = selectedCardIndex;
        visibleSlots[consumedSlot] = -1;
        cardImages[consumedSlot].sprite = defaultSprite;
        if (cardCosts[consumedSlot] != null) cardCosts[consumedSlot].text = "";
        selectedCardIndex = -1;
        DrawIntoSlot(consumedSlot);
        return true;
    }

    public void ShowWarning()
    {
        if (warningPanel == null) return;
        warningPanel.gameObject.SetActive(true);
        warningTimer = warningDuration;
    }

    void DrawIntoSlot(int slot)
    {
        if (drawQueue.Count == 0) return;
        int cardIndex = drawQueue.Dequeue();
        visibleSlots[slot] = cardIndex;
        cardImages[slot].sprite = defaultSprite;
        var card = loadedCards[cardIndex].card;
        if (cardCosts[slot] != null)
            cardCosts[slot].text = card.gatorade_cost.ToString();
        int capturedSlot = slot;
        cardButtons[slot].onClick.RemoveAllListeners();
        cardButtons[slot].onClick.AddListener(() => OnCardClicked(capturedSlot));
        StartCoroutine(LoadImageFromUrl(card.web_url, cardImages[slot]));
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
    
    void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}