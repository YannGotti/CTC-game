using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour
{
    [SerializeField] private GameObject _helpPanel;

    public void EnablePanel() => _helpPanel.SetActive(true);
    public void DisablePanel() => _helpPanel.SetActive(false);

}
