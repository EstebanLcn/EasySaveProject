using Easysave_Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Easysave_Core.Service.SaveService
{
    public class ContextSave
    {
        
            private SaveStrategy _strategy;

            // Usually, the Context accepts a strategy through the constructor, but
            // also provides a setter to change it at runtime.
            public ContextSave(SaveStrategy strategy)
            {
                this._strategy = strategy;
            }

        public ContextSave()
        {
        }

        public SaveStrategy GetStrategy()
            {
                return _strategy;
            }
            // Usually, the Context allows replacing a Strategy object at runtime.
            public void SetStrategy(SaveStrategy strategy)
            {
                this._strategy = strategy;
            }

            // The Context delegates some work to the Strategy object instead of
            // implementing multiple versions of the algorithm on its own.
            public void ContextInterface(DirectoryInfo source, DirectoryInfo target)
            {
                _strategy.SaveInterface(source, target);
            }
    }
}
