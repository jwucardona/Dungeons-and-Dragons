using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClericButtonsScript : MonoBehaviour
{
    public Button healingWordButton;
    public Button massHealingWordButton;
    public Button aidButton;

    // Start is called before the first frame update
    void Start()
    {
        healingWordButton.GetComponent<Button>().onClick.AddListener(HWTaskOnClick);
        massHealingWordButton.GetComponent<Button>().onClick.AddListener(MHWTaskOnClick);
        aidButton.GetComponent<Button>().onClick.AddListener(ATaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HWTaskOnClick()
    {
        ClericUnit.getInstance().HealingWord();
        GameControllerScript.getInstance().clericParentButton.SetActive(false);
    }

    void MHWTaskOnClick()
    {
        ClericUnit.getInstance().MassHealingWord();
        GameControllerScript.getInstance().clericParentButton.SetActive(false);
    }

    void ATaskOnClick()
    {
        ClericUnit.getInstance().Aid();
        GameControllerScript.getInstance().clericParentButton.SetActive(false);
    }
}
