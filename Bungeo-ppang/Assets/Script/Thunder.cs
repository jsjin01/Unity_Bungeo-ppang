using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] public float dmg;        //공격력
    [SerializeField] public float stunTime = 0.5f;   //경직 시간
    [SerializeField] Animation anim;

    void Start()
    {
        dmg = PlayerManager.i.thunder_dmg;
        Invoke("ThunderDestory", 0.5f);
    }
    void ThunderDestory()
    {
        Destroy(gameObject);
    }
}
