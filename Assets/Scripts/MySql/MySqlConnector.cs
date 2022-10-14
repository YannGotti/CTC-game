using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class MySqlConnector : MonoBehaviour
{
    private MySqlConnection _connector;

    private MySqlConnection GetDBConnection()
    {
        // Connection String.
        String connString = "Server=31.31.198.99"+ ";Database=u1625777_ctc-game"
            + ";port=3306"+ ";User Id=u1625777_ctc-use" + ";password=qB6jF9sM2v";

        MySqlConnection conn = new(connString);

        return conn;
    }

    public List<string> SelectUsersLeaders()
    {
        if (!IsSQLConnection()) return null;

        string sql = "SELECT * FROM `users` LIMIT 5";
        MySqlCommand cmd = new(sql, _connector);
        MySqlDataReader rdr = cmd.ExecuteReader();

        List<string> data = new();

        for (int i = 0; rdr.Read(); i++)
            data.Add(rdr.GetString(i));

        rdr.Close();
        _connector.Close();

        return data;
    }


    private bool IsSQLConnection()
    {
        try
        {
            _connector = GetDBConnection();
            _connector.Open();
            Debug.Log("Успешное подключение к базе данных!");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Нет подключения к базе данных!");
            Debug.LogError(e.StackTrace);
            _connector = null;
            return false;
        }
    }
    
}
