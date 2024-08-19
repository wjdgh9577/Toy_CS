using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Utility
{
    public class ArgumentContainer
    {
        Dictionary<string, object> _arguments = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                _arguments.TryGetValue(key, out object value);
                return value;
            }
            set
            {
                if (_arguments.ContainsKey(key))
                    throw new ArgumentException("Duplicated key.");

                _arguments.Add(key, value);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// Boxing 주의.
        /// </param>
        /// <returns></returns>
        public static ArgumentContainer NewContainer(params (string key, object value)[] args)
        {
            ArgumentContainer container = new ArgumentContainer();

            foreach (var arg in args)
                container[arg.key] = arg.value;

            return container;
        }
    }
}