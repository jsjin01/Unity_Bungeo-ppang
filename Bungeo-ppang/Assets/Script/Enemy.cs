using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed = 2f; // 적의 이동 속도
    [SerializeField] protected float hp = 100f;//적의 체력
    [SerializeField] protected float MaxHp = 100f;//적의 최대 체력
    protected Vector2 pos = new Vector2(0, -1);
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected IEnumerator fireCor; //마법 적용 코루틴 함수
    protected IEnumerator iceCor;
    protected IEnumerator thunderCor;
    public virtual void Update()
    {
        if (transform.position.y <= -6f || hp <= 0f)
        {
            EnemyDestroy();
        }
    }
    public virtual void OnEnable()
    {
        hp = MaxHp;
        speed = 2f;
    }

    public virtual void Move()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            
        }
        if(sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        rb.velocity = pos.normalized * speed;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
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
                    EnemyDestroy();
                }
            }
        }
        else if (other.CompareTag("FIRE"))
        {
            MagicBall fireball = other.gameObject.GetComponent<MagicBall>();
            hp -= fireball.dmg;
            if (hp <= 0f)
            {
                EnemyDestroy();
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
                EnemyDestroy();
            }
            else
            {
                if (iceCor != null)
                {
                    StopCoroutine(iceCor);
                }
                iceCor = Ice(iceball.iceSpeedDown);
                StartCoroutine(iceCor);
            }
        }
        else if (other.CompareTag("Shield"))
        {
            Shield shield = other.gameObject.GetComponent<Shield>();
            hp -= shield.dmg;
            if (hp <= 0f)
            {
                EnemyDestroy();
            }
        }
        else if (other.CompareTag("Sword"))
        {
            Sword sword = other.gameObject.GetComponent<Sword>();
            hp -= sword.dmg;
            if (hp <= 0f)
            {
                EnemyDestroy();
            }
        }
        else if (other.CompareTag("THUNDER"))
        {
            Thunder t = other.gameObject.GetComponentInParent<Thunder>();
            hp -= t.dmg;
            if (hp <= 0f)
            {
                EnemyDestroy();
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

    virtual public void EnemyDestroy() //적 삭제
    {
        hp = 100f; //나중에 다시 사용할 때 Hp 100
        EnemyPoolManager.i.ReturnEnemy(gameObject);
        if(fireCor != null)
        {
            StopCoroutine(fireCor);
        }
        if(iceCor != null)
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
            hp -= dmg;//불꽃 도트 데미지
        }
    }

    IEnumerator Ice(float speedDown)
    {
        float nowSpeed = speed;// 현재 속도 저장
        speed *= speedDown;//감속
        rb.velocity = pos.normalized * speed;
        yield return new WaitForSeconds(3f);
        speed = nowSpeed; //다시 되돌아옴
        rb.velocity = pos.normalized * speed;
    }

    IEnumerator Stun(float time) //경직
    {
        float nowSpeed = speed;// 현재 속도 저장
        speed = 0;//감속
        rb.velocity = pos.normalized * speed;
        yield return new WaitForSeconds(time);
        speed = nowSpeed; //다시 되돌아옴
        rb.velocity = pos.normalized * speed;
    }

    public virtual IEnumerator Hitchange()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }
}


