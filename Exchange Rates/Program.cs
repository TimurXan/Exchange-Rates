using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SqlClient;

namespace Exchange_Rates
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=DESKTOP-RI5GSL6;Initial Catalog=Exchange_Rates;Integrated Security=True";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                String URLString = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=21.08.2019";
                String date = "", name = "";
                float value = 0;
                XmlTextReader reader = new XmlTextReader(URLString);

                while (reader.Read())
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // Узел является элементом.
                            while (reader.MoveToNextAttribute()) // Выполняется чтение атрибутов.
                            {
                                if (reader.Name == "Date")
                                    date = reader.Value;
                            }
                            if (reader.Name == "Name")
                            {
                                reader.Read();
                                name = reader.Value;
                            }
                            else if (reader.Name == "Value")
                            {
                                reader.Read();
                                value = System.Convert.ToSingle(reader.Value);
                                string sqlExpression = "INSERT INTO Курс (Дата, Валюта, Курс) VALUES (@date,@name, @value)";

                                SqlCommand command = new SqlCommand(sqlExpression, connection);
                                command.Parameters.AddWithValue("@date", date);
                                command.Parameters.AddWithValue("@name", name);
                                command.Parameters.AddWithValue("@value", value);
                                int number = command.ExecuteNonQuery();
                                //Console.WriteLine("Добавлено объектов: {0}", number);
                            }
                            break;
                    }
            }
            Console.WriteLine("Подключение закрыто...");


        }
    }
}
