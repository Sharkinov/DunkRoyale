using UnityEngine;

public class BasketTrigger : MonoBehaviour
{
    public bool isPlayerBasket; // true = basket del jugador (NPC anota aqui)

    void OnTriggerEnter2D(Collider2D other) => CheckScore(other);
    void OnTriggerStay2D(Collider2D other) => CheckScore(other);

    void CheckScore(Collider2D other)
    {
        Debug.Log($"[BasketTrigger] CheckScore: {other.name}, isPlayerBasket={isPlayerBasket}");
        var combat = other.GetComponent<PlayerCombat>();
        if (combat == null) return;
        if (combat.isExhausted) return; // evitar doble score

        var move = other.GetComponent<CharacterMove>();
        if (move == null) return;

        bool isNPCCharacter = move.isEnemy;
        bool scoredByNPC = isNPCCharacter && isPlayerBasket;
        bool scoredByPlayer = !isNPCCharacter && !isPlayerBasket;
        Debug.Log($"[BasketTrigger] isNPC={isNPCCharacter}, scoredByNPC={scoredByNPC}, scoredByPlayer={scoredByPlayer}");

        if (scoredByNPC || scoredByPlayer)
        {
            ScoreManager.Instance.AddScore(isNPCCharacter);
            combat.BenchPlayer();
        }
    }
}