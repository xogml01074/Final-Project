using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // 발사 위치
    public GameObject firePosition;

    // 투척 무기 오브젝트
    public GameObject bombFactory;

    // 투척 파워
    public float throwPower = 10f;

    // 투척 무기 최대 탄창
    public int bulletCount = 6;

    // 현재 투척 무기 탄창
    public int currentCount = 6;

    public Animator playerAnim;

    public Text bulletTxt;

    // 무기 모드 변수
    enum WeaponMode
    {
        Normal,
        Reload,
        Destroyer
    }
    WeaponMode wMode;

    private void Start()
    {
        // 무기 기본 모드를 노멀 모드로 설정한다.
        wMode = WeaponMode.Normal;

        playerAnim = GetComponentInChildren<Animator>();
        // bulletTxt.text = $"{currentCount} / 6 ";
    }

    private void Update()
    {
        Fire();
        Reloading();
        // bulletTxt.text = $"{currentCount} / 6";
        meleeAttack();
    }
    public void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 탄약이 1개 이상이라도 있을 때
            if (currentCount >= 1)
            {
                playerAnim.SetTrigger("Fire");

                // 수류탄 오브젝트를 생성한 후 수류탄의 생성 위치를 발사 위치로 한다.
                GameObject bomb = Instantiate(bombFactory);
                bomb.transform.position = firePosition.transform.position;

                // 수류탄 오브젝트의 Rigidbody 컴포넌트를 가져온다.
                Rigidbody rb = bomb.GetComponent<Rigidbody>();

                // 카메라의 정면 방향으로 수류탄에 물리적인 힘을 가한다.
                Vector3 throwDir = Camera.main.transform.forward + Vector3.up * 0.5f;
                rb.AddForce(throwDir * throwPower, ForceMode.Impulse);

                // 탄약 소모
                currentCount--;
            }
            else if (currentCount == 0)
            {
                print("No Bullet!");
            }
        }
    }
    // 플레이어가 발사 > 발사한걸로 터져서 > 적을 잡으면 > 점수가 오르게

    void Reloading()
    {
        if (currentCount == 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerAnim.SetTrigger("Reload");
                StartCoroutine(ReloadingDelay());
            }

        }
    }

    IEnumerator ReloadingDelay()
    {
        print("Reloading");
        yield return new WaitForSeconds(3F);
        currentCount = bulletCount;
    }

    void meleeAttack()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            playerAnim.SetTrigger("Attack");
            Collider[] cols = Physics.OverlapSphere(transform.position, 1f, 1 << 8);
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].gameObject.GetComponent<EnemyTest>().HitEnemy(7);
            }
        }
    }
}
