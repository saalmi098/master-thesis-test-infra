using Xunit.Sdk;
using Xunit.v3;

// xUnit reads assembly-level attributes from the test assembly only, so this must live in
// every test project. The tests drive one shared app and must run one at a time.
[assembly: Parallelization(Mode = ParallelMode.None)]
