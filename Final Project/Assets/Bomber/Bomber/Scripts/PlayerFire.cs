using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // �߻� ��ġ
    public GameObject firePosition;

    // ��ô ���� ������Ʈ
    public GameObject bombFactory;

    // ��ô �Ŀ�
    public float throwPower = 10f;

    // ��ô ���� �ִ� źâ
    public int bulletCount = 6;

    // ���� ��ô ���� źâ
    public int currentCount = 6;

    public Animator playerAnim;

    public Text bulletTxt;

    // ���� ��� ����
    enum WeaponMode
    {
        Normal,
        Reload,
        Destroyer
    }
    WeaponMode wMode;

    private void Start()
    {
        // ���� �⺻ ��带 ��� ���� �����Ѵ�.
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
            // ź���� 1�� �̻��̶� ���� ��
            if (currentCount >= 1)
            {
                playerAnim.SetTrigger("Fire");

                // ����ź ������Ʈ�� ������ �� ����ź�� ���� ��ġ�� �߻� ��ġ�� �Ѵ�.
                GameObject bomb = Instantiate(bombFactory);
                bomb.transform.position = firePosition.transform.position;

                // ����ź ������Ʈ�� Rigidbody ������Ʈ�� �����´�.
                Rigidbody rb = bomb.GetComponent<Rigidbody>();

                // ī�޶��� ���� �������� ����ź�� �������� ���� ���Ѵ�.
                Vector3 throwDir = Camera.main.transform.forward + Vector3.up * 0.5f;
                rb.AddForce(throwDir * throwPower, ForceMode.Impulse);

                // ź�� �Ҹ�
                currentCount--;
            }
            else if (currentCount == 0)
            {
                print("No Bullet!");
            }
        }
    }
    // �÷��̾ �߻� > �߻��Ѱɷ� ������ > ���� ������ > ������ ������

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
