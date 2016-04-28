using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using MySqlConnector;
using System.Configuration;

namespace KundeKartotekJA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Kunde> ListData = new List<Kunde>();
        private static DatabaseConnector dbConnector;

        private string server = "52.58.99.157";
        private string database = "kundekartotekja";
        private string uid = "KKAdmin";
        private string password = "Awesome.";
        public MainWindow()
        {
            InitializeComponent();
            dbConnector = new DatabaseConnector(server, database, uid, password);
            listCreate();
        }

        // \n = newline \t = tab

        //open connection
        private void button_Click(object sender, RoutedEventArgs e)
        {

            bool isOpen = dbConnector.OpenConnection();

            if (isOpen)
            {
                string TextBoxMessage;
                TextBoxMessage = "Forbinder til databasen.";
                setTextBoxValue(TextBoxMessage);
                Submit_SQL();
                dbConnector.CloseConnection();

                TextBoxMessage = "Alle data korrekt sat ind i databasen, lukker forbindelsen nu.";
                setTextBoxValue(TextBoxMessage);

            }
            else
            {
                string TextBoxMessage = "Der kan ikke forbindes til databasen.";
                setTextBoxValue(TextBoxMessage);
            }
        }


        private void Submit_SQL()
        {

            string KundeNavn = textBoxNavn.Text;
            string KundeFirma = textBoxFirma.Text;
            string KundeTLF = textBoxTLF.Text;
            string KundeEmail = textBoxEmail.Text;
            

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbConnector.GetConnection();

            //' bruges ved strings.
            string cmdText = "INSERT INTO kunder (KundeNavn, KundeFirma, KundeKontaktTLF, KundeEmail) VALUES (" + "\'" + KundeNavn + "\', \'" + KundeFirma + "\', \'" + KundeTLF + "\', \'" + KundeEmail + "\');";
            cmd.CommandText = cmdText;
            MessageBox.Show("Kundens Navn: " + KundeNavn + "\t" + " Firmaets Navn: " + KundeFirma + "\t" + " Firma TLF: " + KundeTLF + "\t" + " Kundens Email: " + KundeEmail + "\t"
            + "\r\n" + cmdText);
            cmd.ExecuteNonQuery();
            setMessageBoxMainWindow("");

        }

        private void setMessageBoxMainWindow(string TextBoxMessage)
        {
            textBoxNavn.Text = TextBoxMessage;
            textBoxFirma.Text = TextBoxMessage;
            textBoxTLF.Text = TextBoxMessage;
            textBoxEmail.Text = TextBoxMessage;
        }

        private void Get_SQL()
        {
            dbConnector.OpenConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbConnector.GetConnection();
            string WhereInput = "";

            string cmdText = "SELECT * FROM kunder WHERE " + WhereInput;
            cmd.ExecuteReader();
            dbConnector.CloseConnection();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void setTextBoxValue(string TextBoxMessage)
        {
            outputBlock.Text = TextBoxMessage;
        }

        private void setResultOutputBoxValue(string TextBoxMessage)
        {
            resultOutputBox.Text = TextBoxMessage;
        }

        private void setResultBoxValue(string row1, string row2, string row3, string row4, string row0)
        {
            textBlock_KundeNavnOutput.Text = row1;
            textBlock_KundeFirmaOutput.Text = row2;
            textBlock_KundeTLFOutput.Text = row3;
            textBlock_KundeEmailOutput.Text = row4;
            textBlock_KundeNummerOutput.Text = row0;
        }

        

        private void button_RedigerKunde(object sender, RoutedEventArgs e)
        {
            string KundeID = textBlock_KundeNummerOutput.Text;

            UpdateSQL(KundeID);

        }

        private void button_SletKunde(object sender, RoutedEventArgs e)
        {
            string KundeID = textBlock_KundeNummerOutput.Text;

            DeleteSQL(KundeID);
        }

        private void UpdateSQL(string ID)
        {
            dbConnector.OpenConnection();
            string KundeNavn = textBlock_KundeNavnOutput.Text;
            string KundeFirma = textBlock_KundeFirmaOutput.Text;
            string KundeTLF = textBlock_KundeTLFOutput.Text;
            string KundeEmail = textBlock_KundeEmailOutput.Text;

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbConnector.GetConnection();

            string cmdText = "UPDATE kunder SET KundeNavn = \'" + KundeNavn + "\', KundeFirma = \'" + KundeFirma + "\', KundeKontaktTLF = \'" + KundeTLF + "\', KundeEmail = \'" + KundeEmail + "\'WHERE KundeID = \'" + ID + "\'";
            cmd.CommandText = cmdText;
            cmd.ExecuteNonQuery();
            string resultSuccess = "Opdaterede kunde med kunde ID: " + ID;
            setResultOutputBoxValue(resultSuccess);
            dbConnector.CloseConnection();

        }

        private void DeleteSQL(string ID)
        {
            dbConnector.OpenConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbConnector.GetConnection();

            if (MessageBox.Show("Slet Alle data vedrørende kunde med ID: " + ID + "?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                string cmdText = "DELETE FROM kunder WHERE KundeID = \'" + ID + "\'";
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                string resultDelete = "Slettede alle data med kunde ID: " + ID;

                setResultBoxValue("", "", "", "", "");

            }

            dbConnector.CloseConnection();
        }

        private void comboBox_SelectKunde(object sender, SelectionChangedEventArgs e)
        {
            Kunde k = (Kunde)alleKunderBox.SelectedItem;

            if (alleKunderBox.SelectedItem == null)
            {
                SelectKunde(-1);
            }
            else
            {
                SelectKunde(k.KundeID);
            }
        }

        private void opdaterCombo(object sender, RoutedEventArgs e)
        {
            updateCombo();

            //listRefresh();
            listCreate();
        }

        private void listCreate()
        {
            if(ListData.Count() == 0)
            {
                dbConnector.OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbConnector.GetConnection();

                string cmdText = "SELECT * FROM kunder";
                cmd.CommandText = cmdText;
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    ListData.Add(new Kunde { KundeID = dataReader.GetInt32(0), KundeNavn = dataReader.GetString(1) });
                    alleKunderBox.ItemsSource = ListData;
                    alleKunderBox.DisplayMemberPath = "KundeNavn";
                    alleKunderBox.SelectedValuePath = "KundeID";
                    alleKunderBox.SelectedValue = "2";
                }

                alleKunderBox.DataContext = ListData;
                dbConnector.CloseConnection();
            }
        }

        private void listRefresh()
        {
            if(ListData.Count > 0)
            {
                ListData.Clear();

                alleKunderBox.ItemsSource = ListData;
                alleKunderBox.DisplayMemberPath = "KundeNavn";
                alleKunderBox.SelectedValuePath = "KundeID";
                alleKunderBox.SelectedValue = "2";

                alleKunderBox.ItemsSource = ListData;
                alleKunderBox.DataContext = ListData;

            }
            
        }

        void updateCombo()
        {

            alleKunderBox.SelectedIndex = -1;
            alleKunderBox.SelectedValue = -1;

        }

        private void SelectKunde(int KundeID)
        {
                dbConnector.OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbConnector.GetConnection();

                string cmdText = "SELECT * FROM kunder Where KundeID = " + KundeID;
                cmd.CommandText = cmdText;
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string row0 = dataReader.GetString(0);
                    string row1 = dataReader.GetString(1);
                    string row2 = dataReader.GetString(2);
                    string row3 = dataReader.GetString(3);
                    string row4 = dataReader.GetString(4);
                    setResultBoxValue(row1, row2, row3, row4, row0);
                }
                dbConnector.CloseConnection();
            }
        }
    }