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
        public static TreeViewNode JsonToTree(JContainer obj, string nodeName)
        {
            if (obj == null)
                return null;

            var parent = new TreeViewNode() { Content = nodeName };

            if (obj.Type == JTokenType.Object)
            {
                foreach (KeyValuePair<string, JToken> t in (JObject)obj)
                {
                    if (t.Value.Type == JTokenType.Object)
                    {
                        parent.Children.Add(JsonToTree((JObject)t.Value, t.Key));
                    }
                    else if (t.Value.Type == JTokenType.Array)
                    {
                        parent.Children.Add(JsonToTree((JArray)t.Value, $"{nodeName}[{t.Value.Count()}]"));
                    }
                    else
                    {
                        parent.Children.Add(GetChild(t));
                    }
                }
            }
            else if (obj.Type == JTokenType.Array)
            {
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
            }
            else
            {
                parent.Children.Add(new TreeViewNode()
                {
                    Content = obj.ToString()
                });
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
