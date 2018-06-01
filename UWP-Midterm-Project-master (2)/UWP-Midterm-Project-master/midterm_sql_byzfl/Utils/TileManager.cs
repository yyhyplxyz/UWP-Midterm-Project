using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using midterm_project.Services;
using Windows.UI.Notifications;

namespace midterm_sql_byzfl.Utils
{
    class TileManager
    {
        public static void UpdatePrimaryTile(string input, string input2, DateTimeOffset input3)
        {
            var xmlDoc = TileService.CreateTiles(new PrimaryTile(input, input2, input3));
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            TileNotification notification = new TileNotification(xmlDoc);
            updater.Update(notification);
        }

        public void circulationUpdate()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            for (int i = 0; i < materialManager.GetItems(materialManager.SELECTCODE.DETERIORATED).Count(); i++)
            {
                UpdatePrimaryTile(materialManager.GetItems(materialManager.SELECTCODE.DETERIORATED)[i].name, materialManager.GetItems(materialManager.SELECTCODE.DETERIORATED)[i].comment, materialManager.GetItems(materialManager.SELECTCODE.DETERIORATED)[i].purchaseDate);
            }
        }
    }
}
