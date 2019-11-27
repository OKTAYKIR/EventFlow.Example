using EventFlow.Core;
using System;

namespace EventFlowExample.Aggregates.Events
{
    public class WizloId : IIdentity
    {
        public WizloId(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    // Represents the aggregate identity(ID)
    //[Obsolete]
    //public class WizloId :
    //    Identity<WizloId>
    //{
    //    public WizloId(string value) : base(value) { }
    //}
}