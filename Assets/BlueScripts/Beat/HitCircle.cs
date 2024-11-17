using UnityEngine;
using System.Collections.Generic;  // 引入 List
using UnityEngine.tvOS;
using DG.Tweening.Core;

public class HitCircle : MonoBehaviour
{
    public float radius;  // 判定圈的半径
    public float perfectThreshold;  // 完美判定时间窗口
    public float goodThreshold;     // 良好判定时间窗口
    public List<Note> currentNotes = new List<Note>();  // 当前目标音符的列表

    void Update()
    {
        if (Whole.errorTimer>=0f)
        {
            Whole.errorTimer-=Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && Whole.errorTimer<=0f)  // 假设按空格键进行判定
        {
            CheckHits();
        }
    }

    void CheckHits()
    {
        // 检查当前音符列表是否为空
        if (currentNotes.Count == 0)
        {
            Debug.Log("没有需要检查的音符。");
            return;
        }

        for (int i = 0; i < Whole.Notes.Count; i++)
        {
            Note note = Whole.Notes[0];
            if (note == null)
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, note.transform.position);
            distance -= radius;
            if (distance < 0)
            {
                distance = -distance;
            }
            // 如果距离在完美范围内
            if (distance <= perfectThreshold)
            {
                if (note.noteType==NoteType.ERROR)
                {
                    Whole.errorTimer = 1.6f;
                    Whole.NoteExtent.Add("error");
                }
                else
                {
                    Whole.NoteExtent.Add("perfect");
                    if (Whole.Batter < 3)
                    {
                        if (Whole.toThree)
                        {
                            Whole.Batter = 3;
                        }
                        else
                        {
                            Whole.Batter += 1;
                            if (!Whole.toThree && Whole.Batter == 3)
                            {
                                Whole.toThree = true;
                            }
                        }
                    }
                    Whole.Notes.Remove(note);
                }
            }
            // 如果距离在良好范围内
            else if (distance <= goodThreshold)
            {
                if (note.noteType == NoteType.ERROR)
                {
                    Whole.errorTimer = 1.6f;
                    Whole.NoteExtent.Add("miss");
                }
                else
                {
                    Whole.NoteExtent.Add("good");
                    if (Whole.Batter>0)
                    {
                        Whole.Batter -= 1;
                    }
                    Whole.Notes.Remove(note);
                }

            }
            else
            {
                if (note.noteType == NoteType.ERROR)
                {
                    Whole.NoteExtent.Add(Whole.NoteExtent[Whole.NoteExtent.Count-1]);
                }
                else
                {
                    Whole.Batter = 0;
                    Whole.NoteExtent.Add("miss");
                    Whole.Notes.Remove(note);
                }
            }
            break;
        }


        for (int i = currentNotes.Count - 1; i >= 0; i--)
        {
            Note note = currentNotes[i];
            if (note!=null)
            {
                Destroy(note.gameObject);
            }
            else
            {
                break;
            }
        }

        //清空列表
        currentNotes.Clear();
    }


    // 也可以使用此方法来添加一个音符
    public void AddCurrentNote(Note note)
    {
        currentNotes.Add(note);
    }

    // 移除一个音符
    public void RemoveCurrentNote(Note note)
    {
        currentNotes.Remove(note);
    }
}
