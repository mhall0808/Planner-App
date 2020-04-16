using System;
using System.Collections.Immutable;

namespace PlannerApp.Data.ViewModels
{
    public class IndexViewModel
    {
        public bool IsAuthenticated { get; set; }

        public ImmutableList<string> Plans { get; set; }
    }
}
