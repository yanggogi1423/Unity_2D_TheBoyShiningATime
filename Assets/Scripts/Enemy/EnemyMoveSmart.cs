using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMoveSmart : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;
    
    [SerializeField] private float nextMove = 1f;
    [SerializeField] private float thinkTime = 7f;

    public UnityEvent onMonsterDeath = new UnityEvent();
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        Invoke("Think",thinkTime);
    }
    
    private void FixedUpdate()
    {
        //Move
        _rigid.velocity = new Vector2(nextMove,_rigid.velocity.y);
        //Platform Check
        IsHit();
        CheckCliff();
        IsWakling();//idle or walking(animator)
        FacingDirection();//flip
    }

    private void IsHit()
    {
        Vector2 upVec = new Vector2(_rigid.position.x,_rigid.position.y);
        Debug.DrawRay(upVec, Vector3.up,new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(upVec,Vector3.up,1f,LayerMask.GetMask("Player"));
        if (rayHit.collider != null)
        {
            
            _anim.SetBool("isHit", true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            // _collider.enabled = false;
            _rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
            Destroy(gameObject,3f);
        }
    }
    private void CheckCliff()
    {
        //Platform Check
        Vector2 frontVec = new Vector2(_rigid.position.x + (0.6f * _rigid.velocity.normalized.x),_rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down,new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec,Vector3.down,3,LayerMask.GetMask("Ground"));
        if (rayHit.collider == null)//When meet cliff turn around
        {
            nextMove *= -1f;
            CancelInvoke();
            Invoke("Think",thinkTime);
        }
        
    }
    private void FacingDirection()
    {
        if(_rigid.velocity.x < 0)_sprite.flipX = true;//look at the dir of vec
        else _sprite.flipX = false;
    }
    private void IsWakling()
    {
        if(_rigid.velocity.x != 0)_anim.SetBool("isWalking", true);//abs(velocity)>0 == walking
        else _anim.SetBool("isWalking", false);
    }

    private void Die()
    {
        //  setActive false에서 메모리 해제로 변경
        onMonsterDeath.Invoke();
        Destroy(this);
    }

    private void Think()//by enemy _thinkTime secs randomly choose it's velocity
    {
        nextMove = Random.Range(-2,3);
        Invoke("Think",thinkTime);
        // _anim.SetFloat("walkSpeed",_nextMove);
    }
}
