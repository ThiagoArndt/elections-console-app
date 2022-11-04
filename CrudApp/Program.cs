using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace ConsoleAppCrud
{
    class Program
    {
            
          
        static void Main(string[] args)
        {
            //Insert your server name and your database name here;
            string serverName = ".";
            string databaseName = "dbCRUD";
            
            var sqlConnection = sqlConnectionFunction(serverName, databaseName);
            sqlConnection.Open();

            try
            {
                bool isAnswer;
                do
                {
                    var selectedOption = SelectOptionsFunction();

                    switch (selectedOption)
                    {
                        case "Create":
                            //Create
                            CreateFunction(sqlConnection);
                            break;
                        case "Retrive":
                            //Retrieve
                            RetriveFunction(sqlConnection);
                            break;
                        case "Update":

                            //update
                            UpdateFunction(sqlConnection);
                            break;
                        case "Delete":
                            //delete
                            DeleteFunction(sqlConnection);
                            break;
                        case "Exit":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine(":P");
                            break;
                    }

                    isAnswer = AnsiConsole.Prompt(
                        new SelectionPrompt<bool> { Converter = value => value ? "Yes" : "No" }
                            .Title("[yellow3_1]Do you want to continue?[/]")
                            .AddChoices(true, false)
                     );


                    Console.Clear();
                } while (isAnswer);

            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);

            }
            finally
            {
                sqlConnection.Close();
            }
        }
        //Conexão com a Database
        public static SqlConnection sqlConnectionFunction(string sv, string db)
        {
            SqlConnection sqlConnection;
            String connectionString = @"Data Source="+sv+";Initial Catalog="+db+";Integrated Security=True";
            return new SqlConnection(connectionString);

        }

        public static string SelectOptionsFunction()
        {


            AnsiConsole.Write(new FigletText("Ballot Box").Centered().Color(Color.Green));
            var fruit = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
                .Title("Please select [blue]one[/] of the [green]options[/]")
                .AddChoices(new[] {
                     "Create", "Retrive", "Update", "Delete", "Exit"
         }));
            return fruit;
        }

        //====================================================================//


        //Método de Create
        public static void CreateFunction(SqlConnection sqlConnection)
        {
            bool restart = false;
            do
            {
                try
                {
                    //Name
                    var questionName = new Rule("[yellow3_1]What's your name?[/]");
                    questionName.Alignment = Justify.Left;
                    AnsiConsole.Write(questionName);

                    var name = AnsiConsole.Ask<string>("[green]>[/]");

                    //Age
                    var questionAge = new Rule("[yellow3_1]How old are you?[/]");
                    questionAge.Alignment = Justify.Left;
                    AnsiConsole.Write(questionAge);

                    var age = AnsiConsole.Ask<int>("[green]>[/]");


                    //Phone
                    var questionPhone = new Rule("[yellow3_1]What's your phone?[/]");
                    questionPhone.Alignment = Justify.Left;
                    AnsiConsole.Write(questionPhone);

                    var phone = AnsiConsole.Ask<int>("[green]>[/]");

                    //CPF
                    var questionCPF = new Rule("[yellow3_1]What's your CPF?[/]");
                    questionCPF.Alignment = Justify.Left;
                    AnsiConsole.Write(questionCPF);

                    var cpf = AnsiConsole.Ask<string>("[green]>[/]");

                    //Voto
                    var questionVote = new Rule("[yellow3_1]Who will you vote for?[/]");
                    questionVote.Alignment = Justify.Left;
                    AnsiConsole.Write(questionVote);

                    var vote = AnsiConsole.Ask<string>("[green]>[/]");


                    String insertQuery = "INSERT INTO Elections (user_name, user_age, user_tel, user_cpf, user_voto) VALUES('" + name + "'," + age + "," + phone + ",'" + cpf + "','" + vote + "')";


                    SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                    insertCommand.ExecuteNonQuery();

                    // THIS IS JUST VISUAL!!!
                    AnsiConsole.Status()
                    .Start("Loading...", ctx =>
                    {
                        // 1 -
                        AnsiConsole.MarkupLine("\n[bold yellow1]Encrypting your data...[/] ");
                        Thread.Sleep(6000);
                        ClearLine();
                        AnsiConsole.MarkupLine("[bold springgreen2_1]Data Succesfully Encrypted ---------- 100%[/] ");

                        // 2 - 
                        AnsiConsole.MarkupLine("[bold yellow1]Inserting your data...[/] ");
                        Thread.Sleep(6000);
                        ClearLine();
                        AnsiConsole.MarkupLine("[bold springgreen2_1]Data Succesfully Inserted  ---------- 100%[/] ");

                    });


                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine("\n[bold red]Something went wrong...[/] ");

                    Thread.Sleep(5000);
                    Console.Clear();
                    restart = true;
                }
            } while (restart);
        }




        // =================================================================================




        //Clear Console Last Line  
        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            AnsiConsole.MarkupLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }




        // =================================================================================




        //Método Retrive
        public static void RetriveFunction(SqlConnection sqlConnection)
        {
            String displayQuery = "SELECT * FROM Elections";
            SqlCommand viewCommand = new SqlCommand(displayQuery, sqlConnection);
            SqlDataReader dataReader = viewCommand.ExecuteReader();

            var table = new Table();
            table.Border = TableBorder.MinimalHeavyHead;
            table.BorderColor(Color.Yellow3_1);
            table.AddColumn(new TableColumn("Id").Centered());
            table.AddColumn(new TableColumn("[bold]Nome[/]").Centered());
            table.AddColumn(new TableColumn("Idade").Centered());
            table.AddColumn(new TableColumn("Telefone").Centered());
            table.AddColumn(new TableColumn("CPF").Centered());
            table.AddColumn(new TableColumn("Voto").Centered());

            while (dataReader.Read())
            {

                table.AddRow
                    (
                    "[bold springgreen1]" + dataReader.GetValue(0).ToString() + "[/]",
                    dataReader.GetValue(1).ToString(),
                    dataReader.GetValue(2).ToString(),
                    dataReader.GetValue(3).ToString(),
                    dataReader.GetValue(4).ToString(),
                    dataReader.GetValue(5).ToString()
                    );
            }
            AnsiConsole.Write(table);
            dataReader.Close();





        }


        // =================================================================================


        //Método Update
        public static void UpdateFunction(SqlConnection sqlConnection)
        {
            bool restart = false;
            do
            {
                try
                {

                    //Id
                    var questionId = new Rule("[yellow3_1]Insert voter ID:[/]");
                    questionId.Alignment = Justify.Left;
                    AnsiConsole.Write(questionId);

                    var id = AnsiConsole.Ask<int>("[green]>[/]");

                    //Name
                    var questionName = new Rule("[yellow3_1]Insert new voter name:[/]");
                    questionName.Alignment = Justify.Left;
                    AnsiConsole.Write(questionName);

                    var name = AnsiConsole.Ask<string>("[green]>[/]");

                    //Age
                    var questionAge = new Rule("[yellow3_1]Insert new voter age:[/]");
                    questionAge.Alignment = Justify.Left;
                    AnsiConsole.Write(questionAge);

                    var age = AnsiConsole.Ask<int>("[green]>[/]");


                    //Phone
                    var questionPhone = new Rule("[yellow3_1]Insert new voter phone:[/]");
                    questionPhone.Alignment = Justify.Left;
                    AnsiConsole.Write(questionPhone);

                    var phone = AnsiConsole.Ask<int>("[green]>[/]");

                    //CPF
                    var questionCPF = new Rule("[yellow3_1]Insert new voter CPF:[/]");
                    questionCPF.Alignment = Justify.Left;
                    AnsiConsole.Write(questionCPF);

                    var cpf = AnsiConsole.Ask<string>("[green]>[/]");

                    //Voto
                    var questionVote = new Rule("[yellow3_1]Insert new voter president:[/]");
                    questionVote.Alignment = Justify.Left;
                    AnsiConsole.Write(questionVote);

                    var vote = AnsiConsole.Ask<string>("[green]>[/]");



                    var updateQuery = "UPDATE Elections SET user_name = '" + name + "', user_age=" + age + ", user_tel = " + phone + ", user_cpf = '" + cpf + "', user_voto = '" + vote + "' WHERE user_id = " + id + "";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection);
                    updateCommand.ExecuteNonQuery();


                    // THIS IS JUST VISUAL!!!
                    AnsiConsole.Status()
                    .Start("Loading...", ctx =>
                    {
                        // 1 -
                        AnsiConsole.MarkupLine("\n[bold yellow1]Updating your data...[/] ");
                        Thread.Sleep(6000);
                        ClearLine();
                        AnsiConsole.MarkupLine("[bold springgreen2_1]Data Succesfully Updated ---------- 100%[/] ");

                    });
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine("\n[bold red]Something went wrong...[/] ");
                    Thread.Sleep(5000);
                    Console.Clear();
                    restart = true;
                }
            } while (restart);

        }


        // =================================================================================


        //Método Delete
        public static void DeleteFunction(SqlConnection sqlConnection)
        {
            var questionId = new Rule("[yellow3_1]Insert voter ID:[/]");
            questionId.Alignment = Justify.Left;
            AnsiConsole.Write(questionId);

            var id = AnsiConsole.Ask<int>("[green]>[/]");

            String deleteQuery = "DELETE FROM Elections WHERE user_Id = " + id + "";

            SqlCommand deleteCommand = new SqlCommand(deleteQuery, sqlConnection);
            deleteCommand.ExecuteNonQuery();


            AnsiConsole.Status()
                   .Start("Loading...", ctx =>
                   {
                       // 1 -
                       AnsiConsole.MarkupLine("\n[bold yellow1]Deleting selected user...[/] ");
                       Thread.Sleep(6000);
                       ClearLine();
                       AnsiConsole.MarkupLine("[bold springgreen2_1]User Succesfully Deleted! ---------- 100%[/] ");
                   });
        }

        // =================================================================================



    }

}


