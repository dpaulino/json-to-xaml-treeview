using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToTreeSample
{
    public class JsonHelper
    {
        public static TreeViewNode JsonToTree(JArray obj, string nodeName)
        {
            if (obj == null)
                return null;

            string itemCountString = obj.Count.ToString() + " item" + (obj.Count > 1 ? "s" : "");
            var parent = new TreeViewNode() { Content = new KeyValuePair<string, string>(nodeName, itemCountString) };
            int index = 0;

            foreach (JToken token in obj)
            {
                if (token.Type == JTokenType.Object)
                {
                    parent.Children.Add(JsonToTree((JObject)token, $"{nodeName}[{index++}]"));
                }
                else if (token.Type == JTokenType.Array)
                {
                    parent.Children.Add(JsonToTree((JArray)token, $"{nodeName}[{index++}]"));
                }
                else
                {
                    parent.Children.Add(new TreeViewNode()
                    {
                        Content = new KeyValuePair<string, JToken>($"{nodeName}[{index++}]", token)
                    });
                }
            }

            return parent;
        }

        public static TreeViewNode JsonToTree(JObject obj, string nodeName)
        {
            if (obj == null)
                return null;

            var parent = new TreeViewNode() { Content = new KeyValuePair<string, string>(nodeName, obj.Count.ToString() + " items") };

            foreach(KeyValuePair<string, JToken> pair in obj)
            {
                if (pair.Value.Type == JTokenType.Object)
                {
                    parent.Children.Add(JsonToTree((JObject)pair.Value, pair.Key));
                }
                else if (pair.Value.Type == JTokenType.Array)
                {
                    parent.Children.Add(JsonToTree((JArray)pair.Value, pair.Key));
                }
                else
                {
                    parent.Children.Add(GetChild(pair));
                }
            }

            return parent;
        }

        private static TreeViewNode GetChild(KeyValuePair<string, JToken> pair)
        {
            if (pair.Value == null)
                return null;

            TreeViewNode child = new TreeViewNode()
            {
                Content = pair
            };

            return child;
        }
    }
}
