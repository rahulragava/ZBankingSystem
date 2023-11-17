//using System.Collections.ObjectModel;
//using System;
//using Windows.UI.Core;
//using ZBMS.View.UserControl;
//using ZBMSLibrary.Data;
//using ZBMSLibrary.Entities.Model;
//using ZBMSLibrary.UseCase;

//namespace ZBMS.ViewModel
//{
//    public class ChangeSenderAccountDepositViewModel
//    {
//        public readonly IChangeSenderAccountView ChangeSenderAccountView;
//        public ObservableCollection<string> AccountNumbers;

//        public ChangeSenderAccountDepositViewModel(IChangeSenderAccountView changeSenderAccountView)
//        {
//            ChangeSenderAccountView = changeSenderAccountView;
//            AccountNumbers = new ObservableCollection<string>();
//        }


//        public void SetAccountNumbers(ObservableCollection<Account> accounts)
//        {
//            AccountNumbers.Clear();
//            foreach (var account in accounts)
//            {
//                AccountNumbers.Add(account.AccountNumber);
//            }
//        }

//        public void ChangeAccountForDeposit(string accountNumber, Deposit deposit)
//        {

//        }

//        public class ChangeAccountForDepositPresenterCallBack : IPresenterCallBack<ChangeSenderAccountDepositResponse>
//        {
//            private readonly ChangeSenderAccountDepositViewModel _changeSenderAccountDepositViewModel;

//            public ChangeAccountForDepositPresenterCallBack(ChangeSenderAccountDepositViewModel changeSenderAccountDepositViewModel)
//            {
//                _changeSenderAccountDepositViewModel = changeSenderAccountDepositViewModel;
//            }

//            public void OnSuccess(ChangeSenderAccountDepositResponse response)
//            {
//                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
//                    () =>
//                    {
//                        _changeSenderAccountDepositViewModel.ChangeSenderAccountView.UpdateSenderAccount(response.AccountNumber);
//                    }
//                );
//            }

//            public void OnError(Exception ex)
//            {
//                throw new NotImplementedException();
//            }
//        }
//    }
//}