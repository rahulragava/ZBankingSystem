﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMS.ViewModel.DetailViewModel;
using ZBMSLibrary.Entities.BusinessObject;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.Pages.AccountsDetailsPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalLoanDetailPage : Page
    {
        public PersonalLoanAccountDetailViewModel PersonalLoanAccountDetailViewModel { get; set; }
        public PersonalLoanDetailPage()
        {
            PersonalLoanAccountDetailViewModel = new PersonalLoanAccountDetailViewModel();
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
            var personalLoanBObj = e.Parameter as PersonalLoanBObj;
            PersonalLoanAccountDetailViewModel.PersonalLoanBObj= personalLoanBObj;

        }
    }
}
