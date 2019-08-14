﻿using System;
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
using System.Windows.Threading;

namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer seconds = new DispatcherTimer();
        DispatcherTimer milliseconds = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            seconds.Tick += new EventHandler(OneSecond);
            seconds.Interval = new TimeSpan(0, 0, 1);
            seconds.Start();

            milliseconds.Interval = TimeSpan.FromMilliseconds(1);
            milliseconds.Tick += UpdateSecondHand;
            milliseconds.Start();

            GetStudentData.Start();

        }
        void UpdateSecondHand(object sender, object e)
        {
            secondHand.Angle = (DateTime.Now.Second + (double)DateTime.Now.Millisecond / 1000)*6 + 90;
        }
        private void OneSecond(object sender, EventArgs e)
        {
            minuteHand1.Angle = (DateTime.Now.Minute + (double)DateTime.Now.Second / 60) * 6 + 90;
            Console.WriteLine(DateTime.Now.Hour + (double)DateTime.Now.Minute / 60);
            hourhand1.Angle = (DateTime.Now.Hour + (double)DateTime.Now.Minute / 60) * 30 + 90;
        }

        private void TasksClicked(object sender, RoutedEventArgs e)
        {
            ShowTasksPage();
        }

        private void WellbeingClicked(object sender, RoutedEventArgs e)
        {
            ShowWellbeingPage();
        }

        private void GymClicked(object sender, RoutedEventArgs e)
        {
            ShowGymPage();
        }

        private void LiveMarksClicked(object sender, RoutedEventArgs e)
        {
            ShowLiveMarksPage();
        }

        private void ShowTasksPage()
        {
            TasksPage page = new TasksPage();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }

        private void ShowWellbeingPage()
        {
            wellbeing page = new wellbeing();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }
        private void ShowGymPage()
        {
            Gym page = new Gym();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }
        private void ShowLiveMarksPage()
        {
            Marks page = new Marks();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }

        private void DarknessButtonScreenClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
        }

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Visible;
        }
    }
}

