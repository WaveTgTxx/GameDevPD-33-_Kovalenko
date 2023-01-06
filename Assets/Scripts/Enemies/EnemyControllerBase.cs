using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class EnemyControllerBase : MonoBehaviour
{
    protected Rigidbody2D enemyRb;
    protected Animator enemyAnimator;

    [Header("HP")]
    [SerializeField] private int maxHP;
    private int currHP;

    [Header("StateChanges")]
    [SerializeField] private float maxStateTime;
    [SerializeField] private float minStateTime;
    [SerializeField] private EnemyState[] availableState;
    protected EnemyState currState;
    protected float lastStateChange;
    protected float timeToNextChange;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    protected Vector2 startPoint;
    private bool faceRight = true;

    [Header("DamageDealer")]
    [SerializeField] private DamageType collisionDamageType;
    [SerializeField] protected int collisionDamage;
    [SerializeField] protected float collisionTimeDelay;
    private float lastDamageTime;

    private void Start()
    {
        startPoint = transform.position;
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        currHP = maxHP;
    }

    protected void Update()
    {
        if (Time.time - lastStateChange > timeToNextChange)
            GetRandomState();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        TryToDamage(collision.collider);
    }

    protected virtual void TryToDamage(Collider2D enemy)
    {
        if ((Time.time - lastDamageTime) < collisionTimeDelay)
            return;

        Player_controller player = enemy.GetComponent<Player_controller>();
        if (player != null)
            player.TakeDamage(collisionDamage, collisionDamageType, transform);
    }

    private void FixedUpdate()
    {
        if (IsGroundEnding())
            Flip();

        if (currState == EnemyState.Move)
            Move();
    }

    protected virtual void Move()
    {
        enemyRb.velocity = transform.right * new Vector2(speed, enemyRb.velocity.y);
    }

    protected void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(range * 2, 0.5f, 0));
    }

    private bool IsGroundEnding()
    {
        return !Physics2D.OverlapPoint(groundCheck.position, whatIsGround);
    }

    protected virtual void GetRandomState()
    {
        int state = Random.Range(0, availableState.Length);

        if (currState == EnemyState.Idle && availableState[state] == EnemyState.Idle)
        {
            GetRandomState();
        }

        timeToNextChange = Random.Range(minStateTime, maxStateTime);
        ChangeState(availableState[state]);
    }

    protected virtual void ChangeState(EnemyState state)
    {
        if (currState != EnemyState.Idle)
            enemyAnimator.SetBool(currState.ToString(), false);

        if (state != EnemyState.Idle)
            enemyAnimator.SetBool(state.ToString(), true);

        currState = state;
        lastStateChange = Time.time;
    }


    public enum EnemyState
    {
        Idle,
        Move,
    }
}
