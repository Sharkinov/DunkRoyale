using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    public int attack;
    public int defense;
    public float velocity;
    public float fatigueTime = 15f;

    [Header("Detection")]
    public float detectionRange = 3f;
    public float fightRange = 0.4f; // raise from 0.5f to 1f// distancia para pelear

    [Header("UI Bars")]
    public SpriteRenderer attackBarImage;
    public SpriteRenderer defenseBarImage;
    private int maxAttack;
    private int maxDefense;

    [Header("Hit Effects")]
    public float flashDuration = 0.2f;
    public float bounceForce = 0.5f;

    private CharacterMove movement;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float fatigueTimer;
    private bool isExhausted = false;
    private bool inConfrontation = false;
    private PlayerCombat targetOpponent = null;

    void Start()
    {
        movement = GetComponent<CharacterMove>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fatigueTimer = fatigueTime;
    }

    public void Update()
    {
        if (isExhausted) return;

        // fatigue
        fatigueTimer -= Time.deltaTime;
        if (fatigueTimer <= 0f)
        {
            BenchPlayer();
            return;
        }

        // slow down over time
        float fatiguePct = fatigueTimer / fatigueTime;
        movement.speed = Mathf.Max(velocity * fatiguePct, 0.5f);

        // mirror sprite based on horizontal movement
        if (rb.linearVelocity.x > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 
                                                transform.localScale.y, 
                                                transform.localScale.z);
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 
                                                transform.localScale.y, 
                                                transform.localScale.z);
        }

        if (inConfrontation) return;

        // check for enemies
        PlayerCombat enemy = FindNearestEnemy();

        // In Update(), replace the enemy detection block:
        if (enemy != null)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist <= fightRange)
            {
                rb.linearVelocity = Vector2.zero;
                if (!inConfrontation && !enemy.inConfrontation)
                    StartConfrontation(enemy);
            }
            else
            {
                MoveToward(enemy.transform.position);
            }
        }
        else
        {
            movement.Resume(); // ← this was missing, NPCs just froze after a confrontation
        }
    }

    PlayerCombat FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            detectionRange
        );

        PlayerCombat nearest = null;
        float nearestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            var otherCombat = hit.GetComponent<PlayerCombat>();
            if (otherCombat == null) continue;
            if (otherCombat.isExhausted) continue;

            var otherMove = hit.GetComponent<CharacterMove>();
            if (otherMove == null) continue;

            bool otherIsEnemy = otherMove.isEnemy;
            bool iAmEnemy = movement.isEnemy;

            if (iAmEnemy != otherIsEnemy)
            {
                float dist = Vector2.Distance(
                    transform.position, 
                    hit.transform.position
                );
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = otherCombat;
                }
            }
        }

        return nearest;
    }

    void MoveToward(Vector3 targetPos)
    {
        Vector2 direction = (targetPos - transform.position).normalized;
        rb.linearVelocity = direction * movement.speed;
    }

    // Ya no lo ocupo
    // void SpawnStatBars()
    // {
    //     if (statBarPrefab == null) return;

    //     GameObject bars = Instantiate(statBarPrefab, transform);
    //     bars.transform.localPosition = new Vector3(0, 1.2f, 0);

    //     var images = bars.GetComponentsInChildren<Image>();
    //     if (images.Length >= 2)
    //     {
    //         attackBarImage = images[0];
    //         defenseBarImage = images[1];
    //         UpdateBars();
    //     }
    // }

    void UpdateBars()
    {
        if (attackBarImage != null)
        {
            float pct = maxDefense > 0 ? (float)attack / maxAttack : 0f;
            attackBarImage.size = new Vector2(pct, attackBarImage.size.y);
        } 
        if (defenseBarImage != null)
        {
            float pct = maxDefense > 0 ? (float)defense / maxDefense: 0f;
            defenseBarImage.size = new Vector2(pct, defenseBarImage.size.y);
        }
    }

    void StartConfrontation(PlayerCombat opponent)
    {
        if (inConfrontation || opponent.inConfrontation) return;

        inConfrontation = true;
        opponent.inConfrontation = true;
        targetOpponent = opponent;

        // Hard zero before anything else
        rb.linearVelocity = Vector2.zero;
        opponent.rb.linearVelocity = Vector2.zero;

        movement.Stop();
        opponent.movement.Stop();

        StartCoroutine(RunConfrontation(opponent));
    }

IEnumerator RunConfrontation(PlayerCombat opponent)
{
    while (true)
    {
        StartCoroutine(FlashRed());
        StartCoroutine(opponent.FlashRed());

        yield return new WaitForSeconds(0.5f);

        // Safety checks
        if (opponent == null || opponent.isExhausted)
        {
            inConfrontation = false;
            movement.Resume();
            yield break;
        }
        if (isExhausted)
        {
            opponent.inConfrontation = false;
            opponent.movement.Resume();
            yield break;
        }

        // Who attacks who based on field position
        float fightY = (transform.position.y + opponent.transform.position.y) / 2f;
        bool fightIsOnNPCSide = fightY > GridManager.Instance.GetMidlineY();

        PlayerCombat attacker, defender;
        PlayerCombat npc = movement.isEnemy ? this : opponent;
        PlayerCombat player = movement.isEnemy ? opponent : this;

        if (fightIsOnNPCSide)
        {
            attacker = player;
            defender = npc;
        }
        else
        {
            attacker = npc;
            defender = player;
        }

        // Attacker chips away at defender's defense
        int damage = Mathf.Max(attacker.attack - defender.defense, 1);
        defender.defense -= damage;
        defender.UpdateBars(); // show the bar going down

        // Also slightly drain attacker's attack each round
        attacker.attack = Mathf.Max(attacker.attack - 1, 0);
        attacker.UpdateBars();

        // Defender lost — bench them
        if (defender.defense <= 0)
        {
            defender.BenchPlayer();
            attacker.inConfrontation = false;
            attacker.movement.Resume();
            yield break;
        }

        // Both still standing — next round
    }
}

IEnumerator FlashRed()
{
    var allRenderers = GetComponentsInChildren<SpriteRenderer>();
    foreach (var sr in allRenderers) sr.color = Color.red;

    float dir = Random.value > 0.5f ? 1f : -1f;
    rb.bodyType = RigidbodyType2D.Dynamic;
    rb.linearVelocity = new Vector2(dir * bounceForce, 0f);

    yield return new WaitForSeconds(0.08f);
    
    rb.linearVelocity = Vector2.zero;
    rb.bodyType = RigidbodyType2D.Kinematic;

    yield return new WaitForSeconds(0.12f);
    
    foreach (var sr in allRenderers) sr.color = Color.white;
}

    public void BenchPlayer()
    {
        if (isExhausted) return;
        isExhausted = true;
        movement.Stop();

        rb.gravityScale = 0f;
        float direction = movement.isEnemy ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * 1.5f, 0f); // slower exit
        
        Destroy(gameObject, 0.8f); // destroy faster, was 1.5f
    }

    public void Initialize(int atk, int def, int vel)
    {
        attack = atk;
        defense = def;
        maxAttack = atk;
        maxDefense = def;
        velocity = vel;
        fatigueTimer = fatigueTime;
        movement = GetComponent<CharacterMove>();
        if (movement != null)
            movement.speed = velocity;
        UpdateBars();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fightRange);
    }
}