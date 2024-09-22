using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum State { Idle, Trace, Return, Attack, Dead, Size}
    [SerializeField] State curState = State.Idle;
    private BaseState[] states = new BaseState[(int)State.Size];

    [SerializeField] GameObject player;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float traceRange;
    [SerializeField] float attackRange;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 startPos;

    private void Awake()
    {
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Dead] = new DeadState(this);
    }

    private void Start()
    {
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");

        states[(int)curState].Enter();
    }



    private void OnDestroy()
    {
        states[(int)curState].Exit();
    }

    private void Update()
    {
        states[(int)(curState)].Update();
    }

    private void ChangeState(State nextState)
    {
        states[(int)curState].Exit();
        curState = nextState;
        states[(int)(curState)].Enter();
    }

    public void ChangeToDeathState()
    {
        states[(int)curState].Exit();
        curState = State.Dead;
        states[(int)(curState)].Enter();
    }

    private class MonsterState : BaseState
    {
        public Monster monster;

        public MonsterState(Monster monster)
        {
            this.monster = monster;
        }
    }

    //Idle, Trace, Return, Attack, Dead
    private class IdleState : MonsterState
    {
        public IdleState(Monster monster) : base(monster)
        {
        }

        public override void Update()
        {
            //다른 상태로 전환할때
            if(Vector2.Distance(monster.transform.position, monster.player.transform.position) < monster.traceRange)
            {
                monster.ChangeState(State.Trace);
            }
        }
    }

    private class TraceState : MonsterState
    {
        public TraceState(Monster monster) : base(monster)
        {
        }

        public override void Update()
        {
            monster.transform.position = Vector2.MoveTowards(monster.transform.position, monster.player.transform.position, monster.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(monster.transform.position, monster.player.transform.position) > monster.traceRange)
            {
                monster.ChangeState(State.Return);
            }
            if (Vector2.Distance(monster.transform.position, monster.player.transform.position) < monster.attackRange)
            {
                monster.ChangeState(State.Attack);
            }
        }

    }

    private class ReturnState : MonsterState
    {
        public ReturnState(Monster monster) : base(monster)
        {
        }

        public override void Update()
        {
            monster.transform.position = Vector2.MoveTowards(monster.transform.position, monster.startPos, monster.moveSpeed * Time.deltaTime);

            if(Vector2.Distance(monster.transform.position, monster.startPos) < 0.01f)
            {
                monster.ChangeState(State.Idle);
            }
        }
    }

    private class AttackState : MonsterState
    {
        public AttackState(Monster monster) : base(monster)
        {
        }

        public override void Update()
        {
            //공격 구현

            if (Vector2.Distance(monster.transform.position, monster.player.transform.position) > monster.attackRange)
            {
                monster.ChangeState((State)State.Trace);
            }
        }

    }

    private class DeadState : MonsterState
    {
        public DeadState(Monster monster) : base(monster)
        {
        }

        public override void Enter()
        {
            Instantiate(monster.coinPrefab, monster.transform.position, Quaternion.identity);

            Exit();
        }

        public override void Exit()
        {
            Destroy(monster.gameObject);
        }

    }

}
