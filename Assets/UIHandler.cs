using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static bool MultipleDates;
    
    public void ToConsultationsButtonClick()
    {
        GameObject.Find("potScreen").transform.localPosition = new Vector3(0, 5000, 0);
        GameObject.Find("chooseConsultationScreen").transform.localPosition = new Vector3(0, 0, 0);
    }
    
    public void ToPotScreenButtonClick()
    {
        CalculatePortrait.ActivateScreenManually();
        GameObject.Find("potScreen").transform.localPosition = new Vector3(0, 0, 0);
        GameObject.Find("chooseConsultationScreen").transform.localPosition = new Vector3(0, 5000, 0);
    }

    public void HandlePrimaryClick()
    {
        MultipleDates = false;
        ToPotScreenButtonClick();
    }

    public void HandleRelationshipClick()
    {
        MultipleDates = true;
        ToPotScreenButtonClick();
    }
}
