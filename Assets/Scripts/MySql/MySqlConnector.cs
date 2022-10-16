using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using static UnityEditor.Progress;

public class MySqlConnector : MonoBehaviour
{
    public MySqlConnection _connector;
    private string _temp;

    private void Awake() => IsSQLConnection();

    private void OnApplicationQuit()
    {
        _connector.Close();
        Debug.Log("����������� � �� ������� �������!");
    } 

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
        string sql = "SELECT * FROM `users` LIMIT 5";
        MySqlCommand cmd = new(sql, _connector);
        MySqlDataReader rdr = cmd.ExecuteReader();

        List<string> data = new();

        for (int i = 0; rdr.Read(); i++)
            data.Add(rdr.GetString(i));

        rdr.Close();

        return data;
    }

    public void InsertUserData(string username)
    {
        if (IsUserCreated(username)) return;

        var macAdress = SelectLocalMacAdress();

        if (macAdress == null)
        {
            Debug.LogError("��� ����� �� �������");
            return;
        }

        string sql = $"INSERT INTO `users` (`username`, `mac_address`) VALUES ('{username}', '{macAdress}');";
        MySqlCommand cmd = new(sql, _connector);
        cmd.ExecuteNonQuery();
    }

    public bool IsMacAdress(string username, string macAddress)
    {
        if (!IsUserCreated(username)) return true;

        string sql = $"SELECT COUNT(1) FROM users WHERE username = '{username}' AND mac_address = '{macAddress}'";
        MySqlCommand cmd = new(sql, _connector);
        MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read() && rdr.GetInt32(0) != 0)
        {
            rdr.Close();
            return true;
        }

        rdr.Close();

        return false;
    }

    public bool IsUserCreated(string username)
    {
        string sql = $"SELECT COUNT(1) FROM users WHERE username = '{username}'";
        MySqlCommand cmd = new(sql, _connector);
        MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read() && rdr.GetInt32(0) != 0)
        {
            rdr.Close();
            return true;
        }

        rdr.Close();


        return false;
    }

    private bool IsSQLConnection()
    {
        try
        {
            if (_temp == "Open") return true;

            _connector = GetDBConnection();
            _connector.Open();

            _temp = _connector.State.ToString();

            Debug.Log("���� ����������� � ���� ������!");
            
            return true;
        }
        catch (Exception)
        {
            Debug.LogError("��� ����������� � ���� ������!");
            _connector = null;
            return false;
        }
    }

    public string SelectLocalMacAdress()
    {
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in nics)
        {
            PhysicalAddress address = adapter.GetPhysicalAddress();
            if (address.ToString() != "") return address.ToString();
        }

        return null;
    }

}
