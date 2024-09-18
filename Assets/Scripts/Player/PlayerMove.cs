using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
	[SerializeField] private float speed = 10.0f; //�¿� �̵��ӵ���
	[SerializeField] private float jumpSpeed = 20.0f; //���� �̵��ӵ���
	[SerializeField] private int playerHp = 4;

	[SerializeField] private bool isGround = true; //���� ���ɿ��� �Ǵ� ����
	
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _rigid.gravityScale = 10.0f;
    }

    void Update()
    {
        Move();
        Jump();
    }
	void FixedUpdate()
	{
		
	}

    //�÷��̾� �̵����� �Լ�
    void Move()
    {
		//x�� �������� ������ �ӵ���ŭ �̵�
		float x = Input.GetAxisRaw("Horizontal");
		_rigid.velocity = new Vector2(x * speed, _rigid.velocity.y);

		//�÷��̾� �¿� ���� ����
		if (Input.GetButtonDown("Horizontal"))
		{
			_spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
		}

		//�÷��̾� �̵� �ִϸ��̼�
		if (_rigid.velocity.x == 0)
		{
			_animator.SetBool("isRun", false);
		}
		else
		{
			_animator.SetBool("isRun", true);
		}
	}

	//�÷��̾��� �������� �Լ�
	void Jump()
    {
        //������ �ƴ� ���¿��� space�ٸ� ������ ����
        if (Input.GetKeyDown(KeyCode.Space)&&isGround)
        {
            _rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            isGround = false;
			_animator.SetBool("isJump", true);   
		}
        
        //Raycast�� �̿��ϴ� ���
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


    //������ �浹�� üũ�ϴ� �Լ�
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Ground�±��� ��ü�� ������ ������(�������°� �ƴҽ�)
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround=true;
            _animator.SetBool("isJump", false);
        }
        //���Ϳ��� ���ظ� ���� ���
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }
    //�ǰݰ��� �Լ�
    void OnDamaged(Vector2 nowPos)
    {
        playerHp--;
        gameObject.layer = 8;
        _spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        //�ǰݽ� �̵�
        int dict = 0;
        if (transform.position.x - nowPos.x > 0)
            dict = 1;
        else
            dict = -1;
        _rigid.AddForce(new Vector2(dict,1)*1,ForceMode2D.Impulse);

        //�ǰ� �ִ�
        _animator.SetTrigger("isDamaged");

        Invoke("OffDamaged", 2);
    }
    void OffDamaged()
    {
        gameObject.layer = 7;
		_spriteRenderer.color = new Color(1, 1, 1,1f);
	}
}
