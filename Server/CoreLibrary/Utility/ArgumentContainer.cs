using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Utility
{
    public class ArgumentContainer
    {
        Dictionary<string, string> _arguments = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                _arguments.TryGetValue(key, out string value);
                return value;
            }
            set
            {
                if (_arguments.ContainsKey(key))
                    _arguments[key] = value;
                else
                    _arguments.Add(key, value);
            }
        }

        public static ArgumentContainer NewContainer(params (object, object)[] args)
        {
            ArgumentContainer container = new ArgumentContainer();

            foreach (var arg in args)
            {
                if (arg.Item1 is string s_arg)
                {
                    container[s_arg] = arg.Item2.ToString();
                }
                else
                {
                    container[arg.Item1.ToString()] = arg.Item2.ToString();
                }
            }

            return container;
        }
    }
}