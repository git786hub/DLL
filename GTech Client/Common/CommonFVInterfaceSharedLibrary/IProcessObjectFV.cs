// =====================================================================================================================================================================
//  File Name: IProcessObjectFV.cs
// 
// Description:  This is the interface that needs to be implemented by each Feature Validation Interface configured in the model
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/09/2018          Shubham                       Initial implementation
//=====================================================================================================================================================================

namespace GTechnology.Oncor.CustomAPI
{
  public interface IProcessFV
    {
        bool Process();
    }
}
