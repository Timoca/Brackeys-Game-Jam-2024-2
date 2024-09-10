using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectible", menuName = "Collectible")]
public class Collectible : ScriptableObject
{
    public string objectName;
    public GameObject prefab;
    public string objectTag;
    public int score;
}
