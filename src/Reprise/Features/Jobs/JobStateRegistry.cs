using System.Collections;

namespace Reprise
{
    internal sealed class JobStateRegistry : IEnumerable<JobState>
    {
        private readonly List<JobState> _JobStates = new();

        public void Add(JobState jobState)
        {
            _JobStates.Add(jobState);
        }

        public IEnumerator<JobState> GetEnumerator()
        {
            return _JobStates.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
