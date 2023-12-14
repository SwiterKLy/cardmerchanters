using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data.SqlClient;
public class UpdateScore : MonoBehaviour
{
    public Text RecordText;
    public Image Avatar;
    public Sprite img1;
    public Sprite img2;
    public Sprite img3;

    // Connection string для підключення до бази даних
    private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CardMerchanters;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    void Start()
    {
        RecordText.text = PlayerPrefs.GetInt("My").ToString();
        if (PlayerPrefs.GetInt("Img") == 1)
        {
            Avatar.sprite = img1;
        }
        else if (PlayerPrefs.GetInt("Img") == 2)
        {
            Avatar.sprite = img2;
        }
        else if (PlayerPrefs.GetInt("Img") == 3)
        {
            Avatar.sprite = img3;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Отримання значень для збереження в базі даних
            int score = PlayerPrefs.GetInt("My");
            int imgIndex = PlayerPrefs.GetInt("Img");

            // Збереження даних у базі даних
            SaveMatchData(score, imgIndex);

            // Очищення значень в PlayerPrefs
            PlayerPrefs.DeleteKey("My");
            RecordText.text = PlayerPrefs.GetInt("My").ToString();
            Avatar.sprite = img1;
        }
    }

    private void SaveMatchData(int playerID, int score)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // SQL-запит для вставки даних у таблицю Games
            string query = "INSERT INTO Games (PlayerID, Score, GameDateTime) VALUES (@PlayerID, @Score, @GameDateTime)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Параметри для запиту
                command.Parameters.AddWithValue("@PlayerID", playerID);
                command.Parameters.AddWithValue("@Score", score);
                command.Parameters.AddWithValue("@GameDateTime", System.DateTime.Now);

                // Виконання запиту
                command.ExecuteNonQuery();
            }
        }
    }
}
