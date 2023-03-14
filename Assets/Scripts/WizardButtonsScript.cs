using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardButtonsScript : MonoBehaviour
{
    public Button fireBoltButton;
    public Button rayOfFrostButton;
    public Button magicMissileButton;
    public Button scorchingRayButton;

    // Start is called before the first frame update
    void Start()
    {
        fireBoltButton.GetComponent<Button>().onClick.AddListener(FBTaskOnClick);
        rayOfFrostButton.GetComponent<Button>().onClick.AddListener(ROFTaskOnClick);
        magicMissileButton.GetComponent<Button>().onClick.AddListener(MMTaskOnClick);
        scorchingRayButton.GetComponent<Button>().onClick.AddListener(SRTaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FBTaskOnClick()
    {
        WizardUnit.getInstance().FireBolt();
        GameControllerScript.getInstance().wizardParentButton.SetActive(false);
    }

    void ROFTaskOnClick()
    {
        WizardUnit.getInstance().RayOfFrost();
        GameControllerScript.getInstance().wizardParentButton.SetActive(false);
    }

    void MMTaskOnClick()
    {
        GameControllerScript.getInstance().wizardParentButton.SetActive(false);
    }

    void SRTaskOnClick()
    {
        GameControllerScript.getInstance().wizardParentButton.SetActive(false);
    }
}
