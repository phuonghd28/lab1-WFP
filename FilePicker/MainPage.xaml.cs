using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FilePicker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private async void HandleOpenFile()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".txt");
            
            StorageFile file = await openPicker.PickSingleFileAsync();
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.CloseButtonText = "Done";
            if (file != null)
            {
                var stringContent = await FileIO.ReadTextAsync(file);
                editor.Text = stringContent;
            }
            else
            {
                contentDialog.Title = "Action failed !";
                contentDialog.Content = "Please choose to file to save !";
            }
        }

        private async void HandleSaveFile()
        {
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "New Document";
            StorageFile file = await savePicker.PickSaveFileAsync();
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.CloseButtonText = "Done";
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, editor.Text);
                
                contentDialog.Title = "Action success !";
                contentDialog.Content = "Save file success !";
            }
            else
            {
                contentDialog.Title = "Action failed !";
                contentDialog.Content = "Please choose to file to save !";
            }
            await contentDialog.ShowAsync();
        }

        private async void HandleSaveFileInStorage()
        {
            StorageFolder storage = ApplicationData.Current.LocalFolder;
            StorageFile file = await storage.CreateFileAsync("test.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, editor.Text);
        }


        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            switch(menuItem.Tag)
            {
                case "Open":
                    HandleOpenFile();
                    break;
                case "Save":
                    HandleSaveFile();
                    break;
                case "saveStorage":
                    HandleSaveFileInStorage();
                    break;
                case "New":
                    break;
            }
        }

        
    }
}
