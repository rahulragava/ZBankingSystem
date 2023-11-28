using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBMS.Util;
using ZBMS.Util.PageArguments;
using ZBMS.View.UserControl;

namespace ZBMS.Services
{
    public class WindowService
    {
        public static Dictionary<int, ViewCollectionValueObject> ViewCollection { get; set; } = new Dictionary<int, ViewCollectionValueObject>();
        private static async Task ShowAsync<T>(ProfilePageArguments profilePageArguments, bool isFullScreenRequested = false)
        {
            int viewId = -1;
            var t1 = Thread.CurrentThread.ManagedThreadId; //3

            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                var frame = new Frame
                {
                    RequestedTheme = AppSettings.Theme
                };
                var t2 = Thread.CurrentThread.ManagedThreadId; //11
                //rootGrid.Children.Add(frame);
                Window.Current.Content = frame;
                Window.Current.Activate();
                frame.Navigate(typeof(T),profilePageArguments);
                var view = ApplicationView.GetForCurrentView();

                //appView = view;

                if (isFullScreenRequested)
                {
                    view.TryEnterFullScreenMode();
                }
                else
                {
                    if (view.IsFullScreenMode)
                    {
                        view.ExitFullScreenMode();
                    }
                }

                view.Consolidated += Helper_Consolidated;
                viewId = view.Id;
                var viewCollectionValueObject = new ViewCollectionValueObject()
                {
                    Dispatcher = frame.Dispatcher,
                    Name = typeof(T).Name,
                }; 
                ViewCollection.TryAdd(view.Id , viewCollectionValueObject);
            });
            var a = t1;
            var b = Thread.CurrentThread.ManagedThreadId;
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId, ViewSizePreference.UseHalf);
        }

        public static async Task ShowOrSwitchAsync<T>(ProfilePageArguments profilePageArguments, bool isFullScreenRequested = false)
        {
            if (ViewCollection.Values.FirstOrDefault(view => string.Equals(view.Name, typeof(T).Name)) != null)
            {
                var viewId = ViewCollection.First(view => view.Value.Name == typeof(T).Name).Key;
                await ApplicationViewSwitcher.SwitchAsync(viewId);
            }
            else
            {
                await ShowAsync<T>(profilePageArguments, isFullScreenRequested);
            }
        }

        public static void Helper_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            ViewCollection.Remove(ApplicationView.GetForCurrentView().Id);
            ApplicationView.GetForCurrentView().Consolidated -= Helper_Consolidated;
        }

        public static void CloseWindow()
        {
            Debug.WriteLine("inside closeWindow");
            var a = Thread.CurrentThread.ManagedThreadId;
            if (ViewCollection.Count > 0)
            {
                foreach (var viewCollectionValueObject in ViewCollection)
                {
                    viewCollectionValueObject.Value.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                             CoreApplication.GetCurrentView().CoreWindow.Close();
                        }
                    );
                }
                ViewCollection.Clear();
                //ViewCollection.Values..Close();
                //ApplicationView.GetForCurrentView().TryConsolidateAsync();
                Debug.WriteLine("inside closeWindow");

            }
        }

    }
}