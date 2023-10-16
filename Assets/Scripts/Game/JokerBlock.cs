using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerBlock : Block
{
    bool[] isJokerMerged = new bool[4];
    List<Block> jokerInclusiveBlockList = new List<Block>();

    protected override void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < isJokerMerged.Length; i++)
        {
            isJokerMerged[i] = false;
        }

        jokerInclusiveBlockList.Clear();
    }
}
