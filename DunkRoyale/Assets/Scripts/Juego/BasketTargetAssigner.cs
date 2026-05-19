using UnityEngine;

public class BasketTargetAssigner : MonoBehaviour
{
    public Transform npcBasketTarget;
    public Transform playerBasketTarget;

    void Update()
    {
        var allCombat = FindObjectsOfType<PlayerCombat>();
        foreach (var combat in allCombat)
        {
            if (combat.basketTarget != null) continue;
            var move = combat.GetComponent<CharacterMove>();
            if (move == null) continue;
            combat.basketTarget = move.isEnemy ? npcBasketTarget : playerBasketTarget;
        }
    }
}