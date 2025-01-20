using System;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    private int basePoints;
    private FruitData fruitData;
    public FruitData FruitData => fruitData;

    //private bool hasCollidedBefore = false;
    private float startTimeInSeconds;  // If after this much time, the fruit still triggers the end line? Game over.
    private float endLineCooldownInSeconds = 2.0f;  // If after this much time, the fruit still triggers the end line? Game over.
    private bool endLineCooldownFinished = false;

    public void Initialize(FruitData newData)
    {
        if(newData == null) throw new Exception("Fruit data is necessary.");
        fruitData = newData;
        basePoints = newData.BasePoints;
        spriteRenderer.sprite = newData.Sprite;
        transform.localScale = Vector3.one * newData.Radius;
    }

    void Start(){
        startTimeInSeconds = Time.time;
    }

    void Update(){
        if(!endLineCooldownFinished && Time.time > startTimeInSeconds + endLineCooldownInSeconds){
            endLineCooldownFinished = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        // grant fruitData.BasePoints * comboFactor.
        // create NextFruit instance at center of these two.
        
        //hasCollidedBefore = true;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
        if(otherFruit == null)
            return;
        
        if(fruitData.name == otherFruit.FruitData.name){
            //  Both fruit will get this event.
            //  Only the lesser ID will create the new fruit.
            if(GetInstanceID() < otherFruit.gameObject.GetInstanceID()){
                SuikaGameManager.Instance.MergeFruit(this, otherFruit);
            }
            Destroy(gameObject);
        }
    }

    /*void OnTriggerEnter2D(Collider2D collider){
        if(hasCollidedBefore && collider.gameObject.CompareTag("End Line"))
        {
            print("SuikaGameManager.Instance.CrossedEndLine();");
        }
    }*/

    void OnTriggerStay2D(Collider2D collider){
        if(endLineCooldownFinished && collider.gameObject.CompareTag("End Line"))
        {
            SuikaGameManager.Instance.FruitCrossedEndLine();
        }
    }
}
