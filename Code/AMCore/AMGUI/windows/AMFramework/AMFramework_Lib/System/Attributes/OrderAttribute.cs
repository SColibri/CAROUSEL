using System.Runtime.CompilerServices;

namespace AMFramework_Lib.AMSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class OrderAttribute : Attribute
    {
        private readonly int _order;
        public int Order { get { return _order; } }

        public OrderAttribute([CallerLineNumber] int order = 0)
        {
            _order = order;
        }


    }
}
