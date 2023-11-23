using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMS.Util;
using ZBMS.Util.PageArguments;
using ZBMS.ViewModel.DetailViewModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.Pages.AccountsDetailsPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecurringDepositDetailPage : Page, IRecurringDepositView
    {
        public RecurringDepositDetailViewModel RecurringDepositDetailViewModel { get; set; }
        public RecurringDepositDetailPage()
        {
            RecurringDepositDetailViewModel = new RecurringDepositDetailViewModel(this);
            this.InitializeComponent();
        }

        private void BackButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var recurringDepositParameters = e.Parameter as RecurringDepositPageArguments;
            RecurringDepositDetailViewModel.RecurringAccountBObj= recurringDepositParameters?.RecurringAccountBObj;
            RecurringDepositDetailViewModel.RepaymentAccountNumber =
                recurringDepositParameters?.RecurringAccountBObj.SavingsAccountId;
            RecurringDepositDetailViewModel.FromAccountNumber =
                recurringDepositParameters?.RecurringAccountBObj.FromAccountId;
            RecurringDepositDetailViewModel.Accounts= recurringDepositParameters?.Accounts;
            if (RecurringDepositDetailViewModel.RecurringAccountBObj?.AccountStatus == AccountStatus.Closed)
            {
                DepositCloseIcon.Visibility = Visibility.Visible;
                DetailGrid.Visibility = Visibility.Collapsed;
                CloseDeposit.Visibility = Visibility.Collapsed;
                
            }
            else
            {
                DepositCloseIcon.Visibility = Visibility.Collapsed;
                DetailGrid.Visibility = Visibility.Visible;
                CloseDeposit.Visibility = Visibility.Visible;

            }
        }

        public void UpdateRepaymentAccount(string savingsAccountNumber)
        {
            RecurringDepositDetailViewModel.RepaymentAccountNumber = savingsAccountNumber;

        }
        private void ClosingDepositContentDialog_OnPrimaryButtonClicked()
        {
            RecurringDepositDetailViewModel.ClosingAccountManually();
        }
        private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;
                BackButton_OnTapped(sender, new TappedRoutedEventArgs());
                //DepositButton_OnClick(sender, e);
            }
        }

        private void CloseDeposit_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (RecurringDepositDetailViewModel.RecurringAccountBObj.AccountStatus == AccountStatus.Active)
            {
                ClosingDepositContentDialog.Visibility = Visibility.Visible;
                ClosingDepositContentDialog.ShowDialog();
            }
            
        }

        public void CloseRecurringDepositSuccess()
        {
            RecurringDepositSummary.OnDepositClosed();  
            ChangeRepaymentAccountControl.OnCloseDeposit();
            DetailGrid.Visibility = Visibility.Collapsed;
            DepositCloseIcon.Visibility = Visibility.Visible;
            CloseDeposit.Visibility = Visibility.Collapsed;
        }

        private void BackButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }

        private void BackButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
        }

        private void CloseDeposit_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 19;

                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }

        private void CloseDeposit_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 21;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
        }
    }

    public interface IRecurringDepositView
    {
        void CloseRecurringDepositSuccess();
    }
}
