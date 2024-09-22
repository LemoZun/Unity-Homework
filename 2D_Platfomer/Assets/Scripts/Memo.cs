using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo 
{
    /*
     * moveSpeed 같은걸 인스펙터상에서 보려고 RunState 앞에 system.serializable 을 붙여주었지만 안보인다
     * 각 상태에서 사용하는 변수는 해당 항태 클래스에만 넣어두고싶은데 
     * math는 system에 포함됨, mathf는 unityengine에 포함됨
     * playAnimator의 사용처가 뭔가 모호하다.. 사용위치가..
     * 미끄러지듯 이동하는것도 고쳐야함
     * Monobehaviour를 상속받지 않아 fall 상태일때 바닥에 충돌시 onCollisionEnter2D를 사용하지 못함
     * 이벤트를 사용해서 바닥에 닿을때 상태를 바꾸는걸로 사용하면 
     * 이벤트는 공격당했을 경우에 사용하고 바닥은 아래쪽으로 향하는 velocity가 0이 되었을때로 구현하면 어떨까
     * 바닥에 닿았을때 이벤트로 해주는편이 더 편리하고 정확한 판정이다..
     * 게임클리어 메세지가 왜 재시작 후에도 계속 나오지
     * 
     * 플레이어쪽에서 건드린게 없는데 갑자기 플레이어의 움직임이 제자리걸음이 되는 현상이 나타났다
     * 왜??
     * 이동을 velocity로 주는편이 미끄러지는 판정이 없어서 좀 괜찮은거같다
     * 어느 상태건 죽음은 발생할 수 있으니 죽음으로 가는 상태만 따로 만들었는데 이런 상태가 있을때마다 따로 만들어야하나?
     * 피격 모션에셋도 있으니 만들고싶은데 게임 재시작 처리가 해결이 안된다..
     * 
     * 
     */
}
