using UnityEngine;

public class GhostPlayerInputs : MonoBehaviour
{
    private LoadWizardGhost _selectionPanel;
    private MyInputManager _input;
    private int acceptIteration = 0;

    private void Awake()
    {
        _selectionPanel = FindObjectOfType<LoadWizardGhost>();
        _input = FindObjectOfType<MyInputManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (_selectionPanel && _selectionPanel.inputEnabled)
        {
            // SHow accept image when any input is detected
            if (_input.AnyInputDetected())
            {
                _selectionPanel.UpdateAcceptImage(true);
                acceptIteration++;
            }

            // Confirm
            if (_input.NavigationSelect() && acceptIteration > 1)
            {
                _selectionPanel.PlayerAccept();
            }

            //left 
            //if (_input.NavigationLeft())
            //{
            //    _selectionPanel.SelectLeft(_input);
            //}

            ////right
            //if (_input.NavigationRight())
            //{
            //    _selectionPanel.SelectRight(_input);
            //}
            //exit
        }
    }
}