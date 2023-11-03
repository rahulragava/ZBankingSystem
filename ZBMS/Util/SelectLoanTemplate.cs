using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util
{
    public class SelectLoanTemplate: DataTemplateSelector
    {
       
        public DataTemplate PersonalLoanDataTemplate{ get; set; }
       
        protected override DataTemplate SelectTemplateCore(object loan)
        {
            switch (loan)
            {
                case Loan _:
                    return PersonalLoanDataTemplate;
                default:
                    return base.SelectTemplateCore(loan);
            }
        }

        protected override DataTemplate SelectTemplateCore(object loan, DependencyObject container)
        {
            return SelectTemplateCore(loan);
        }
    }
}