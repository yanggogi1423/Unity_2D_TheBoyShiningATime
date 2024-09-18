using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemyMoveSmart : MonoBehaviour
{
    Rigidbody2D _rigid;
    Animator _anim;
    SpriteRenderer _sprite;
    BoxCollider2D _collider;
    private float _nextMove = 1f;
    private float _thinkTime = 7f;
    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        Invoke("Think",_thinkTime);
    }

    
    void FixedUpdate()
    {
        //Move
        _rigid.velocity = new Vector2(_nextMove,_rigid.velocity.y);
        //Platform Check
        IsHit();
        CheckCliff();
        IsWakling();//idle or walking(animator)
        FacingDirection();//flip
        
        
    }

    void IsHit()
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
    void CheckCliff()
    {
        //Platform Check
        Vector2 frontVec = new Vector2(_rigid.position.x + (0.6f * _rigid.velocity.normalized.x),_rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down,new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec,Vector3.down,1,LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)//When meet cliff turn around
        {
            _nextMove *= -1f;
            CancelInvoke();
            Invoke("Think",_thinkTime);
        }
        
    }
    void FacingDirection()
    {
        if(_rigid.velocity.x < 0)_sprite.flipX = true;//look at the dir of vec
        else _sprite.flipX = false;
    }
    void IsWakling()
    {
        if(_rigid.velocity.x != 0)_anim.SetBool("isWalking", true);//abs(velocity)>0 == walking
        else _anim.SetBool("isWalking", false);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
    void Think()//by enemy _thinkTime secs randomly choose it's velocity
    {
        _nextMove = Random.Range(-2,3);
        Invoke("Think",_thinkTime);
        // _anim.SetFloat("walkSpeed",_nextMove);
    }
}
