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
            //피격시
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
            Debug.Log("죽음");
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
            //Debug.Log("Idle 상태 진입");
        }


        public override void Update()
        {
            player.PlayAnimator(State.Idle);

            // Run 진입 조건
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
            {
                player.ChangeState(State.Run);
            }

            // Jump 진입 조건
            if(Input.GetKeyDown(KeyCode.Space))
            {
                player.ChangeState(State.Jump);
            }



        }
    }

    
    private class RunState : PlayerState
    {
        // 여기에 두니 인스펙터 상에서 확인이 힘들어서 그냥 
        private float movePower = 5f;
        private float maxMoveSpeed = 6f;
        
        public RunState(PlayerController player) : base(player)
        {
            
        }

        public override void Enter()
        {
            //Debug.Log("Run 상태 진입");
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

            //Idle 상태로 전환
            //x 입력이 없을때와 속도가 작을때 두 경우중 무엇이 더 자연스러운지 나중에 테스트해보자
            // if(x < 0.01f) 으로 하면 뒤로 갈때 idle과 run 상태 진입이 모호해짐 -> 절대값으로 조건을 변경
            if (Mathf.Abs(player.rb.velocity.x) < 0.01f)
            {
                player.ChangeState(State.Idle);
            }

            // Jump 진입 조건
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
           // Debug.Log("점프 진입");
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
           // Debug.Log("떨어짐 진입");
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
          //  Debug.Log("바닥에 닿음");
            player.ChangeState(State.Idle);
        }

    }



    #endregion


}
