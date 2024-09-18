using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    private float _speed = 10.0f; //좌우 이동속도값
    private float _jumpSpeed = 20.0f; //점프 이동속도값
    private bool _isGround = true; //점프 가능여부 판단 변수
    public int playerHp = 4;

    private Rigidbody2D _rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

	}

    void Update()
    {
        Move();
        Jump();
    }
	void FixedUpdate()
	{
		
	}

    //플레이어 이동관련 함수
    void Move()
    {
		//x축 방향으로 정해진 속도만큼 이동
		float x = Input.GetAxisRaw("Horizontal");
		_rigid.velocity = new Vector2(x * _speed, _rigid.velocity.y);

		//플레이어 좌우 방향 조절
		if (Input.GetButtonDown("Horizontal"))
		{
			spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
		}

		//플레이어 이동 애니메이션
		if (_rigid.velocity.x == 0)
		{
			animator.SetBool("isRun", false);
		}
		else
		{
			animator.SetBool("isRun", true);
		}
	}

	//플레이어의 점프관련 함수
	void Jump()
    {
        //공중이 아닌 상태에서 space바를 누르면 점프
        if (Input.GetKeyDown(KeyCode.Space)&&_isGround)
        {
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            _isGround = false;
			animator.SetBool("isJump", true);   
		}
        
        //Raycast를 이용하는 경우
        /*
        if(_rigid.velocity.y < 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(_rigid.position, Vector2.down, 1, LayerMask.GetMask("Ground"));
            if (rayHit.collider != null)
            {
                if(rayHit.distance < 0.2)
                {
                    animator.SetBool("isJump", false);
                }
            }
        }
        */
    }


    //땅과의 충돌을 체크하는 함수
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Ground태그인 물체에 접촉해 있을때(점프상태가 아닐시)
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround=true;
            animator.SetBool("isJump", false);
        }
        //몬스터에게 피해를 입은 경우
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }
    //피격관련 함수
    void OnDamaged(Vector2 nowPos)
    {
        playerHp--;
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        //피격시 이동
        int dict = 0;
        if (transform.position.x - nowPos.x > 0)
            dict = 1;
        else
            dict = -1;
        _rigid.AddForce(new Vector2(dict,1)*1,ForceMode2D.Impulse);

        //피격 애니
        animator.SetTrigger("isDamaged");

        Invoke("OffDamaged", 2);
    }
    void OffDamaged()
    {
        gameObject.layer = 7;
		spriteRenderer.color = new Color(1, 1, 1,1f);
	}
}