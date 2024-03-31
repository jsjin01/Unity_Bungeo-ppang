using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] float speed = 2f; // ���� �̵� �ӵ�
    [SerializeField] float hp = 500f;//���� ü��
    [SerializeField] float MaxHp = 500f;//���� �ִ� ü��
    Vector2 pos = new Vector2(0, -1);
    Rigidbody2D rb;
    SpriteRenderer sr;

    IEnumerator fireCor; //���� ���� �ڷ�ƾ �Լ�
    IEnumerator iceCor;
    IEnumerator thunderCor;
    private void Start()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        InvokeRepeating("SpawnSeed", 1f, 2f);
    }
    private void Update()
    {
        if (hp <= 0f)
        {
            BossDestroy();
        }
    }

    void SpawnSeed()
    {
        EnemyPoolManager.i.CreateSeed();
    }
    
    private void OnEnable()
    {
        hp = MaxHp;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bungeo_ppong_BulletComponent bullet = other.gameObject.GetComponent<Bungeo_ppong_BulletComponent>();
            StartCoroutine(Hitchange());
            if (bullet.isShield)
            {
                bullet.isShield = false;
            }
            else
            {
                hp -= bullet.dmg;
                if (hp <= 0f)
                {
                    BossDestroy();
                }
            }
        }
        else if (other.CompareTag("FIRE"))
        {
            MagicBall fireball = other.gameObject.GetComponent<MagicBall>();
            hp -= fireball.dmg;
            if (hp <= 0f)
            {
                BossDestroy();
            }
            else
            {
                if (fireCor != null)
                {
                    StopCoroutine(fireCor);
                }
                fireCor = Fire(fireball.firedmg);
                StartCoroutine(fireCor);
            }
        }
        else if (other.CompareTag("ICE"))
        {
            MagicBall iceball = other.gameObject.GetComponent<MagicBall>();
            hp -= iceball.dmg;
            if (hp <= 0f)
            {
                BossDestroy();
            }
            else
            {
                if (iceCor != null)
                {
                    StopCoroutine(iceCor);
                }
                iceCor = Ice(iceball.iceTime);
                StartCoroutine(iceCor);
            }
        }
        else if (other.CompareTag("Shield"))
        {
            Shield shield = other.gameObject.GetComponent<Shield>();
            hp -= shield.dmg;
            if (hp <= 0f)
            {
                BossDestroy();
            }
        }
        else if (other.CompareTag("Sword"))
        {
            Sword sword = other.gameObject.GetComponent<Sword>();
            hp -= sword.dmg;
            if (hp <= 0f)
            {
                BossDestroy();
            }
        }
        else if (other.CompareTag("THUNDER"))
        {
            Thunder t = other.gameObject.GetComponentInParent<Thunder>();
            hp -= t.dmg;
            if (hp <= 0f)
            {
                BossDestroy();
            }
            else
            {
                if (thunderCor != null)
                {
                    StopCoroutine(thunderCor);
                }
                thunderCor = Stun(t.stunTime);
                StartCoroutine(thunderCor);
            }
        }
    }

    void BossDestroy() //�� ����
    {
        hp = 500f; //���߿� �ٽ� ����� �� Hp 500
        Destroy(gameObject);
        if (fireCor != null)
        {
            StopCoroutine(fireCor);
        }
        if (iceCor != null)
        {
            StopCoroutine(iceCor);
        }
        if (thunderCor != null)
        {
            StopCoroutine(thunderCor);
        }
    }

    IEnumerator Fire(float dmg)
    {
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.5f);
            hp -= dmg;//�Ҳ� ��Ʈ ������
        }
    }

    IEnumerator Ice(float t) //�̵��ӵ� 0���� ����� ��Ŀ���� ��ü
    {
        float nowSpeed = speed;// ���� �ӵ� ����
        //speed = 0;          //�ӵ� ����
        //rb.velocity = pos.normalized * speed;
        yield return new WaitForSeconds(t);
        //speed = nowSpeed; //�ٽ� �ǵ��ƿ�
        //rb.velocity = pos.normalized * speed;
    }

    IEnumerator Stun(float time) //���� => ��Ŀ���� ��ü?
    {
        float nowSpeed = speed;// ���� �ӵ� ����
        //speed = 0;//����
        //rb.velocity = pos.normalized * speed;
        yield return new WaitForSeconds(time);
        //speed = nowSpeed; //�ٽ� �ǵ��ƿ�
        //rb.velocity = pos.normalized * speed;
    }

    IEnumerator Hitchange()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }
}