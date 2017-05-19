using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface XIProgress
{
    float progress { get; }
    string info { get; }
    string tip { get; }
    bool isComplete { get; }
}
