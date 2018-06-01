
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
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace midterm_sql_byzfl
{

    public sealed partial class GroupingZoomedInViewmenu : UserControl, ISemanticZoomInformation
    {
        menuviewmodel peopleViewModel;

        public GroupingZoomedInViewmenu()
        {
            this.InitializeComponent();
            peopleViewModel = new menuviewmodel();
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
            List<String> tmp = new List<string>();
            tmp.Add("233");
            List<double> tmp2 = new List<double>();
            tmp2.Add(1);
            menuItem newItem = new menuItem("samplename", tmp, tmp2, "Eastern", "", "", "100$");
            peopleViewModel.staticData.Add(newItem);
            dataGrid.SelectItem(newItem);

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
                CustomerSearchBox.AutoSuggestBox.PlaceholderText = "Search menus...";
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
                    peopleViewModel.Load();
                    sender.ItemsSource = null;
                }
                else
                {
                    string[] parameters = sender.Text.Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    sender.ItemsSource = peopleViewModel.staticData
                        .Where(x => parameters.Any(y =>
                            x.menuName.StartsWith(y, StringComparison.OrdinalIgnoreCase)))
                        .OrderByDescending(x => parameters.Count(y =>
                            x.menuName.StartsWith(y, StringComparison.OrdinalIgnoreCase)
                           ))
                        .Select(x => $"{x.menuName} {x.description}");
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

                var matches = peopleViewModel.staticData.Where(x => parameters
                     .Any(y =>
                         x.menuName.StartsWith(y, StringComparison.OrdinalIgnoreCase)
                        ))
                    .OrderByDescending(x => parameters.Count(y =>
                        x.menuName.StartsWith(y, StringComparison.OrdinalIgnoreCase)))
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
            if (dataGrid.SelectedItem != null)
            {
                menuItem i = (menuItem)dataGrid.SelectedItem;
                menuManager.Remove(i.menuName);
                peopleViewModel.staticData.Remove(i);
                dataGrid.SelectedItem = null;
            }
        }



        private static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
            bitmapImage.SetSource(stream);
            return bitmapImage;

        }

        private async void slectphoto(object sender, RoutedEventArgs e)
        {
            menuItem i = (menuItem)dataGrid.SelectedItem;
            BitmapImage tmp = new BitmapImage();
            FileOpenPicker fo = new FileOpenPicker();
            fo.FileTypeFilter.Add(".png");
            fo.FileTypeFilter.Add(".jpg");
            fo.SuggestedStartLocation = PickerLocationId.Desktop;
            var f = await fo.PickSingleFileAsync();
            BitmapImage img = new BitmapImage();
            tmp = await LoadImage(f);
            i.trueimage = tmp;
            string t = f.DisplayType;
            t = t.Substring(0, t.Length - 2);
            t = t.ToLower();
            i.Image = "Assets/" + f.DisplayName + "." + t;
            menuManager.Update(i.menuName, i);
            await peopleViewModel.loadAsync();
            dataGrid.SelectedItem = null;
        }
    }
    public class CustomCommitEditCommand_menu : DataGridCommand
    {
        public CustomCommitEditCommand_menu()
        {
            this.Id = CommandId.CommitEdit;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public async override void Execute(object parameter)
        {
            var context = parameter as EditContext;
            var i = (menuItem)(context.CellInfo.Item);
            if (menuManager.Insert(i))
            {
                this.Owner.CommandService.ExecuteDefaultCommand(CommandId.BeginEdit, context);
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "Notice",
                    Content = "菜单名不能重复",
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = "OK",
                };
                await dialog.ShowAsync();
            }
        }
    }

}
