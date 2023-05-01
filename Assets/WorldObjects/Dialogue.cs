using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [Serializable]
    public class Line
    {
        public string speaker;
        public string text;
        public Sprite speakerImg;
        public List<int> nextLine;

        public int GetNextLine()
        {
            return nextLine[UnityEngine.Random.Range(0, nextLine.Count)];
        }
    }
    public List<Line> dialogue;

    public Line GetLine(int id)
    {
        return dialogue[id];
    }

}
