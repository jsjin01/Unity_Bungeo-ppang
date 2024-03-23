using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] bool autoAttack;

    float atkspd;     //공격속도
    float speed;      //이동속도

    Vector3 movement;       //move에서 사용할 변수 => 이동할 위치의 변수
    Quaternion rotation;    //회전값

    bool isShot = true;     //발사 가능 변수

    [SerializeField]GameObject[] magicPrefebs;
    [SerializeField] GameObject shieldPrefebs;
    //[SerializeField] GameObject swordPrefebs;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = PlayerManager.i.speed;
        atkspd = PlayerManager.i.atk_spd;
        rotation = Quaternion.Euler(0, 0, 0); //회전하지 않은 상태
    }

    // Update is called once per frame
    void Update()
    {
        if (autoAttack == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (isShot)
                {
                    Attack();
                }
            }
        }
        else
        {
            if(isShot)
            {
                Attack();
            }
           
        }
    }

    private void FixedUpdate()
    {
        Move(Input.GetAxisRaw("Horizontal"));
    }

    void Move(float x)
    {
        movement.Set(x, 0, 0);
        movement = movement.normalized * speed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);
    }

    void Attack()
    {
        StartCoroutine(ShootCol());
        
        Bungeo_ppong_PoolManager.i.UseBuneo_ppong(transform.position, rotation);
        Instantiate(magicPrefebs[0], transform.position - new Vector3(1, 0, 0) ,rotation);//파이어볼이 잘되는지 실험
        Instantiate(magicPrefebs[1], transform.position - new Vector3(-1, 0, 0), rotation);//아이스볼이 잘 되는지 실험
        ThunderCreat();//번개 실험
        
        Instantiate(shieldPrefebs, transform.position, rotation);
        /*Bungeo_ppong_BulletComponent.b.transform.position = transform.position;
        if(Bungeo_ppong_BulletComponent.b.isDestroy==true)
        {
            Instantiate(swordPrefebs,transform.position, Sword.s.rotation);
        }*/
                
    }

    IEnumerator ShootCol()
    {
        isShot = false;
        yield return new WaitForSeconds(atkspd);
        isShot = true;
    }

    void ThunderCreat()
    {
        float x1 = Random.Range(-2.7f, 2.7f);
        float x2 = Random.Range(-2.7f, 2.7f);
        Debug.Log(x1 + " " + x2);
        Vector2 thunderAngle = new Vector2(x2, 8)- new Vector2(x1, 0);
        thunderAngle.Normalize();
        Quaternion thunderQ = new Quaternion();
        thunderQ.eulerAngles = thunderAngle;
        Instantiate(magicPrefebs[2], new Vector3(x1, 0, 0), thunderQ);
    }
}
