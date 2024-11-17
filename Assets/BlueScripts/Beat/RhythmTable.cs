using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class RhythmTable : MonoBehaviour
{
    public GameObject defaultNotePrefab;  // 默认音符预设
    public GameObject moveNotePrefab;     // 移动音符预设
    public GameObject attackNotePrefab;   // 攻击音符预设
    public GameObject errorNotePrefab;    // 错误音符预设

    public static List<Note> notes = new List<Note>();

    private float beatDuration = 0.4f;    // 每个节拍400ms
    private float currentTime = 0f;

    public GameObject theTransform;


    // 生成音符的方法，根据时间生成不同类型的音符
    public void SpawnNote(float spawnTime, NoteType noteType)
    {
        GameObject noteObject = null;
        GameObject noteObject1 = null;
        Note note=null;
        Vector3 currentRotation;
        switch (noteType)
        {
            case NoteType.DEFAULT:
                noteObject = Instantiate(defaultNotePrefab,theTransform.transform.position,Quaternion.identity);
                noteObject1 = Instantiate(defaultNotePrefab,
                                          new Vector3(
                                              -theTransform.transform.position.x + 2*transform.position.x,  // x坐标对称
                                              theTransform.transform.position.y,                             // y坐标保持一致
                                              -theTransform.transform.position.z + 2*transform.position.z   // z坐标对称
                                          ),
                                          Quaternion.identity);
                note = noteObject1.GetComponent<Note>();
                Whole.Notes.Add(note);
                Whole.NoteTypes.Add(note.noteType);
                currentRotation = note.gameObject.transform.localRotation.eulerAngles;
                note.gameObject.transform.localRotation = Quaternion.Euler(currentRotation.x, -180, currentRotation.z);
                break;
            case NoteType.MOVE:
                noteObject = Instantiate(moveNotePrefab, theTransform.transform.position, Quaternion.identity);
                noteObject1 = Instantiate(moveNotePrefab,
                                          new Vector3(
                                              -theTransform.transform.position.x + 2 * transform.position.x,  // x坐标对称
                                              theTransform.transform.position.y,                             // y坐标保持一致
                                              -theTransform.transform.position.z + 2 * transform.position.z   // z坐标对称
                                          ),
                                          Quaternion.identity);
                note = noteObject1.GetComponent<Note>();
                Whole.Notes.Add(note);
                Whole.NoteTypes.Add(note.noteType);
                currentRotation = note.gameObject.transform.localRotation.eulerAngles;
                note.gameObject.transform.localRotation = Quaternion.Euler(currentRotation.x, -180, currentRotation.z);
                break;
            case NoteType.ATTACK:
                noteObject = Instantiate(attackNotePrefab, theTransform.transform.position, Quaternion.identity);
                noteObject1 = Instantiate(attackNotePrefab,
                                          new Vector3(
                                              -theTransform.transform.position.x + 2 * transform.position.x,  // x坐标对称
                                              theTransform.transform.position.y,                             // y坐标保持一致
                                              -theTransform.transform.position.z + 2 * transform.position.z   // z坐标对称
                                          ),
                                          Quaternion.identity);
                note = noteObject1.GetComponent<Note>();
                Whole.Notes.Add(note);
                Whole.NoteTypes.Add(note.noteType);
                currentRotation = note.gameObject.transform.localRotation.eulerAngles;
                note.gameObject.transform.localRotation = Quaternion.Euler(currentRotation.x, -180, currentRotation.z);
                break;
            case NoteType.ERROR:
                noteObject = Instantiate(errorNotePrefab, theTransform.transform.position, Quaternion.identity); 
                noteObject1 = Instantiate(errorNotePrefab,
                                          new Vector3(
                                              -theTransform.transform.position.x + 2 * transform.position.x,  // x坐标对称
                                              theTransform.transform.position.y,                             // y坐标保持一致
                                              -theTransform.transform.position.z + 2 * transform.position.z   // z坐标对称
                                          ),
                                          Quaternion.identity);
                note = noteObject1.GetComponent<Note>();
                Whole.Notes.Add(note);
                Whole.NoteTypes.Add(note.noteType);
                currentRotation = note.gameObject.transform.localRotation.eulerAngles;
                note.gameObject.transform.localRotation = Quaternion.Euler(currentRotation.x, -180, currentRotation.z);
                break;
        }

        if (noteObject != null)
        {
            note = noteObject.GetComponent<Note>();
            notes.Add(note);
        }
    }

    // 更新节奏表
    void Update()
    {
        currentTime += Time.deltaTime;

        // 根据当前时间检查并生成新的音符
        if (currentTime >= beatDuration)
        {
            int p=Random.Range(0, 4);
            switch (p)
            {
                case 0:
                    SpawnNote(currentTime, NoteType.DEFAULT);
                    break;
                case 1:
                    SpawnNote(currentTime, NoteType.ATTACK); 
                    break;
                case 2:
                    SpawnNote(currentTime, NoteType.MOVE);
                    break;
                case 3:
                    SpawnNote(currentTime, NoteType.ERROR);
                    break;
            }
            currentTime = 0f;
        }

        if (Whole.Notes.Count>0)
        {
            if (Whole.Notes[0] == null)
            {
                //if (Whole.NoteTypes[Whole.NoteTypes.Count-Whole.Notes.Count]!=NoteType.ERROR)
                //{
                    Whole.NoteExtent.Add("miss");
                //}
                Whole.Notes.Remove(Whole.Notes[0]);
            }
        }
    }
}
