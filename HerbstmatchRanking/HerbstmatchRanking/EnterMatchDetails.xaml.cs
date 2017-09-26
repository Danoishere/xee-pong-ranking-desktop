using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace HerbstmatchRanking
{
    /// <summary>
    /// Interaktionslogik für EnterMatchDetails.xaml
    /// </summary>
    public partial class EnterMatchDetails : Window
    {
        public string PlayerName { get; set; }
        public string Mail { get; set; }

        public EnterMatchDetails(int points)
        {
            InitializeComponent();
            Punkte.Content = points + " Punkte";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerName = string.IsNullOrEmpty(TxtName.Text)? "<Kein Name>" : TxtName.Text;
            Mail = string.IsNullOrEmpty(TxtMail.Text) ? "" + TxtMail.Text : TxtMail.Text;

            Close();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Left =  ((Application.Current.MainWindow.Width / 2)- (Width/2));
            this.Top = ((Application.Current.MainWindow.Height / 2) - (Height / 2));

            TxtName.Focus();
        }
    }
}
