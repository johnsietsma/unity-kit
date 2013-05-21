using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public class TestExposeEnumerable : MonoBehaviour {
    [ExposeEnumerable]
    public SerializedList stringTest = new SerializedList() { "one", "two" };
}

[Serializable]
public class SerializedList : List<string> {}
