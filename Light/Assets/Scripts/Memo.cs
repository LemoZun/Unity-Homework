using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo : MonoBehaviour
{
    // 효과음을 여러개 재생하려면 어떻게 해야하지?
    // 수업때 잠깐 풀링해서 해준다는 말을 들은거같다 
    // 일단 사운드 배열을 만들어보자
    // 배열을 playoneshot해서 배열을 순회하며 재생할 수 있지만 이건 지금 필요한게 아닌거같다
    // 일단 원하는대로 동작하는데 이게 맞나? 
    // 효과음마다 오브젝트를 만들고 거기에 오디오 소스를 넣어주고 컨트롤러 클립에도 


    // 파티클 설정 하나하나 만져보는데 뭔가 바뀌는것 같지가 않다..
    // 이펙트가 나오게끔 했는데 이젠 루프를 껐는데도 계속 나온다 
    // 왜 이펙트가 안꺼지지? 루핑도 껐고 stop ation도 disable로 해봤는데..
    // 이젠 또 아예 이펙트가 안나오네..
    // 총구의 방향과 실제 맞추고싶은 방향이 달라 플레이어 몸 중간에 realMuzzlePoint를 뒀다


    // 피격한곳에 파티클 효과를 내려면 레이캐스트로 충돌 판정을 하면 되나?

}
