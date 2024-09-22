using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public event Action OnCollisionGround;
    public enum State { Idle, Run, Jump, Fall, Size }
    [SerializeField] State curState = State.Idle;
    private BaseState[] states = new BaseState[(int)State.Size];

    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Text stateText;
    private const int Maxhp = 3;
    private int curHp;
    [SerializeField] GameObject[] hpObject = new GameObject[Maxhp];
    [SerializeField] Collider2D attackPoint;
    

    private static int idleHash = Animator.StringToHash("Idle");
    private static int runHash = Animator.StringToHash("Run");
    private static int jumpHash = Animator.StringToHash("Jump");
    private static int fallHash = Animator.StringToHash("Fall");

    private void Awake()
    {
        
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Run] = new RunState(this);
        states[(int)State.Jump] = new JumpState(this);
        states[(int)State.Fall] = new FallState(this);
    }

    private void Start()
    {
        curHp = Maxhp;
        UpdateHpObject();
        states[(int)curState].Enter();
    }

    private void OnDestroy()
    {
        states[(int)curState].Exit();
    }

    private void Update()
    {
        states[(int)curState].Update();
        stateText.text = curState.ToString();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
        {
            if (OnCollisionGround != null)
            {
                OnCollisionGround?.Invoke();
            }
        }

        if(LayerMask.LayerToName(collision.gameObject.layer) == "Enemy")
        {
            //�ǰݽ�
            TakeDamage();
            UpdateHpObject();
        }

    }




    public void ChangeState(State nextState)
    {
        states[(int)curState].Exit();
        curState = nextState;
        states[(int)curState].Enter();
    }

    private void TakeDamage()
    {
        if (curHp > 0)
        {
            curHp--;
        }

        if(curHp <= 0)
        {
            Debug.Log("����");
            GameManager.Instance.GameOverTrigger();
        }
    }

    private void UpdateHpObject()
    {
        for(int i=0; i< hpObject.Length; i++)
        {
            if(i < curHp)
            {
                hpObject[i].SetActive(true);
            }
            else
            {
                hpObject[i].SetActive(false);
            }
            
        }
    }


    private void PlayAnimator(State _curState)
    {
        switch(_curState)
        {
            case State.Idle:
                animator.Play(idleHash);
                break;
            case State.Run:
                animator.Play(runHash);
                break;
            case State.Fall:
                animator.Play(fallHash);
                break;
            case State.Jump:
                animator.Play(jumpHash);
                break;
        }
    }


    #region State

    private class PlayerState : BaseState
    {
        public PlayerController player;

        public PlayerState(PlayerController player)
        {
            this.player = player;
        }
    }

    private class IdleState : PlayerState
    {

        public IdleState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            //Debug.Log("Idle ���� ����");
        }


        public override void Update()
        {
            player.PlayAnimator(State.Idle);

            // Run ���� ����
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
            {
                player.ChangeState(State.Run);
            }

            // Jump ���� ����
            if(Input.GetKeyDown(KeyCode.Space))
            {
                player.ChangeState(State.Jump);
            }



        }
    }

    
    private class RunState : PlayerState
    {
        // ���⿡ �δ� �ν����� �󿡼� Ȯ���� ���� �׳� 
        private float movePower = 5f;
        private float maxMoveSpeed = 6f;
        
        public RunState(PlayerController player) : base(player)
        {
            
        }

        public override void Enter()
        {
            //Debug.Log("Run ���� ����");
        }


        public override void Update()
        {
            player.PlayAnimator(State.Run);

            float x = Input.GetAxisRaw("Horizontal");
            player.rb.velocity = new Vector2(x*movePower, player.rb.velocity.y);
            //player.rb.AddForce(Vector2.right * x * movePower, ForceMode2D.Force);

            


            if (player.rb.velocity.x > maxMoveSpeed)
            {
                player.rb.velocity = new Vector2(maxMoveSpeed, player.rb.velocity.y);
            }
            else if (player.rb.velocity.x < -maxMoveSpeed)
            {
                player.rb.velocity = new Vector2(-maxMoveSpeed, player.rb.velocity.y);
            }

            if (x < 0)
            {
                player.render.flipX = true;
            }
            if (x > 0)
            {
                player.render.flipX = false;
            }

            //Idle ���·� ��ȯ
            //x �Է��� �������� �ӵ��� ������ �� ����� ������ �� �ڿ��������� ���߿� �׽�Ʈ�غ���
            // if(x < 0.01f) ���� �ϸ� �ڷ� ���� idle�� run ���� ������ ��ȣ���� -> ���밪���� ������ ����
            if (Mathf.Abs(player.rb.velocity.x) < 0.01f)
            {
                player.ChangeState(State.Idle);
            }

            // Jump ���� ����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.ChangeState(State.Jump);
            }


        }

    }

    private class JumpState : PlayerState
    {
        private float jumpPower = 10f;
        private float movePower = 2f;
        private float maxMoveSpeed = 6f;

        public JumpState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
           // Debug.Log("���� ����");
            player.rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            player.PlayAnimator(State.Jump);
        }

        public override void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            player.rb.AddForce(Vector2.right * x * movePower, ForceMode2D.Force);
            if (player.rb.velocity.x > maxMoveSpeed)
            {
                player.rb.velocity = new Vector2(maxMoveSpeed, player.rb.velocity.y);
            }
            else if (player.rb.velocity.x < -maxMoveSpeed)
            {
                player.rb.velocity = new Vector2(-maxMoveSpeed, player.rb.velocity.y);
            }

            if (x < 0)
            {
                player.render.flipX = true;
            }
            if (x > 0)
            {
                player.render.flipX = false;
            }




            if (player.rb.velocity.y <=0.01f)
            {
                player.ChangeState(State.Fall);
            }


        }

    }

    private class FallState : PlayerState
    {
        public FallState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
           // Debug.Log("������ ����");
            player.PlayAnimator(State.Fall);
            player.OnCollisionGround += CollideGround;
        }
        public override void Update()
        {
           

        }

        public override void Exit()
        {
            player.OnCollisionGround -= CollideGround;
        }



        private void CollideGround()
        {
          //  Debug.Log("�ٴڿ� ����");
            player.ChangeState(State.Idle);
        }

    }



    #endregion


}
