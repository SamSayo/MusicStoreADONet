using static System.Console;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace samSayona;

class Program
{
    SqlConnection? conn = null;
    SqlDataAdapter da = null;
    DataSet set = null;
    SqlCommandBuilder cmd = null;
    string cs = "";

    public Program()
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
        conn = new SqlConnection(ConnectionString);
    }

    static void Main()
    {
        Program program = new Program();

        int choice = -1;
        int cash = 5000;

        while (choice != 0)
        {
            Clear();
            WriteLine($"Музыкальный магазин \"samSayona\"\t\t\tДенег в кассе: {cash}");
            choice = Convert.ToInt32(ReadLine());

            switch (choice)
            {
                case 1:
                    program.InsertQuery();
                    break;

                case 2:
                    program.DeleteQuery();
                    break;

                case 3:
                    program.UpdateQuery();
                    break;

                case 4:
                    program.Sell(ref cash);
                    break;

                case 5:
                    program.WriteOff();
                    break;

                //case 6:
                //    program.Promotion();
                //    break;

                case 0: 
                    WriteLine("Пока");
                    break;

                default:
                    WriteLine("Unknown command");
                    break;
            }
        }
    }

    public void InsertQuery()
    {
        Clear();
        WriteLine();
        Write("Введите название пластинки: ");
        string name = ReadLine();
        Write("Введите жанр: ");
        string genre = ReadLine();
        Write("Введите имя группы: ");
        string artists = ReadLine();
        Write("Введите количество треков на пластинке: ");
        int tracks = Convert.ToInt32(ReadLine());
        Write("Введите год выпуска: ");
        int year = Convert.ToInt32(ReadLine());
        Write("Введите название издательства: ");
        string publish = ReadLine();
        Write("Введите себестоимость: ");
        int cost = Convert.ToInt32(ReadLine());
        Write("Введите цену: ");
        int price = Convert.ToInt32(ReadLine());

        try
        {
            conn.Open();
            string insertString = $@"insert into Musics (Name, Genre, Artists, NumTracks, Year, Publisher, CostPrice, Price) values ('{name}', '{genre}', '{artists}', {tracks}, {year}, '{publish}', {cost}, {price})";
            SqlCommand cmd = new SqlCommand(insertString, conn);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            WriteLine("Пластинка была добавлена");
            Read();
        }
    }

    public void DeleteQuery()
    {
        Clear();
        WriteLine();
        Write("Введите Id пластинки: ");
        int id = Convert.ToInt32(ReadLine());

        try
        {
            conn.Open();
            string insertString = $@"delete from Musics where {id} = id;";
            SqlCommand cmd = new SqlCommand(insertString, conn);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            WriteLine("Пластинка под номером " + id + " была удалена");
            Read();
        }
    }

    public void UpdateQuery()
    {
        Clear();
        WriteLine();
        Write("Введите Id пластинки: ");
        int id = Convert.ToInt32(ReadLine());
        WriteLine("1.Название 2.Жанр 3.Исполнителей 4.Кол-во треков 5.Год выпуска 6.Издателя 7.Себестоимость 8.Цена 9.Остаток(Не работает)");
        Write("Что какое поле нужно изменить: ");
        int choice = Convert.ToInt32(ReadLine());

        string columnNum = "";
        switch (choice)
        {
            case 1:
                columnNum = "Name";
                break;

            case 2:
                columnNum = "Genre";
                break;

            case 3:
                columnNum = "Artists";
                break;

            case 4:
                columnNum = "NumTracks";
                break;

            case 5:
                columnNum = "Year";
                break;

            case 6:
                columnNum = "Publisher";
                break;

            case 7:
                columnNum = "CostPrice";
                break;

            case 8:
                columnNum = "Price";
                break;

            //case 9:
            //    columnNum = "Quantity";
            //    break;

            default:
                WriteLine();
                WriteLine("Столбца под таким числом не существует");
                ReadLine();
                return;
        }

        WriteLine();
        Write("На что заменить: ");
        object opt = null;
        if (choice == 9 || choice == 8 || choice == 7 || choice == 5 || choice == 4)
        {
            opt = Convert.ToInt32(ReadLine());
        }
        else
        {
            opt = $@"'{ReadLine()}'";
        }

        try
        {
            conn.Open();
            string insertString = $@"update Musics set {columnNum} = {opt} where {id} = id;";
            SqlCommand cmd = new SqlCommand(insertString, conn);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            WriteLine("Пластинка под номером " + id + " была изменена");
            Read();
        }
    }

    public void Sell(ref int cash)
    {
        Clear();
        WriteLine();
        Write("Введите Id пластинки на продажу: ");
        int id = Convert.ToInt32(ReadLine());
        WriteLine();
        Write("Введите кол-во пластинок на продажу: ");
        int newQuantity = Convert.ToInt32(ReadLine());

        try
        {
            conn.Open();
            
            using (SqlCommand command = new SqlCommand(@"select Price from Musics", conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    int price = Convert.ToInt32(result);
                    Console.WriteLine($"Цена пластинки: {price}");
                    cash += price * newQuantity;
                }
            }

            string insertString = $@"update Musics set Quantity = Quantity - {newQuantity} where {id} = id;";
            SqlCommand cmd = new SqlCommand(insertString, conn);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            WriteLine("Пластинка под номером " + id + " была продана. Спасибо за покупку");
            Read();
        }
    }

    public void WriteOff()
    {
        Clear();
        WriteLine();
        Write("Введите Id пластинки: ");
        int id = Convert.ToInt32(ReadLine());

        try
        {
            conn.Open();
            string insertString = $@"delete from Musics where {id} = id;";
            SqlCommand cmd = new SqlCommand(insertString, conn);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            WriteLine("Пластинка под номером " + id + " была списана"); 
            Read();
        }
    }

    //private void Promotion()
    //{
    //    Clear();
    //    WriteLine();
    //    Write("Введите жанр пластинок: ");
    //    string genre = ReadLine();
    //    double promPrice;

    //    try
    //    {
    //        conn.Open(); 

    //        using (SqlCommand command = new SqlCommand(@"select Price from Musics", conn))
    //        {
    //            command.Parameters.AddWithValue("@Genre", genre);
    //            object result = command.ExecuteScalar();
    //            if (result != null)
    //            {
    //                int price = Convert.ToInt32(result);
    //                Console.WriteLine($"Цена пластинки: {price}");
    //                promPrice = price * 0.1;
    //            }
    //        }

    //        string insertString = $@"update Musics set Price = {(int)Math.Truncate(promPrice)} where {genre} = Genre;";
    //        SqlCommand cmd = new SqlCommand(insertString, conn);
    //        cmd.ExecuteNonQuery();
    //    }
    //    finally
    //    {
    //        if (conn != null)
    //        {
    //            conn.Close();
    //        }
    //    }
    //}

    public void Search()
    {
        Clear();
        WriteLine("По какой категории вы хотите искать пластинки?");
        WriteLine("1. Название диска 2. Исполнитель 3.Жанр");
        Write("Выбор: "); 
        int choice = Convert.ToInt32(ReadLine());
        Write("Что искать: ");
        string ans = ReadLine();
        string[] insertString = { 
            $"select * from Musics where Name = '{ans}'",
            $"select * from Musics where Artists = '{ans}'",
            $"select * from Musics where Genre = '{ans}'"
        };
        

        try
        {
            conn.Open();
            SqlCommand cmd = null; 
            if (choice == 1)
            {
                cmd = new SqlCommand(insertString[0], conn);
            }
            else if (choice == 2)
            {
                cmd = new SqlCommand(insertString[1], conn);
            }
            else if (choice == 3)
            {
                cmd = new SqlCommand(insertString[2], conn);
            }

            if (cmd != null)
            {
                cmd.ExecuteNonQuery();
            }
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            
            Read();
        }
    }
}