﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml;
using System.Configuration;


namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for Marks.xaml
    /// </summary>
    public class Mark
    {
        public string subject{ get; set; }
        [PrimaryKey, Unique]
        public string name { get; set; }
        public string mark { get; set; }
        public double weight { get; set; }
        public DateTime date { get; set; }
        public int year { get; set; }

        [AutoIncrement]
        int MarkID { get; set; }
        
        public int average { get; set; }
        public double percent { get; set; }

        public string comment { get; set; }
}
    [Table("Subject")]
    public class Subject
    {
        [Unique,PrimaryKey]
        public string Name { get; set; }

        public int Year { get; set; }

        public string teacher { get; set; }
        public int YourAverage { get; set; }
        public int EveryoneAverage { get; set; }
        

        public List<Mark> marks = new List<Mark>();
    }

    public partial class Marks : Page
    {

            
        public static ObservableCollection<Subject> SubjectResults = new ObservableCollection<Subject>();
        
        public Marks()
        {
            InitializeComponent();
            
            OverallRes.Visibility = Visibility.Visible;
            if (SubjectResults.Count != 0)
            {
                TitleSubject.Content = "Overall";
            }
                        
            TitleGrade.Content = "";
            
            
            SubjectList.ItemsSource = SubjectResults;
            OverallRes.ItemsSource = SubjectResults;

            

        }

        private void MyListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem) && r.VisualHit.GetType() != typeof(Button))
                SubjectList.UnselectAll();
                HoverTestInfo.Visibility = Visibility.Collapsed;
                Graph.Children.Clear();
                OverallRes.Visibility = Visibility.Visible;
                TitleSubject.Content = "Overall";
                TitleGrade.Content = "";
            

        }

        private void changed(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectList.SelectedIndex != -1)
            {
                OverallRes.Visibility = Visibility.Collapsed;
                var display = SubjectResults[SubjectList.SelectedIndex];
                TitleSubject.Content = display.Name;
                if (display.YourAverage != -1)
                    TitleGrade.Content = display.YourAverage + " %";
                else
                    TitleGrade.Content = "N/A";

                if (display.YourAverage <= 50)
                    TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(181,0,0));
                else if (display.YourAverage <= 75)
                    TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(181, Convert.ToByte((181/25)*(display.YourAverage-50)), 0));
                else
                    try
                    {
                        TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(Convert.ToByte(181 - (181 / 25) * (display.YourAverage - 75)), 181, 0));
                    }
                    catch
                    {
                        TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(0, 181, 0));
                    }
                    
                DrawGraph(display);
            }          
        }
        void DrawGraph(Subject sub)
        {
            Graph.Children.Clear();
            int wi = Convert.ToInt32( Graph.ActualWidth);
            int he = Convert.ToInt32(Graph.ActualHeight);


            if (sub.marks.Count >=2)
            {
                int incr = wi / (sub.marks.Count-1);

                PointCollection MyPoints = new PointCollection();
                PointCollection AvPoints = new PointCollection();

                var ButList = new List<Button>();

                for (var i = 0; i < sub.marks.Count; i++)
                {
                    Button a = new Button();

                    a.Style = (Style)FindResource("MarkHover");
                    a.Template = (ControlTemplate)FindResource("HMK_Dark");
                    Canvas.SetLeft(a, i * incr - 6.5);
                    Canvas.SetTop(a, he - sub.marks[i].average * he / 100 - 6.5);
                    a.MouseEnter += GradeHover;
                    a.Name = "Av" + i;
                    ButList.Add(a);

                    Button b = new Button();
                    
                    b.Style = (Style)FindResource("MarkHover");
                    b.Template = (ControlTemplate)FindResource("HMK");
                    Canvas.SetLeft(b, i * incr -6.5);
                    Canvas.SetTop(b, he - sub.marks[i].percent * he - 6.5);
                    b.MouseEnter += GradeHover;
                    b.Name = "My" + i;
                    ButList.Add(b);

                    


                    MyPoints.Add(new Point(i*incr, he - sub.marks[i].percent*he));
                    AvPoints.Add(new Point(i * incr,he- sub.marks[i].average* he/100));
                }
                Polyline MyLline = new Polyline
                {
                    StrokeThickness = 3,
                    Stroke = Brushes.Red,
                    Points = MyPoints
                };
                Polyline AvLline = new Polyline
                {
                    StrokeThickness = 3,
                    Stroke = Brushes.DarkGray,
                    Points = AvPoints
                };
                Graph.Children.Add(AvLline);
                Graph.Children.Add(MyLline);
                foreach (var i in ButList)
                {
                    Graph.Children.Add(i);
                }
                
            }

        }
        private void SizeChangedd(object sender, EventArgs e)
        {
            if (SubjectList.SelectedIndex != -1)
            {
                DrawGraph(SubjectResults[SubjectList.SelectedIndex]);
            }
        }

        private void GradeHover(object sender, MouseEventArgs e)
        {
            string name = ((Button)sender).Name;
            Subject sub = SubjectResults[SubjectList.SelectedIndex];
            var val = Convert.ToInt32(name[2]) - 48;

            
            Mark m = sub.marks[val];
            
            if (name[0] == 'M') // It is persons grade
            {
                //HoverTestInfo.Visibility = Visibility.Visible;
                
                MessageBox.Show("Name: "+ m.name +"\nMark: " + m.mark + "\nPercent: " +m.percent*100 + "\nWeight: " + m.weight + "\nDate" +m.date);
            }
            else // average grade
            {
                
                MessageBox.Show("Name: " + m.name +  "\nPercent: " + m.average + "\nWeight: " + m.weight + "\nDate" + m.date);
            }

        }
    }
}