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
using ZBMS.View.UserControl.DepositSummary;
using ZBMS.ViewModel.DetailViewModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.Pages.AccountsDetailsPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FixedDepositDetailPage : Page, IFixedDepositView
    {
        public FixedDepositDetailViewModel FixedDepositDetailViewModel;
        public FixedDepositDetailPage()
        {
            FixedDepositDetailViewModel = new FixedDepositDetailViewModel(this);
            this.InitializeComponent();
        }


        private void BackButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }



        public void UpdateSenderAccount(string fromSenderAccount)
        {
            FixedDepositDetailViewModel.FromAccountNumber = fromSenderAccount;
        }

        public void UpdateRepaymentAccount(string savingsAccountNumber)
        {
            FixedDepositDetailViewModel.RepaymentAccountNumber= savingsAccountNumber;

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var fixedDepositParameters = e.Parameter as FixedDepositPageArguments;

            FixedDepositDetailViewModel.FixedDepositBObj = fixedDepositParameters?.FixedDepositBObj;
            FixedDepositDetailViewModel.FromAccountNumber = fixedDepositParameters?.FixedDepositBObj.FromAccountId;
            FixedDepositDetailViewModel.RepaymentAccountNumber= fixedDepositParameters?.FixedDepositBObj.SavingsAccountId;
            FixedDepositDetailViewModel.Accounts = fixedDepositParameters?.Accounts;
            if (FixedDepositDetailViewModel.FixedDepositBObj?.AccountStatus == AccountStatus.Closed)
            {
                DetailGrid.Visibility = Visibility.Collapsed;
                CloseDeposit.Visibility = Visibility.Collapsed;
                DepositCloseIcon.Visibility = Visibility.Visible;
            }
            else
            {
                DetailGrid.Visibility = Visibility.Visible;
                CloseDeposit.Visibility = Visibility.Visible;
                DepositCloseIcon.Visibility = Visibility.Collapsed;
            }
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
            ClosingDepositContentDialog.Visibility = Visibility.Visible;
            ClosingDepositContentDialog.ShowDialog();
        }

        private void ClosingDepositContentDialog_OnPrimaryButtonClicked()
        {
            FixedDepositDetailViewModel.ClosingAccountManually();
        }

        public void OnFixedDepositSuccessfullyClosed()
        {
            FixedDepositSummary.OnDepositClosed();
            ChangeRepaymentAccount.OnCloseDeposit();
            DetailGrid.Visibility = Visibility.Collapsed;
            CloseDeposit.Visibility = Visibility.Visible;
            //FixedDepositDetailViewModel.FixedDepositBObj.DepositedAmount = 0;
            //FixedDepositDetailViewModel.FixedDepositBObj.MaturityAmount = 0;
            //FixedDepositDetailViewModel.FixedDepositBObj.InterestRate = 0;
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

    public interface IFixedDepositView
    {
        void OnFixedDepositSuccessfullyClosed();
    }

}
