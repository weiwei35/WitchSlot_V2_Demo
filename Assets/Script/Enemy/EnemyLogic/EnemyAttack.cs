using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float minDistance = 3.0f;
    public float moveSpeed = 5.0f;
    public float coldTime = 0.5f;

    private Player player;
    private Rigidbody2D rb;
    private enum EnemyState { Idle, Charging, Cooldown }
    private EnemyState state = EnemyState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (state == EnemyState.Idle)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= minDistance)
            {
                StartCoroutine(ChargeAndCooldown());
            }
        }
    }

    IEnumerator ChargeAndCooldown()
    {
        state = EnemyState.Charging;

        Vector3 target = GetDashTarget();

        float duration = 0.2f;
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        // 保证最终位置正确
        transform.position = target;

        rb.velocity = Vector2.zero;

        // 进入冷却状态
        state = EnemyState.Cooldown;
        yield return new WaitForSeconds(coldTime);

        // 恢复 Idle
        state = EnemyState.Idle;
    }

    private Vector3 GetDashTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        return transform.position + direction;
    }

    void FixedUpdate()
    {
        if (state == EnemyState.Idle)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero; // 保证在 Charging 和 Cooldown 期间怪不动
        }
    }
}
