using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace midterm_sql_byzfl.Services
{
    public static class shareManager
    {
        static string Title;
        static string Description;
        static string Text;
        static StorageFile ImageFile;

        //Failed!!
        //public static void shareIt(string title, string description, string text, StorageFile imageFile)
        //{
        //    DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
        //    Title = title;
        //    Description = description;
        //    Text = text;
        //    ImageFile = imageFile;
        //    DataTransferManager.ShowShareUI();
        //    DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        //}
        public static void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = Title;
            request.Data.Properties.Description = Description;
            request.Data.SetText(Text);
            if (ImageFile != null)
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(ImageFile));
        }
    }
}
