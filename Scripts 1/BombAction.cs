using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect; // ���� ����Ʈ ������ ����
    public int attackPower = 10; // ����ź ���ݷ�
    public float explosionRadius = 5f; //���� ȿ�� �ݰ�
    private void OnCollisionEnter(Collision collision)
    {
        // ����ź �ݰ� 5���� �ȿ� �ִ� ������Ʈ�� �߿���
        // ���̾ 10(Enemy)�� ������Ʈ���� ��ȯ
        Collider[] cols = Physics.OverlapSphere(transform.position,
            explosionRadius, 1<<10);
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }
        //���� �������� ����
        GameObject eff = Instantiate(bombEffect);
        //���� �������� ����ź ������Ʈ�� ��ġ�� �����ϰ� �Ѵ�.
        eff.transform.position = transform.position;
        Destroy(gameObject); //�ڽ� �ڽ��� �����Ѵ�. 
    }
}
