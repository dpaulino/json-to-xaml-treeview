using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JsonToTreeSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public void LoadTree(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            MainTreeView.RootNodes.Clear();

            JContainer json;
            try
            {
                if (content.StartsWith("["))
                {
                    json = JArray.Parse(content);
                    MainTreeView.RootNodes.Add(JsonHelper.JsonToTree((JArray)json, "Root"));
                }
                else
                {
                    json = JObject.Parse(content);
                    MainTreeView.RootNodes.Add(JsonHelper.JsonToTree((JObject)json, "Root"));
                }
            }
            catch (JsonReaderException)
            {
                // invalid json
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadTree(JsonTextBox.Text);
        }

        private void MainTreeView_ItemInvoked(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs args)
        {
            var content = ((Microsoft.UI.Xaml.Controls.TreeViewNode)args.InvokedItem).Content;

            if (content is KeyValuePair<string, string> c)
            {
                DetailsBlock.Text = c.Value;
            }
            else if (content is KeyValuePair<string, JToken> j)
            {
                DetailsBlock.Text = j.Value.ToString();
            }
        }
    }
}
