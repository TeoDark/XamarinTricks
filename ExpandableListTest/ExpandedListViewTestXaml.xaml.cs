using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FLM.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpandedListViewTestXaml : ContentPage
    {
        private ObservableCollection<FoodGroup> _allGroups;
        private ObservableCollection<FoodGroup> _expandedGroups;


        public ExpandedListViewTestXaml()
        {
            InitializeComponent();
            _allGroups = FoodGroup.All;
            UpdateListContent();
        }

        private void HeaderTapped(object sender, EventArgs args)
        {
            int selectedIndex = _expandedGroups.IndexOf(
                ((FoodGroup)((Button)sender).CommandParameter));
            _allGroups[selectedIndex].Expanded = !_allGroups[selectedIndex].Expanded;
            UpdateListContent();
        }

        private void UpdateListContent()
        {
            _expandedGroups = new ObservableCollection<FoodGroup>();
            foreach(FoodGroup group in _allGroups)
            {
                FoodGroup newGroup = new FoodGroup(group.Title, group.ShortName, group.Expanded);
                if(group.Expanded)
                {
                    foreach(Food food in group)
                    {
                        newGroup.Add(food);
                    }
                }
                _expandedGroups.Add(newGroup);
            }
            GroupedView.ItemsSource = _expandedGroups;
        }
    }
    public class Food
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class FoodGroup : ObservableCollection<Food>, INotifyPropertyChanged
    {
        public static ObservableCollection<FoodGroup> All { private set; get; }
        private bool _expanded;
        public string Title { get; set; }
        public string TitleWithItemCount
        {
            get { return string.Format("{0} ({1})", Title, FoodCount); }
        }
        public string ShortName { get; set; }
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon { get { return Expanded ? "arrow_up.png" : "arrow_down.png"; } }

        public int FoodCount { get; set; }
        public FoodGroup(string title, string shortName, bool expanded = true)
        {
            Title = title;
            ShortName = shortName;
            Expanded = expanded;
        }

        static FoodGroup()
        {
            ObservableCollection<FoodGroup> Groups = new ObservableCollection<FoodGroup>();
            for (int i = 5; i < 20; i++)
            {
                var foodGroup = new FoodGroup("ASDDFGH " + i, i.ToString(), false);
                for (int y = 0; y < i; y++)
                {
                    foodGroup.Add(new Food { Name = "MNB" + y, Description = i.ToString(), Icon = "btn_dead_fish.png" });
                }
                Groups.Add(foodGroup);
            }
            All = Groups;
        }
    }
}
