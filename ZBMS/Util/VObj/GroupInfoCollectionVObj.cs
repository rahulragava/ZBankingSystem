using System.Collections.ObjectModel;

namespace ZBMS.Util.VObj
{
    public class GroupInfoCollectionVObj<T> : ObservableCollection<T>
    { 
        public object Key { get; set; }
    }
}