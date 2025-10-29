using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Planet Limits per Level")]
    public int smallLimit = 3; // küçük
    public int mediumLimit = 2; // orta
    public int largeLimit = 1; // büyük
}
