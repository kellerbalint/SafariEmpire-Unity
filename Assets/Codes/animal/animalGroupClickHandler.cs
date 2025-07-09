using Codes.animal;
using UnityEngine;

public class AnimalGroupClickHandler : MonoBehaviour
{
    public AnimalGroup animalGroup;

    void OnMouseDown()
    {
        Debug.Log("Kattint�s az �llatcsoportra!");
        if (animalGroup != null)
        {
            // Pl. jelen�tsd meg az adatait UI-on
            Debug.Log($"�llatt�pus: {animalGroup.animalType}, Egyedek sz�ma: {animalGroup.animals.Count}");
        }
    }
}