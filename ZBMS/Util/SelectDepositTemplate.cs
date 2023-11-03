using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util
{
    public class SelectDepositTemplate: DataTemplateSelector
    {
       
        public DataTemplate RecurringDepositDataTemplate { get; set; }
        public DataTemplate FixedDepositDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object deposit)
        {
            switch (deposit)
            {
                case RecurringAccount _:
                    return RecurringDepositDataTemplate;
                case FixedDeposit _:
                    return FixedDepositDataTemplate;
                default:
                    return base.SelectTemplateCore(deposit);
            }
        }

        protected override DataTemplate SelectTemplateCore(object deposit, DependencyObject container)
        {
            return SelectTemplateCore(deposit);
        }
    }
}