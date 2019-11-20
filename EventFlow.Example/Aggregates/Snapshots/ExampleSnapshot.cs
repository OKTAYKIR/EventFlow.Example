using EventFlow.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventFlowExample.Aggregates.Snapshots
{
    [Serializable]
    [SnapshotVersion("example", 1)]
    public class ExampleSnaphost : ISnapshot
    {
        #region Variables
        public int? MagicNumber { get; }
        public int Counter { get; }
       
        public IReadOnlyCollection<ExampleSnapshotVersion> PreviousVersions { get; }
        #endregion

        public ExampleSnaphost(int? magicNumber,    
                               int counter, 
                               IEnumerable<ExampleSnapshotVersion> previousVersions)
        {
            MagicNumber = magicNumber;
            Counter = counter;
            PreviousVersions = (previousVersions ?? Enumerable.Empty<ExampleSnapshotVersion>()).ToList();
        }
    }
}
