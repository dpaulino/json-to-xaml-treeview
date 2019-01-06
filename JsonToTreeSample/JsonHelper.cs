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

            var parent = new TreeViewNode() { Content = nodeName };
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
                        Content = token.ToString()
                    });
                }
            }

            return parent;
        }

        public static TreeViewNode JsonToTree(JObject obj, string nodeName)
        {
            if (obj == null)
                return null;

            var parent = new TreeViewNode() { Content = nodeName };

            foreach(KeyValuePair<string, JToken> token in obj)
            {
                if (token.Value.Type == JTokenType.Object)
                {
                    parent.Children.Add(JsonToTree((JObject)token.Value, token.Key));
                }
                else if (token.Value.Type == JTokenType.Array)
                {
                    parent.Children.Add(JsonToTree((JArray)token.Value, token.Key));
                }
                else
                {
                    parent.Children.Add(GetChild(token));
                }
            }

            return parent;
        }

        private static TreeViewNode GetChild(KeyValuePair<string, JToken> token)
        {
            if (token.Value == null)
                return null;

            TreeViewNode child = new TreeViewNode()
            {
                Content = $"{token.Key}: {token.Value.ToString()}"
            };

            return child;
        }
    }
}
