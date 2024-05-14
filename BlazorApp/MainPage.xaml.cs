using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Dynamic;
using System.Collections;
using OpenSilver.Samples.Showcase;
using System.Windows.Data;
using System.ComponentModel;

namespace BlazorApp
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        private IQueryable<Planet> _planetsArray;

        public IQueryable<Planet> PlanetsArray
        {
            get { return _planetsArray; }
            set
            {
                if (_planetsArray != value)
                {
                    _planetsArray = value;
                    OnPropertyChanged(nameof(PlanetsArray));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPage()
        {
            this.InitializeComponent();

            this.DataContext = this;

            //PlanetsArray = Planet.GetListOfPlanets().AsQueryable();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            PlanetsArray = Planet.GetListOfPlanets().AsQueryable();
        }
    }
}
