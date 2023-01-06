using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement_Controller : MonoBehaviour
{
    public event Action<bool> OnGetHurt = delegate { };
    private Rigidbody2D playerRB;
    private Animator playerAnimator;
    private Player_controller playerController;

    [Header("Horizontal movement")]
    [SerializeField] private float speed;
    [Range(0, 1)]
    [SerializeField] private float crouchSpeedReduce;
    private bool faceRight = true;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float radius;
    [SerializeField] private bool airControll;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;
    private bool canDoubleJump;

    [Header("Crouching")]
    [SerializeField] private Transform cellCheck;
    [SerializeField] private Collider2D headCollider;
    private bool canStand;

    [Header("Shooting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private Transform crouchBulletPoint;
    [SerializeField] private float bulletSpeed;
    private bool isShooting;
    private bool isRunShooting;
    private bool isCrouchShooting;

    [Header("PowerShooting")]
    [SerializeField] private GameObject powerBullet;
    [SerializeField] private Transform powerBulletPoint;
    [SerializeField] private Transform powerBulletCrouchPoint;
    [SerializeField] private float powerBulletSpeed;
    [SerializeField] private int powerShootCost;
    private bool isPowerShooting;

    [SerializeField] private float pushForce;
    private float lastHurtTime;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<Player_controller>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);

        if (playerAnimator.GetBool("Hurt") && grounded && Time.time - lastHurtTime > 0.5f)
            EndHurt();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cellCheck.position, radius);
    }

    void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }

    public void Move(float move, bool jump, bool crouch)
    {
        #region Movement
        float speedModificator = crouch ? crouchSpeedReduce : 1;

        if (move != 0 && (grounded || airControll))
            playerRB.velocity = new Vector2(speed * move * speedModificator, playerRB.velocity.y);

        if (move > 0 && !faceRight)
        {
            Flip();
        }
        else if (move < 0 && faceRight)
        {
            Flip();
        }
        #endregion

        #region Jumping
        if (jump)
        {
            if (grounded)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }
        #endregion

        #region Crouching
        canStand = !Physics2D.OverlapCircle(cellCheck.position, radius, whatIsGround);
        if (crouch)
        {
            headCollider.enabled = false;
        }
        else if (!crouch && canStand)
        {
            headCollider.enabled = true;
        }
        #endregion

        #region Animation
        playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        playerAnimator.SetBool("Jump", !grounded);
        playerAnimator.SetBool("GroundCheck", grounded);
        playerAnimator.SetBool("Crouch", !headCollider.enabled);
        #endregion
    }

    public void GetHurt(Vector2 position)
    {
        lastHurtTime = Time.time;
        OnGetHurt(false);
        Vector2 pushDirection = new Vector2();
        pushDirection.x = position.x > transform.position.x ? -1 : 1;
        pushDirection.y = 1;
        playerAnimator.SetBool("Hurt", true);
        playerRB.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
    }

    public void EndHurt()
    {
        playerAnimator.SetBool("Hurt", false);
        OnGetHurt(true);
    }

    public void StartShooting()
    {
        if (isShooting)
            return;

        isShooting = true;
        playerAnimator.SetBool("Shooting", true);
    }

    private void Shoot()
    {
        GameObject _bullet = Instantiate(bullet, bulletPoint.position, Quaternion.identity);
        _bullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
        _bullet.GetComponent<SpriteRenderer>().flipX = !faceRight;
        Destroy(_bullet, 5f);
    }

    private void EndShooting()
    {
        isShooting = false;
        playerAnimator.SetBool("Shooting", false);
    }

    public void StartRunShooting()
    {
        if (isRunShooting)
            return;

        isRunShooting = true;
        playerAnimator.SetBool("Run-Shooting", true);

    }

    private void EndRunShooting()
    {
        isRunShooting = false;
        playerAnimator.SetBool("Run-Shooting", false);
    }

    public void StartCrouchShooting()
    {
        if (isCrouchShooting)
            return;

        isCrouchShooting = true;
        playerAnimator.SetBool("Crouch-Shooting", true);
    }

    private void crouchShoot()
    {
        GameObject _bullet = Instantiate(bullet, crouchBulletPoint.position, Quaternion.identity);
        _bullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
        _bullet.GetComponent<SpriteRenderer>().flipX = !faceRight;
        Destroy(_bullet, 5f);
    }

    private void EndCrouchShooting()
    {
        isCrouchShooting = false;
        playerAnimator.SetBool("Crouch-Shooting", false);
    }

    public void StartPowerShooting()
    {
        if (isPowerShooting || !playerController.ChangeMp(-powerShootCost))
            return;

        isPowerShooting = true;
        playerAnimator.SetBool("Power-Shooting", true);
    }

    private void powerShoot()
    {
        GameObject _powerBullet = Instantiate(powerBullet, powerBulletPoint.position, Quaternion.identity);
        _powerBullet.GetComponent<Rigidbody2D>().velocity = transform.right * powerBulletSpeed;
        _powerBullet.GetComponent<SpriteRenderer>().flipX = !faceRight;
        Destroy(_powerBullet, 5f);
    }

    private void powerCrouchShoot()
    {
        GameObject _powerBullet = Instantiate(powerBullet, powerBulletCrouchPoint.position, Quaternion.identity);
        _powerBullet.GetComponent<Rigidbody2D>().velocity = transform.right * powerBulletSpeed;
        _powerBullet.GetComponent<SpriteRenderer>().flipX = !faceRight;
        Destroy(_powerBullet, 5f);
    }

    private void EndPowerShooting()
    {
        isPowerShooting = false;
        playerAnimator.SetBool("Power-Shooting", false);
    }

    private void ResetPlayer()
    {
        playerAnimator.SetBool("Hurt", false);
        playerAnimator.SetBool("Power-Shooting", false);
        playerAnimator.SetBool("Crouch-Shooting", false);
        playerAnimator.SetBool("Run-Shooting", false);
        playerAnimator.SetBool("Shooting", false);
        playerAnimator.SetBool("Crouch", false);
        playerAnimator.SetBool("Jump", false);
        isShooting = false;
        isRunShooting = false;
        isCrouchShooting = false;
        isPowerShooting = false;
    }
}
