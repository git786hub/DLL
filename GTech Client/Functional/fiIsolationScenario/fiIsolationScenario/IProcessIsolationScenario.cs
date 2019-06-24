// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: IProcessIsolationScenario.cs
// 
//  Description:   ProcessIsolationScenario interface to get the validation message.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  23/03/2018        Prathyusha                  Created 
// ======================================================
namespace GTechnology.Oncor.CustomAPI
{
    public interface IProcessIsolationScenario
    {
        /// <summary>
        /// ProcessIsolationScenario
        /// </summary>
        /// <param name="ErrorMessage">Error Message to be displayed</param>
        /// <param name="ErrorPriority">Error Priority</param>
        void ProcessIsolationScenario(out string ErrorMessage, out string ErrorPriority);
        void ValidateIsolationScenario();
    }
}
