using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whole : MonoBehaviour
{
    //存放场上现有的节拍
    public static List<Note> Notes = new List<Note>();
    //存放产生的所有的节拍类型
    public static List<NoteType> NoteTypes = new List<NoteType>();
    //存放产生的所有的节拍的对押程度
    public static List<string> NoteExtent = new List<string>();
    //存放连击数
    public static int Batter;

    public static bool toThree;

    public static float errorTimer;
}
