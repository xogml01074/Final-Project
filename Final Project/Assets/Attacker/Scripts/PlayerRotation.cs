using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // 회전 속도 변수
    [SerializeField]
    private float rotSpeed = 300f;

    // 회전 값 변수
    float mx = 0;

    private void Update()
    {
        // 마우스 좌우 입력을 받는다.
        float mouse_X = Input.GetAxis("Mouse X");

        // 회전 값 변수에 마우스 입력 값만큼 미리 누적시킨다.
        mx += mouse_X * rotSpeed * Time.deltaTime;

        // 회전 방향으로 물체를 회전시킨다.
        transform.eulerAngles = new Vector3(0, mx + 60, 0);

    }

}
