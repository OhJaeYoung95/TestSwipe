using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataTable
{
    protected string path = string.Empty;

    public abstract void Load();
}
