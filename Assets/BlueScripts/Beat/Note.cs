using UnityEngine;

public enum NoteType { DEFAULT, MOVE, ATTACK, ERROR }

public class Note : MonoBehaviour
{
    public NoteType noteType;
    public float speed = 2f; // 音符的移动速度
    public float moveDirection = 1f;  // 音符移动方向，1为从左到右，-1为从右到左
    //public GameObject determine;  //实际判定坐标

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * moveDirection * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="HitCircle")
        {
            HitCircle hitCircle=other.GetComponent<HitCircle>();
            hitCircle.AddCurrentNote(this);
        }
        else if (other.tag=="Destroy")
        {
            if (noteType!=NoteType.ERROR)
            {
                Whole.Batter = 0;
            }
            else
            {
                Whole.NoteExtent.Add("miss");
            }
            Destroy(gameObject);
        }
    }
}
