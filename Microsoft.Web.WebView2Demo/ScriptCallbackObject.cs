using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

namespace WebView2Demo
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.core.corewebview2.addhostobjecttoscript?view=webview2-dotnet-1.0.664.37
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    /// <summary>
    /// 网页调用C#方法
    /// </summary>
    public class ScriptCallbackObject
    {
        public string UserName { get; set; } = "我是C#属性";

        public void ShowMessage()
        {
            MessageBox.Show("网页调用C#");
        }

        public void ShowMessageArg(string arg)
        {
            MessageBox.Show("【网页调用C#】:" + arg);
        }

        public string GetData(string arg)
        {
            return "【网页调用C#获取数据】;" + arg;
        }

        [System.Runtime.CompilerServices.IndexerName("Items")]
        public string this[int index]
        {
            get { return m_dictionary[index]; }
            set { m_dictionary[index] = value; }
        }
        private Dictionary<int, string> m_dictionary = new Dictionary<int, string>();

        /// <summary>
        /// 通过回调方式返回数据【该种方法咱不知道如何传参数】
        /// </summary>
        /// <param name="javascriptCallback"></param>
        //public void GetDataByCallback(IJavascriptCallback javascriptCallback)
        //{
        //    Task.Factory.StartNew(async () =>
        //    {
        //        using (javascriptCallback)
        //        {
        //            await javascriptCallback.ExecuteAsync("【网页调用C#返回数据】:" + Guid.NewGuid().ToString());
        //        }
        //    });
        //}
    }
}
