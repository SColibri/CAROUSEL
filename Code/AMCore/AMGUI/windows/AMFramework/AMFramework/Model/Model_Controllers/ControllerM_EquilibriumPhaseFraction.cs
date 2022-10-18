﻿using AMFramework.Core;
using AMFramework.Model.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Model.Model_Controllers
{
    public class ControllerM_EquilibriumPhaseFraction : Controller_Abstract_Models<Model_EquilibriumPhaseFraction>
    {
        // Constructors
        public ControllerM_EquilibriumPhaseFraction(IAMCore_Comm comm) : base(comm)
        { }
        public ControllerM_EquilibriumPhaseFraction(IAMCore_Comm comm, ModelController<Model_EquilibriumPhaseFraction> modelMC) : base(comm, modelMC)
        { }

        #region Model_methods

        #endregion
    }
}