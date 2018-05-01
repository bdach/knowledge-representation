using System;
using System.Collections.Generic;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;

namespace Client.Global
{
    [Serializable]
    public class SerializableScenario
    {
        public List<ActionViewModel> ActionViewModels { get; set; }
        
        public List<CompoundActionViewModel> CompoundActionViewModels { get; set; }
        
        public List<LiteralViewModel> LiteralViewModels { get; set; }
        
        public List<ProgramViewModel> ProgramViewModels { get; set; }
        
        public SerializableScenario()
        {
            ActionViewModels = new List<ActionViewModel>();
            CompoundActionViewModels = new List<CompoundActionViewModel>();
            LiteralViewModels = new List<LiteralViewModel>();
            ProgramViewModels = new List<ProgramViewModel>();
        }
    }
}