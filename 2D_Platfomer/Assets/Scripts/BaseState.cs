using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public virtual void Enter() { } // 상태 시작
    public virtual void Update() { } // 상태 동작중
    public virtual void Exit() { } // 상태 마무리
}
