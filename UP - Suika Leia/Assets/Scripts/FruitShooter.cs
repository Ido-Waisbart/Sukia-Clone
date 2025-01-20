using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitShooter : MonoBehaviour
{
    [SerializeField] protected FruitData[] fruitPool;
    [SerializeField] protected SpriteRenderer fruitMainSprite;
    [SerializeField] protected SpriteRenderer fruitNextSprite;
    [SerializeField] protected GameObject fruitPrefab;  // ASSUMPTION: This has Fruit.cs.
    [SerializeField] protected Transform fruitGroup;
    [SerializeField] protected float clampDistanceFromCenter = 1;
    [SerializeField] protected float shootCooldownInSeconds = 0.25f;
    private float lastShootTime = -50;
    private FruitData fruitMain;
    private FruitData fruitNext;

    private bool IsOnCooldown => Time.time < lastShootTime + shootCooldownInSeconds;  // Waits for [CONDITION].

    void Start()
    {
        fruitMain = ChooseRandomFruit();
        fruitMainSprite.sprite = fruitMain.Sprite;
        fruitMainSprite.transform.localScale = Vector3.one * fruitMain.Radius;
        fruitNext = ChooseRandomFruit();
        fruitNextSprite.sprite = fruitNext.Sprite;
        fruitNextSprite.transform.localPosition = new Vector3(1, -1, 0) * fruitMain.Radius + new Vector3(0.25f, -0.25f, 0);
    }

    void Update()
    {
        if(!SuikaGameManager.Instance.IsGameOngoing) return;

        // Ready to shoot.
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float fruitRadius = fruitMain.Radius;
        float clampedMouseX = Mathf.Clamp(mousePosWorld.x, -clampDistanceFromCenter + fruitRadius, clampDistanceFromCenter - fruitRadius);
        //float clampedMouseX = mousePosWorld.x;
        transform.position = new Vector3(clampedMouseX, transform.position.y, transform.position.z);

        // TODO OPTIONAL: Actual touch input support.
        //if(Input.GetButton("Fire1"))
        if(Input.GetMouseButtonDown(0) && !IsOnCooldown)
        {
            OnShootCircle();
        }
    }

    FruitData ChooseRandomFruit(){
        return fruitPool[UnityEngine.Random.Range(0, fruitPool.Length)];
    }

    public void OnShootCircle(){
        lastShootTime = Time.time;
        SuikaGameManager.Instance.scoreManager.ResetCombo();

        GameObject fruitGO = Instantiate(fruitPrefab, transform.position, Quaternion.identity, fruitGroup);
        Fruit fruit = fruitGO.GetComponent<Fruit>();
        if(fruit == null)
            throw new Exception("Shot fruit must have Fruit.cs.");
        fruit.Initialize(fruitMain);

        fruitMain = fruitNext;
        fruitMainSprite.sprite = fruitMain.Sprite;
        fruitMainSprite.transform.localScale = Vector3.one * fruitMain.Radius;
        fruitNext = ChooseRandomFruit();
        fruitNextSprite.sprite = fruitNext.Sprite;
    }
}
