using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

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

    private void InsertUserData(string username)
    {
        if (!IsSQLConnection()) return;

        string macAdress = SelectLocalMacAdress();

        if (macAdress == null)
        {
            Debug.LogError("Мак Адрес не получен");
            return;
        }

        string sql = $"INSERT INTO `users` (`username`, `mac-address`) VALUES ({username}, {macAdress});";
        MySqlCommand cmd = new(sql, _connector);
        cmd.ExecuteNonQuery();
        _connector.Close();
    }

    private string SelectLocalMacAdress()
    {
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in nics)
        {
            PhysicalAddress address = adapter.GetPhysicalAddress();
            if (address.ToString() != "") return address.ToString();
        }

        return null;
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
