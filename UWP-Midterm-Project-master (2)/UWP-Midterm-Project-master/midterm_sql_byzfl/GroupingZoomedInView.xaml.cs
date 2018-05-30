using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Data.Core;
using Telerik.UI.Xaml.Controls.Grid;
using Telerik.UI.Xaml.Controls.Grid.Commands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using midterm_project.Models;
using System.Collections.ObjectModel;
using Telerik.Core;
using midterm_sql_byzfl;
using Microsoft.Toolkit.Uwp.Helpers;
using midterm_project.Services;


//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace midterm_sql_byzfl
{
    public sealed partial class GroupingZoomedInView : UserControl, ISemanticZoomInformation
    {
        PeopleViewModel peopleViewModel;
       
        public GroupingZoomedInView()
        {
            this.InitializeComponent();
            peopleViewModel = new PeopleViewModel();
            //oldview = new PeopleViewModel();
            this.DataContext = peopleViewModel;
        }

        public void CompleteViewChange()
        {
            // throw new NotImplementedException();
        }

        public void CompleteViewChangeFrom(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
            var list = this.SemanticZoomOwner.ZoomedOutView as GridView;

            if (list != null)
            {
                list.ItemsSource = source.Item;
            }
        }

        public void CompleteViewChangeTo(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
            // throw new NotImplementedException();
        }

        public void InitializeViewChange()
        {
            // throw new NotImplementedException();
        }

        public bool IsActiveView
        {
            get;
            set;
        }

        public bool IsZoomedInView
        {
            get;
            set;
        }

        public void MakeVisible(SemanticZoomLocation item)
        {
           
        }

        public SemanticZoom SemanticZoomOwner
        {
            get;
            set;
        }

        public void StartViewChangeFrom(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
            source.Item = this.dataGrid.GetDataView().Items.OfType<IDataGroup>().Select(c => c.Key);
        }

        public void StartViewChangeTo(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
            var dataview = this.dataGrid.GetDataView();
            var group = dataview.Items.OfType<IDataGroup>().Where(c => c.Key.Equals(source.Item)).FirstOrDefault();

            var lastGroup = dataview.Items.Last() as IDataGroup;
            if (group != null && lastGroup != null)
            {
                this.dataGrid.ScrollItemIntoView(lastGroup.ChildItems[lastGroup.ChildItems.Count - 1], () =>
                {
                    this.dataGrid.ScrollItemIntoView(group.ChildItems[0]);
                });
            }
        }

        private void CreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            userItem newItem = new userItem("333", "233", 1, "picture");
            peopleViewModel.staticData.Add(newItem);
            //var i = dataGrid.SelectedItem;
            //dataGrid.ScrollItemIntoView(newItem);
            dataGrid.SelectItem(newItem);
           // dataGrid.BeginEdit(dataGrid.SelectedItem);
            dataGrid.ScrollItemIntoView(newItem, () =>
            {
                try
                {
                    dataGrid.BeginEdit(dataGrid.SelectedItem);
                }
                catch
                {

                }
            });
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            
        }
        private void CustomerSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (CustomerSearchBox != null)
            {
                CustomerSearchBox.AutoSuggestBox.QuerySubmitted += CustomerSearchBox_QuerySubmitted;
                CustomerSearchBox.AutoSuggestBox.TextChanged += CustomerSearchBox_TextChanged;
                CustomerSearchBox.AutoSuggestBox.PlaceholderText = "Search users...";
            }

            
        }
        private async void CustomerSearchBox_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (String.IsNullOrEmpty(sender.Text))
                {
                   // peopleViewModel.Load();
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                       await peopleViewModel.loadAsync());
                    //peopleViewModel.Load();
                    sender.ItemsSource = null;
                }
                else
                {
                    string[] parameters = sender.Text.Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    sender.ItemsSource = peopleViewModel.staticData
                        .Where(x => parameters.Any(y =>
                            x.UserName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ))
                        .OrderByDescending(x => parameters.Count(y =>
                            x.UserName.StartsWith(y, StringComparison.OrdinalIgnoreCase) 
                           ))
                        .Select(x => $"{x.UserName} {x.Password}");
                }
            }
        }
        private async void CustomerSearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.QueryText))
            {
               await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                   await peopleViewModel.loadAsync());
                //sender.ItemsSource = peopleViewModel.staticData;
            }
            else
            {
                string[] parameters = sender.Text.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                var matches =peopleViewModel.staticData.Where(x => parameters
                    .Any(y =>
                        x.UserName.StartsWith(y, StringComparison.OrdinalIgnoreCase)
                       ))
                    .OrderByDescending(x => parameters.Count(y =>
                        x.UserName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ))
                    .ToList();

                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    peopleViewModel.staticData.Clear();
                    foreach (var match in matches)
                    {
                        peopleViewModel.staticData.Add(match);
                    }
                });
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedItem != null)
            {
                userItem i = (userItem)dataGrid.SelectedItem;
                userManager.Remove(i.UserName);
                peopleViewModel.staticData.Remove(i);
                dataGrid.SelectedItem = null;
            }
        }
    }

    public class FlyoutGroupHeaderTapCommand : DataGridCommand
    {
        public FlyoutGroupHeaderTapCommand()
        {
            this.Id = CommandId.FlyoutGroupHeaderTap;
        }

        public override bool CanExecute(object parameter)
        {
            return parameter is FlyoutGroupHeaderTapContext;
        }

        public override void Execute(object parameter)
        {
            var context = parameter as FlyoutGroupHeaderTapContext;
            if (context.Descriptor is DelegateGroupDescriptor && context.Action == DataGridFlyoutGroupHeaderTapAction.RemoveDescriptor)
            {
                // do not allow removal of our custom Delegate descriptor
                return;
            }
            this.Owner.CommandService.ExecuteDefaultCommand(CommandId.FlyoutGroupHeaderTap, parameter);
        }
       
    }

    public class selectviewmodel : ViewModelBase
    {
         private ObservableCollection<userItem> selelectedGridItems;
        public ObservableCollection<userItem> SelelectedGridItems
        {
            get { return selelectedGridItems ?? (selelectedGridItems = new ObservableCollection<userItem>()); }
            set { selelectedGridItems = value;  OnPropertyChanged(); }
        }
        public void RadDataGrid_OnSelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
        {
            // Add any selected items
            if (e.AddedItems != null && e.AddedItems.Any())
            {
                foreach (userItem item in e.AddedItems)
                {
                    if (!SelelectedGridItems.Contains(item))
                        SelelectedGridItems.Add(item);
                }
            }

            // Remove any unselected items
            if (e.RemovedItems != null && e.RemovedItems.Any())
            {
                foreach (userItem item in e.RemovedItems)
                {
                    if (SelelectedGridItems.Contains(item))
                        SelelectedGridItems.Remove(item);
                }
            }
        }
    }
    public class CustomCommitEditCommand : DataGridCommand
    {
        public CustomCommitEditCommand()
        {
            this.Id = CommandId.CommitEdit;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var context = parameter as EditContext;
            var i = (userItem)( context.CellInfo.Item);
            userManager.Insert(i);
            // Executes the default implementation of this command
            this.Owner.CommandService.ExecuteDefaultCommand(CommandId.CommitEdit, context);
        }
    }

    public class CustomBeginEditCommand : DataGridCommand
    {
        public CustomBeginEditCommand()
        {
            this.Id = CommandId.BeginEdit;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var context = parameter as EditContext;
            var i = (userItem)(context.CellInfo.Item);
            userManager.Remove(i.UserName);
            // Executes the default implementation of this command
            this.Owner.CommandService.ExecuteDefaultCommand(CommandId.BeginEdit, context);
        }
    }
    public class CustomCancleEditCommand : DataGridCommand
    {
        public CustomCancleEditCommand()
        {
            this.Id = CommandId.CancelEdit;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var context = parameter as EditContext;
            var i = (userItem)(context.CellInfo.Item);
            userManager.Insert(i);

            this.Owner.CommandService.ExecuteDefaultCommand(CommandId.BeginEdit, context);
        }

       
    }
}
