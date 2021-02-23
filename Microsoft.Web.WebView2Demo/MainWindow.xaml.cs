using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace WebView2Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //LoadTest();
            webView.NavigationStarting += WebView_NavigationStarting;
            InitializeAsync();
        }



        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);

            webView.CoreWebView2.AddHostObjectToScript("webBrowserObj", new ScriptCallbackObject());

            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("var webBrowserObj= window.chrome.webview.hostObjects.webBrowserObj;");

            webView.CoreWebView2.ProcessFailed += delegate
            {

            };
            #region 网页给浏览器发消息
            webView.CoreWebView2.WebMessageReceived += UpdateAddressBar;
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            #endregion

            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
        }
        private void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string uri = e.TryGetWebMessageAsString();

            //addressBar.Text = uri;
            webView.CoreWebView2.PostWebMessageAsString(uri);
        }

        private void WebView_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            //webView.CoreWebView2.Settings.IsZoomControlEnabled = false;
        }

        protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }


        private void btn_Click(object sender, RoutedEventArgs e)
        {
            LoadTest();
        }

        private void LoadTest()
        {
            //注入脚本 这里执行 有时候成功 有时候不成功 放到FrameLoadEnd事件里面
            // chromiumWebBrowser.ExecuteScriptAsync("CefSharp.BindObjectAsync('chromiumWebBrowser')");
            string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/"), "pages/testPage.html");


            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate("file:///" + url);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(txtUrl.Text);
            }
        }


        private void callJS_Click(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2.ExecuteScriptAsync("ShowMessage()");

            return;

            #region 其他功能
            webView.CoreWebView2.ExecuteScriptAsync("Hello()");
            // 为 js 的 变量jsVar赋值 'abc'
            webView.CoreWebView2.ExecuteScriptAsync("jsVar='---改变值abc'");
            webView.CoreWebView2.ExecuteScriptAsync("Hello()");
            #endregion
        }

        private void callJSArg_Click(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2.ExecuteScriptAsync($"ShowMessageArg('{txtArg.Text}')");
        }

        private async void callJSGetData_Click(object sender, RoutedEventArgs e)
        {
            var jsResult = await webView.CoreWebView2.ExecuteScriptAsync($"GetData('{txtArg.Text}')");
            if (!string.IsNullOrEmpty(jsResult))
            {
                MessageBox.Show(jsResult);
            }

            //var jsResult =   webView.CoreWebView2.ExecuteScriptAsync($"GetData('{txtArg.Text}')");
            //jsResult.Wait();
            //if (jsResult.Result != null)
            //{
            //    MessageBox.Show(jsResult.Result.ToString());
            //}
        }
    }
}
