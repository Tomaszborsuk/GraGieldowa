using GraGieldowa.Model;
using System.Collections.Generic;

namespace GraGieldowa.Interfaces
{
    public interface IDataService
    {
        IList<User> GetUsers();
        User GetUser(int id);
        //int AddItem(MediaItem item);
        //void UpdateItem(MediaItem item);
        //IList<ItemType> GetItemTypes();
        //Medium GetMedium(string name);
        //IList<Medium> GetMediums();
        //IList<Medium> GetMediums(ItemType itemType);
        //IList<LocationType> GetLocationTypes();
    }
}