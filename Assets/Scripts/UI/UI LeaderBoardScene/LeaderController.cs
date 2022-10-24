using System.Collections.Generic;
using UnityEngine;

public class LeaderController : MonoBehaviour
{
    [SerializeField] private Transform[] _gridsTransform;
    private MySqlConnector _connector;
    private List<List<string>> dataUsers = new();

    private void Start()
    {
        _connector = GetComponent<MySqlConnector>();

        dataUsers = SelectUserData();

        if (dataUsers == null) return;

        InsertDataUserInGrid();
    }


    private void InsertDataUserInGrid()
    {
        int gridIndex = 0;

        foreach (var grid in _gridsTransform)
        {
            if (gridIndex >= dataUsers.Count) return;

            for (int b = 0; b < dataUsers[gridIndex].Count; b++)
            {
                if (b + 1 > grid.childCount) return;

                var tmp_component = grid.GetChild(b + 1).GetComponentInChildren<TMPro.TMP_Text>();
                tmp_component.text = dataUsers[gridIndex][b];
            }

            gridIndex++;
        }
    }

    private List<List<string>> SelectUserData()
    {
        var data = _connector.SelectUsersLeaders();

        if (data.Count == 0) return null;

        var tempList = new List<string>();

        List<List<string>> dataUser = new();

        int i = 0;

        do
        {
            tempList.Add(data[i]);

            if (tempList.Count == 4)
            {
                dataUser.Add(new List<string>(tempList));
                tempList.Clear();
            }

            i++;
        } while (i < data.Count);

        return dataUser;
    }



}
