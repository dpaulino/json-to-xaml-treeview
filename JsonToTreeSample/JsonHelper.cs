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
        public static TreeViewNode JsonToTree(JArray root, string rootName = "Root", string nodeName = "Item")
        {
            TreeViewNode parent = new TreeViewNode()
            {
                Content = rootName
            };
            int index = 0;

            foreach (JToken obj in root)
            {
                if (obj.Type != JTokenType.Object)
                {
                    parent.Children.Add(new TreeViewNode() { Content = obj.ToString() });
                    continue;
                }

                TreeViewNode child = new TreeViewNode()
                {
                    Content = $"{nodeName} {index++}"
                };

                foreach (KeyValuePair<string, JToken> token in (JObject)obj)
                {
                    switch (token.Value.Type)
                    {
                        case JTokenType.Array:
                            child.Children.Add(JsonToTree((JArray)token.Value, token.Key));
                            break;
                        case JTokenType.Object:
                            child.Children.Add(JsonToTree((JObject)token.Value, token.Key));
                            break;
                        default:
                            child.Children.Add(GetChild(token));
                            break;
                    }
                }

                parent.Children.Add(child);
            }

            return parent;
        }

        public static TreeViewNode JsonToTree(JObject root, string text = "")
        {
            TreeViewNode parent = new TreeViewNode()
            {
                Content = text
            };

            foreach (KeyValuePair<string, JToken> token in root)
            {

                switch (token.Value.Type)
                {
                    case JTokenType.Object:
                        parent.Children.Add(JsonToTree((JObject)token.Value, token.Key));
                        break;
                    case JTokenType.Array:
                        parent.Children.Add(JsonToTree((JArray)token.Value, token.Key));
                        break;
                    default:
                        parent.Children.Add(GetChild(token));
                        break;
                }
            }

            return parent;
        }

        private static TreeViewNode GetChild(KeyValuePair<string, JToken> token)
        {
            TreeViewNode child = new TreeViewNode()
            {
                Content = $"{token.Key}: {token.Value.ToString()}"
            };

            return child;
        }
    }
}
