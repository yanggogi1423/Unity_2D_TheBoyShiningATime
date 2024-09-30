using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyMoveSmart : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;
    
    public UnityEvent onMonsterDeath = new UnityEvent();
    private Spawner _spawner;
    private bool _isDead;
    
    [SerializeField] private float nextMove = 1f;
    [SerializeField] private float thinkTime = 7f;
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        Invoke("Think",thinkTime);
        
        //  Event Handler(죽은 경우 Spawner의 함수를 호출) -> 코드로 동적 연결시킴
        _spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        if (_spawner == null) Debug.Log("Error : Cant find spawner");
        onMonsterDeath.AddListener(_spawner.UpdateSpawn);

        _isDead = false;
    }
    
    private void FixedUpdate()
    {
        //Move
        _rigid.velocity = new Vector2(nextMove,_rigid.velocity.y);
        
        //Platform Check
        if (!_isDead)   //  Destory에 딜레이가 있어 FixedUpdate가 지속 실행되는 문제가 존재. _isDead 변수 도입
        {
            IsHit();
            CheckCliff();
        
            IsWakling();//idle or walking(animator)
            FacingDirection();//flip
        }
    }

    private void IsHit()
    {
        Vector2 upVec = new Vector2(_rigid.position.x,_rigid.position.y);
        Debug.DrawRay(upVec, Vector3.up,new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(upVec,Vector3.up,1f,LayerMask.GetMask("Player"));
        
        if (rayHit.collider != null)
        {
            //  죽는 기능을 Method화 시킴
            Die();
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

    private void DeActive()  
    {
        gameObject.SetActive(false);
    }

    private void Die()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        _anim.SetBool("isHit", true);
        // _collider.enabled = false;
        _rigid.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
            
        //  Event 발생 -> 사망
        _isDead = true;
        onMonsterDeath.Invoke();
        Destroy(gameObject,3f);
    }

    private void Think()//by enemy _thinkTime secs randomly choose it's velocity
    {
        nextMove = Random.Range(-2,3);
        Invoke("Think",thinkTime);
        // _anim.SetFloat("walkSpeed",_nextMove);
    }
}
