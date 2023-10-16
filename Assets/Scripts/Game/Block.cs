using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private float blockSize = 0.18f;

    public ParticleSystem mergeEffect1;
    public ParticleSystem mergeEffect2;

    public BlockPattern type = BlockPattern.None;

    public int X { get; set; }
    public int Y { get; set; }

    public int CurrentPattern { get; set; } = 0;
    public bool IsMerged { get; set; } = false;
    public bool IsChcekIndex { get; set; } = false;
    public bool IsContainList { get; set; } = false;

    public void SetIndex(int y, int x)
    {
        Y = y;
        X = x;
        CurrentPattern = (int)type;
    }

    protected virtual void OnEnable()
    {
        IsMerged = false;
    }
}