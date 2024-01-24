using Fusion;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : NetworkBehaviour
{
    // �߻� ��ġ
    public GameObject firePosition;

    // źȯ
    public GameObject bombFactory;

    // ����
    public GameObject mineFactory;

    // ���� ��ġ
    public GameObject minePosition;

    public int mineCount = 5;

    // ��ô �Ŀ�
    public float throwPower;

    // ��ô ���� �ִ� źâ
    public int bulletCount;

    // ���� ��ô ���� źâ
    public int currentCount;

    public Animator playerAnim;

    public Camera bCamera;

    // ���� ��� ����
    public enum WeaponMode
    {
        Normal,
        Reload,
        Destroyer
    }
    public WeaponMode wMode;

    public override void Spawned()
    {
        // ���� �⺻ ��带 ��� ���� �����Ѵ�.
        wMode = WeaponMode.Normal;

        playerAnim = GetComponentInChildren<Animator>();

        bulletCount = 6;
        currentCount = 6;
        throwPower = 10f;
        // bulletTxt.text = $"{currentCount} / 6 ";
    }

    public override void FixedUpdateNetwork()
    {
        Fire();
        Reloading();
        // bulletTxt.text = $"{currentCount} / 6";
        meleeAttack();
        Trapping();
    }
    public void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ź���� 1�� �̻��̶� ���� ��
            if (currentCount >= 1)
            {
                // ����ź ������Ʈ�� ������ �� ����ź�� ���� ��ġ�� �߻� ��ġ�� �Ѵ�.
                GameObject bomb = Instantiate(bombFactory);
                bomb.transform.position = firePosition.transform.position;

                // ����ź ������Ʈ�� Rigidbody ������Ʈ�� �����´�.
                Rigidbody rb = bomb.GetComponent<Rigidbody>();

                // ī�޶��� ���� �������� ����ź�� �������� ���� ���Ѵ�.
                Vector3 throwDir = bCamera.transform.forward + Vector3.up * 0.5f;
                rb.AddForce(throwDir * throwPower, ForceMode.Impulse);

                // ź�� �Ҹ�
                currentCount--;
            }
            else if (currentCount == 0)
            {
                Reloading();
            }
        }
    }
    // �÷��̾ �߻� > �߻��Ѱɷ� ������ > ���� ������ > ������ ������

    public void Trapping()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (mineCount <= 0)
            {
                Debug.Log("Not Having Mine");
                return;
            }

            GameObject mine = Instantiate(mineFactory);
            mine.transform.position = minePosition.transform.position;
            playerAnim.SetTrigger("Plant");
            mineCount--;
        }
    }

    void Reloading()
    {
        if (currentCount == 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(ReloadingDelay());
            }

        }
    }

    IEnumerator ReloadingDelay()
    {
        print("Reloading");
        playerAnim.SetTrigger("Reload");
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
                cols[i].gameObject.GetComponent<ZombieMovement>().Hurt(7);
            }
        }
    }
    public void UBC(int rndN)
    {
        print("Worked");
        print(rndN);
        currentCount += rndN;
        print(gameObject.name);
    }
}
