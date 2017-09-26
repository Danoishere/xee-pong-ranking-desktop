using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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

namespace HerbstmatchRanking
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string RESULT_FILE_PATH = "../Result.csv";

        public ObservableCollection<Participant> ParticipantsTop10 { get; set; } = new ObservableCollection<Participant>();
        public List<Participant> AllParticipants { get; set; } = new List<Participant>();
        public PointListener PointListener { get; set; } = new PointListener();


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            PointListener.OnRequestReceived += PointListener_OnRequestReceived;
            PointListener.StartListening();

            ReloadResultList();
        }

        private void PointListener_OnRequestReceived(HttpListenerContext context)
        {
            var queryStrings = context.Request.QueryString;
            var points = queryStrings["points"];
            int pointInt;

            if (!string.IsNullOrEmpty(points) && int.TryParse(points, out pointInt))
            {
                var enterMatchDetails = new EnterMatchDetails(int.Parse(points));
                enterMatchDetails.ShowDialog();
                Focus();
                File.AppendAllLines(RESULT_FILE_PATH, new string[] { enterMatchDetails.PlayerName + ";" + points + ";" + enterMatchDetails.Mail });
                ReloadResultList();
            }
        }

        private void ReloadResultList()
        {
            if (File.Exists(RESULT_FILE_PATH))
            {
                var lines = File.ReadAllLines(RESULT_FILE_PATH);
                var participants = new List<Participant>();

                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    participants.Add(new Participant
                    {
                        Name = parts[0],
                        Points = int.Parse(parts[1]),
                        Email = parts[2]
                    });
                }
                
                AllParticipants.Clear();
                AllParticipants = participants.OrderByDescending(p => p.Points).ToList();

                ParticipantsTop10.Clear();
                var top10 = AllParticipants.Take(10);
                var rankCounter = 1;
                foreach (var player in top10)
                {
                    player.Rank = rankCounter;
                    ParticipantsTop10.Add(player);
                    rankCounter++;
                }
            }
           
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape || e.Key == Key.Enter)
            { 
                Application.Current.Shutdown();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KeyDown += MainWindow_KeyDown;
        }
    }
}
