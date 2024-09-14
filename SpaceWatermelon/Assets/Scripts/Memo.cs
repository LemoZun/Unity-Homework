using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo
{
    // 행성 초기 위치 생성 후 impulse 등으로 발사
    // 중간 위치에 중력 생성
    // 같은 행성끼리 접촉하면 둘의 중간위치 계산 후 위치에 다음행성 생성
    // 행성마다 굳이 다음 행성들을 모두 가지고있을 이유는 없다
    // pluto 한번까진 moon으로 합쳐지지만 한번 한 뒤에 다시 합쳐지지 않는다
    // 프리팹은 게임오브젝트의 참조를 할 수 없으니까 코드로 넣어줘야함
    // 코루틴으로 일정시간 충돌이 없었을시 삭제도 해야함
    // 



    /*  시행착오
     * 화면의 오른쪽으로 발사하려고 right로 했지만 꼭지점의 오른쪽으로 발사됨
     * up으로 바꾸니 해결됨
     * 삭제와 생성이 자주 반복되니 오브젝트 풀을 구현하면 좋겠지만
     * 일단 구현을 어느정도 한 다음 생각하자
     * phase 가 0인 pluto 끼리 부딪히면 phase가 2인 mars가 양산됨
     * 물리판정은 tag 보다 layer를 쓰는게 맞다고 해서 layer로 고쳐준 뒤 
     * 잘못 적용된 phase 를 새로 적용함
     * 충돌을 했는지 판단하는 여부 bool 변수를 넣고 ontrigger엔 아무작업을 하지 않는다를 넣었다.
     * 
     */
}
