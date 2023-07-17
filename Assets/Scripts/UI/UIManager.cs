using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InitialPosition initialPosition;
    [SerializeField] private Window_Graph windowGraph;

    [Header ("CANVAS")]
    [SerializeField] private GameObject graphicCanvas;
    [SerializeField] private GameObject screenCanvas;

    [Header("PANELS")]
    [SerializeField] private GameObject menuButtons;

    [Header("BUTTONS")]
    [SerializeField] private Button enabledGraphicBtn;
    [SerializeField] private Button disabledGraphicBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button backLoginBtn;
    [SerializeField] private Button hideMenuBtn;
    [SerializeField] private Button unhideMenuBtn;

    [Header("OBJECTS")]
    [SerializeField] private GameObject guyWireSuppported_1;
    [SerializeField] private GameObject guyWireSuppported_2;

    [Header("INSTRUCTIONS")]
    [SerializeField] private GameObject panelPosition;
    [SerializeField] private GameObject panelRotation;
    [SerializeField] private GameObject panelScale;
    [SerializeField] private Button instructionPosition;
    [SerializeField] private Button instructionRotation;
    [SerializeField] private Button instructionScale;

    [Header("SOUNDS")]
    [SerializeField] private AudioSource buttonClick;

    // Start is called before the first frame update
    void Start()
    {
        buttonClick.Pause();
        enabledGraphicBtn.onClick.AddListener(ActivateGraph);
        disabledGraphicBtn.onClick.AddListener(DisableGraph);
        restartBtn.onClick.AddListener(RestartScene);
        backLoginBtn.onClick.AddListener(BackToLogin);
        hideMenuBtn.onClick.AddListener(HideMenu);
        unhideMenuBtn.onClick.AddListener(UnHideMenu);
        instructionPosition.onClick.AddListener(ActionInstructionPosition);
        instructionRotation.onClick.AddListener(ActionInstructionRotation);
        instructionScale.onClick.AddListener(ActionInstructionScale);

        StartCoroutine(InitialHideMenu());
    }

    private void Update()
    {
        
    }

    private void ActivateGraph() 
    {
        buttonClick.Play();
        graphicCanvas.SetActive(true);
        windowGraph.GetComponent<Window_Graph>().ActiveGraph();
        disabledGraphicBtn.gameObject.SetActive(true);
        enabledGraphicBtn.gameObject.SetActive(false);
        guyWireSuppported_2.SetActive(true);
        guyWireSuppported_1.SetActive(false);
    }

    private void DisableGraph()
    {
        buttonClick.Play();
        windowGraph.GetComponent<Window_Graph>().RestartGraph();
        graphicCanvas.SetActive(false);
        disabledGraphicBtn.gameObject.SetActive(false);
        enabledGraphicBtn.gameObject.SetActive(true);
        guyWireSuppported_2.SetActive(false);
        guyWireSuppported_1.SetActive(true);
    }

    private void RestartScene() 
    {
        //buttonClick.Play();
        initialPosition.GetComponent<InitialPosition>().RestartObjectsPosition();
        windowGraph.GetComponent<Window_Graph>().RestartGraph();
        buttonClick.Play();
        disabledGraphicBtn.gameObject.SetActive(false);
        enabledGraphicBtn.gameObject.SetActive(true);
        guyWireSuppported_2.SetActive(false);
        guyWireSuppported_1.SetActive(true);
        graphicCanvas.SetActive(false);
        //SceneManager.LoadScene("SceneExperience");
    }

    private void BackToLogin() 
    {
        SceneManager.LoadScene("LoginRegister");
    }

    private void HideMenu() 
    {
        buttonClick.Play();
        menuButtons.transform.LeanMoveLocalX(-596.22f, 1f);
        hideMenuBtn.gameObject.transform.LeanMoveLocalX(-487f, 1);
        unhideMenuBtn.gameObject.transform.LeanMoveLocalX(-487f, 1);
        hideMenuBtn.gameObject.SetActive(false);
        unhideMenuBtn.gameObject.SetActive(true);
    }

    private void UnHideMenu() 
    {
        menuButtons.transform.LeanMoveLocalX(-471.64f, 1f);
        unhideMenuBtn.gameObject.transform.LeanMoveLocalX(-374f, 1);
        hideMenuBtn.gameObject.transform.LeanMoveLocalX(-374f, 1);
        hideMenuBtn.gameObject.SetActive(true);
        unhideMenuBtn.gameObject.SetActive(false);
    }

    IEnumerator InitialHideMenu() 
    {
        yield return new WaitForSeconds(1.5f);
        menuButtons.transform.LeanMoveLocalX(-596.22f, 1f);
        hideMenuBtn.gameObject.transform.LeanMoveLocalX(-487f, 1);
        unhideMenuBtn.gameObject.transform.LeanMoveLocalX(-487f, 1);
        hideMenuBtn.gameObject.SetActive(false);
        unhideMenuBtn.gameObject.SetActive(true);
    }

    private void ActionInstructionPosition() 
    {
        buttonClick.Play();
        panelPosition.SetActive(false);
        panelRotation.SetActive(true);
    }

    private void ActionInstructionRotation()
    {
        buttonClick.Play();
        panelRotation.SetActive(false);
        panelScale.SetActive(true);
    }

    private void ActionInstructionScale()
    {
        buttonClick.Play();
        panelScale.SetActive(false);
        //panelScale.SetActive(false);
    }
}
