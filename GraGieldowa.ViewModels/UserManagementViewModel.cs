using MvvmGen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraGieldowa.ViewModels
{
    [ViewModel]
    public partial class UserManagementViewModel
    {
        [Property]
        private UserViewModel _currentUser;
        [Property]
        private UserViewModel _selectedUser;
        partial void OnInitialize()
        {

        }
        public ObservableCollection<UserViewModel> Users { get; } = new();
    }
}
