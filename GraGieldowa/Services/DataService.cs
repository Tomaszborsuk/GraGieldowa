using GraGieldowa.Interfaces;
using GraGieldowa.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraGieldowa.Services
{
    public class DataService : IDataService
    {
        private IList<User> _users;
        //private IList<ItemType> _itemTypes;
        //private IList<Medium> _mediums;
        //private IList<LocationType> _locationTypes;

        public DataService()
        {
            //PopulateItemTypes();
            //PopulateMediums();
            //PopulateLocationTypes();
            PopulateItems();
        }

        private void PopulateItems()
        {
            var cd = new User
            {
                Id = 1,
                UserName = "Test1",
                AccountBalance = 1000
            };

            var book = new User
            {
                Id = 2,
                UserName = "Test2",
                AccountBalance = 3000
            };

            _users = new List<User>
            {
                cd,
                book,
            };
        }

        //private void PopulateMediums()
        //{
        //    var cd = new Medium { Id = 1, MediaType = ItemType.Music, Name = "CD" };
        //    var vinyl = new Medium { Id = 2, MediaType = ItemType.Music, Name = "Vinyl" };
        //    var hardcover = new Medium { Id = 3, MediaType = ItemType.Book, Name = "Hardcover" };
        //    var paperback = new Medium { Id = 4, MediaType = ItemType.Book, Name = "Paperback" };
        //    var dvd = new Medium { Id = 5, MediaType = ItemType.Video, Name = "DVD" };
        //    var bluRay = new Medium { Id = 6, MediaType = ItemType.Video, Name = "Blu Ray" };

        //    _mediums = new List<Medium>
        //    {
        //        cd,
        //        vinyl,
        //        hardcover,
        //        paperback,
        //        dvd,
        //        bluRay
        //    };
        //}

        //private void PopulateItemTypes()
        //{
        //    _itemTypes = new List<ItemType>
        //    {
        //        ItemType.Book,
        //        ItemType.Music,
        //        ItemType.Video
        //    };
        //}

        //private void PopulateLocationTypes()
        //{
        //    _locationTypes = new List<LocationType>
        //    {
        //        LocationType.InCollection,
        //        LocationType.Loaned
        //    };
        //}

        //public int AddItem(MediaItem item)
        //{
        //    item.Id = _items.Max(i => i.Id) + 1;
        //    _items.Add(item);

        //    return item.Id;
        //}

        public User GetUser(int id)
        {
            return _users.FirstOrDefault(i => i.Id == id);
        }

        public IList<User> GetUsers()
        {
            return _users;
        }

        //public IList<ItemType> GetItemTypes()
        //{
        //    return _itemTypes;
        //}

        //public IList<Medium> GetMediums()
        //{
        //    return _mediums;
        //}

        //public IList<Medium> GetMediums(ItemType itemType)
        //{
        //    return _mediums
        //        .Where(m => m.MediaType == itemType)
        //        .ToList();
        //}

        //public IList<LocationType> GetLocationTypes()
        //{
        //    return _locationTypes;
        //}

        //public void UpdateItem(MediaItem item)
        //{
        //    var idx = -1;
        //    var matchedItem =
        //        (from x in _items
        //         let ind = idx++
        //         where x.Id == item.Id
        //         select ind).FirstOrDefault();

        //    if (idx == -1)
        //    {
        //        throw new Exception("Unable to update item. Item not found in collection.");
        //    }

        //    _items[idx] = item;
        //}

        //public Medium GetMedium(string name)
        //{
        //    return _mediums.FirstOrDefault(m => m.Name == name);
        //}
    }
}