using System.Collections;
using System.Collections.Generic;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Entities.BusinessObject
{
    public class UserBObj : User
    {
       public IEnumerable<Account> AccountList { get; set; }
    }
}