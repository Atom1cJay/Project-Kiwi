using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeafFallScript : MonoBehaviour
{
    GameObject fallingLeaf = null;
    Vector3 originalPos;
    Vector3 scale;

    [SerializeField] float DeltaX, DeltaY, fallingSpeed, randomMultiplier, DeltaAngle, targetAngle, deltaAngleSpeed, speed, changeInYAngleSpeed, finishedYLocation, fadeInTime;
    [SerializeField] Image image;

    //targets with randomized
    float adjustedSpeed = 0f;
    float randomizedDeltaX = 0f;
    float randomizedDeltaY = 0f;
    float adjustedFallingSpeed = 0f;

    //current values
    float currentDeltaX = 0f;
    float currentDeltaY = 0f;
    float currentFallingSpeed = 0f;

    //misc vars
    float startTime = 0f;
    float angleY = 0f;
    float alpha = 0f;

    Color c;
    bool healed;

    void Start ()
    {
        fallingLeaf = null;
        angleY = 0f;
        originalPos = transform.position;
        c = image.color;
        scale = Vector3.one;
        //HealLeaf();
    }

    // Update is called once per frame
    void Update()
    {

        if (fallingLeaf != null)
        {

            //all maths to show floating down

            float mult = Mathf.Cos((Time.time - startTime) * adjustedSpeed);

            if (Mathf.Abs(mult) > 0.99999f)
            {
                randomizedDeltaX = DeltaX * Random.Range(1f - randomMultiplier, 1f + randomMultiplier);
                randomizedDeltaY = DeltaY * Random.Range(1f - randomMultiplier, 1f + randomMultiplier);
                adjustedFallingSpeed = fallingSpeed * Random.Range(1f - randomMultiplier, 1f + randomMultiplier);
            }

            currentDeltaX += (randomizedDeltaX - currentDeltaX) * Time.deltaTime;
            currentDeltaY += (randomizedDeltaY - currentDeltaY) * Time.deltaTime;
            currentFallingSpeed += (adjustedFallingSpeed - currentFallingSpeed) * Time.deltaTime;

            float x = mult * currentDeltaX;
            float y = (-currentFallingSpeed + ((1 - Mathf.Abs(mult)) * currentDeltaY)) * Time.deltaTime;

            fallingLeaf.transform.position = new Vector3(originalPos.x + x,  fallingLeaf.transform.position.y + y, 0f);

            float angle = fallingLeaf.transform.eulerAngles.z;


            angleY += Time.deltaTime * changeInYAngleSpeed;

            if (mult >= 0)
                fallingLeaf.transform.eulerAngles = new Vector3(0f, angleY, angle + (targetAngle + DeltaAngle - angle) * deltaAngleSpeed * Time.deltaTime);
            else
                fallingLeaf.transform.eulerAngles = new Vector3(0f, angleY, angle + (targetAngle - DeltaAngle - angle) * deltaAngleSpeed * Time.deltaTime);



            //we done?
            if (fallingLeaf.transform.localPosition.y <= finishedYLocation)
            {
                Destroy(fallingLeaf);
                fallingLeaf = null;
                alpha = 0f;
            }
        }

        //we're healed and should show this
        if (healed)
        {
            //fade back in if not done
            if (image.color.a <= 255f)
            {
                image.color = new Color(c.r, c.g, c.b, alpha);
                alpha += Time.deltaTime / fadeInTime;
            }
        }
        
    }
    
    public void HealLeaf()
    {
        image.color = new Color(c.r, c.g, c.b, 0f);
        alpha = 0f;
        healed = true;
    }

    public void DropLeaf()
    {
        //we're not healed
        healed = false;

        //make new leaf
        GameObject newLeaf = Instantiate(gameObject, transform.position, Quaternion.identity);

        //copy shiz
        newLeaf.transform.SetParent(transform.parent);
        newLeaf.transform.position = transform.position;
        newLeaf.transform.eulerAngles = transform.eulerAngles;
        newLeaf.transform.localScale = transform.localScale;
        Destroy(newLeaf.GetComponent<LeafFallScript>());

        //set up our initial vars
        fallingLeaf = newLeaf;
        adjustedSpeed = speed;
        randomizedDeltaX = DeltaX * Random.Range(1f - randomMultiplier, 1f + randomMultiplier);
        randomizedDeltaY = 0f;
        adjustedFallingSpeed = fallingSpeed / 2f;
        startTime = Time.time;
        angleY = 0f;

        //update color
        c = image.color;

        //disable image
        image.color = new Color(c.r, c.g, c.b, 0f);
    }
}
