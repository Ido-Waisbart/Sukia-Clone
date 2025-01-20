using UnityEngine;

[CreateAssetMenu(fileName = "NewFruit", menuName = "Game/Fruit")]
public class FruitData : ScriptableObject
{
    public Sprite Sprite;
    public float Radius;
    public int BasePoints;
    public FruitData NextFruit;
}
