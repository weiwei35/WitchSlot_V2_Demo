using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {
    public static DeckManager Instance { get; private set; }

    [Header("Deck Configuration")]
    public List<CardData> allCards = new List<CardData>();
    [SerializeField] private bool useCardRecycling = true;
    [SerializeField] private bool autoShuffleWhenRefillingDeck = true;

    private List<CardData> drawingPile = new List<CardData>();   // 当前可抽的牌
    private List<CardData> discardPile = new List<CardData>();   // 用过的牌回收池

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void InitializeDeck(bool shuffle = true) {
        drawingPile.Clear();
        discardPile.Clear();
        drawingPile.AddRange(allCards);

        if (shuffle) ShuffleDeck();
        Debug.Log($"📘 牌库已加载 {allCards.Count} 张牌");
    }

    public void ShuffleDeck() {
        for (int i = 0; i < drawingPile.Count; i++) {
            CardData temp = drawingPile[i];
            int randomIndex = Random.Range(i, drawingPile.Count);
            drawingPile[i] = drawingPile[randomIndex];
            drawingPile[randomIndex] = temp;
        }
        Debug.Log("🔀 牌堆已洗牌");
    }

    /// <summary>
    /// 持续抽取指定数量的牌（最多不会超过总牌数）
    /// </summary>
    public List<CardData> DrawCards(int amount) {
        List<CardData> drawnCards = new List<CardData>();

        while (amount > 0 && (HasCards() || HasDiscard())) {
            if (HasCards()) {
                int drawNow = Mathf.Min(amount, drawingPile.Count);
                drawnCards.AddRange(drawingPile.GetRange(0, drawNow));
                drawingPile.RemoveRange(0, drawNow);
                amount -= drawNow;
            }
            else if (HasDiscard()) {
                Debug.Log("🔄 牌库已空，从弃牌中回收");
                RefillDeckFromDiscard();
                if (autoShuffleWhenRefillingDeck) ShuffleDeck();
            }
        }

        // 将抽走的牌放入弃牌区（供后续回收）
        if (useCardRecycling) discardPile.AddRange(drawnCards);

        return drawnCards;
    }

    private void RefillDeckFromDiscard() {
        if (discardPile.Count > 0) {
            drawingPile.AddRange(discardPile);
            discardPile.Clear();
            Debug.Log($"🔁 回收 {drawingPile.Count} 张弃牌回牌库");
        }
    }

    [ContextMenu("强制清空抽牌堆并抽空")]
    public void ForceEmptyDeckForTest() {
        drawingPile.Clear();
        for (int i = 0; i < allCards.Count; i++) {
            discardPile.Add(allCards[i]);
        }
        Debug.Log("🧪 测试：牌库已强制抽空");
    }

    private bool HasCards() => drawingPile.Count > 0;
    private bool HasDiscard() => discardPile.Count > 0;
}
