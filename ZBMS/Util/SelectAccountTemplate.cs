using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util
{
    public class SelectAccountTemplate : DataTemplateSelector
    {
        public DataTemplate SavingsAccountDataTemplate { get; set; }
        public DataTemplate CurrentAccountDataTemplate { get; set; }
      
        protected override DataTemplate SelectTemplateCore(object account)
        {
            switch (account)
            {
                case SavingsAccount _:
                    return SavingsAccountDataTemplate;
                case CurrentAccount _:
                    return CurrentAccountDataTemplate;
                default:
                    return base.SelectTemplateCore(account);
            }
        }

        protected override DataTemplate SelectTemplateCore(object account, DependencyObject container)
        {
            return SelectTemplateCore(account);
        }
    }
}