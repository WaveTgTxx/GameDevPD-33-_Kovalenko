using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    public float _groundCheckerRadius;
    public Transform _groundChecker;
    public LayerMask _whatIsGround;
    public Transform _transform;
    public Rigidbody2D _rigidbody;
    public float _speed;
    public float _jumpPower;


    public Collider2D _headCollider;
    public Transform _cellChecker;
    public float _cellCheckerRadius;


    public Animator _animator;
    public string _runAnimationKey;
    public string _crouchAnimationKey;

    private bool _facingRight = true;

    void Update()
    {

        var grounded = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);


        float direction = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = _rigidbody.velocity;
        _animator.SetBool(_runAnimationKey, direction != 0);
      

        if (grounded)
        {
            _rigidbody.velocity = new Vector2(_speed * direction, velocity.y);

            if (direction < 0 && _facingRight)
            {
                Flip();
            }

            else if (direction > 0 && !_facingRight)
            {
                Flip(); 
            }

            if (Input.GetButtonUp("Jump") && grounded)
            {
                _rigidbody.velocity = new Vector2(velocity.x, _jumpPower);

            }

            bool CellAbove = Physics2D.OverlapCircle(_cellChecker.position, _cellCheckerRadius, _whatIsGround);


            _animator.SetBool(_crouchAnimationKey, !_headCollider.enabled);

            if (Input.GetKey(KeyCode.S))
            {
                _headCollider.enabled = false;
            }

            else if (!CellAbove)
            {
                _headCollider.enabled = true;
            }

        }

       

        if (direction < 0 && _facingRight)
        {
            Flip();
        }

        else if (direction > 0 && !_facingRight)
        {
            Flip();
        }

        if (Input.GetButtonUp("Jump") && grounded)
        {
            _rigidbody.velocity = new Vector2(velocity.x, _jumpPower);
                      
        }
    }


    private void Flip() 
    {
        _facingRight = !_facingRight;
        _transform.Rotate(0, 180, 0);
    }
    
    
    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_cellChecker.position, _cellCheckerRadius);
    }
    
    
}
