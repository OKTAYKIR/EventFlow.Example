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
    [Obsolete]
    public class ExampleId :
        Identity<ExampleId>
    {
        public ExampleId(string value) : base(value) { }
    }
}