using PluginModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformaSitecoreWord.Util
{
    public sealed class PluginSingletonVerticalRoot
    {
        private static volatile PluginSingletonVerticalRoot instance;
        private static object syncRoot = new Object();
        public VerticalStruct CurrentVertical { get; set; }

        private PluginSingletonVerticalRoot() { }

        public static PluginSingletonVerticalRoot Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PluginSingletonVerticalRoot();
                    }
                }

                return instance;
            }
        }
    }
}
