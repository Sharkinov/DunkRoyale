using UnityEngine;

public class BasketTrigger : MonoBehaviour
{
    public bool isPlayerBasket; // true = basket del jugador (NPC anota aqui)

    void OnTriggerEnter2D(Collider2D other)
    {
        var combat = other.GetComponent<PlayerCombat>();
        if (combat == null) return;

        bool isNPCCharacter = other.GetComponent<CharacterMove>().isEnemy;

        bool scoredByNPC = isNPCCharacter && isPlayerBasket;
        bool scoredByPlayer = !isNPCCharacter && !isPlayerBasket;

        if (scoredByNPC || scoredByPlayer)
        {
            ScoreManager.Instance.AddScore(isNPCCharacter);
            combat.BenchPlayer();
        }
    }
}