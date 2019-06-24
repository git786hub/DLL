using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
  public class GUIMode
  {

    /// <summary>
    /// Determines whether system is in interactive or unattended mode
    /// based on the absence/presence of the UnattendedMode G/Technology property
    /// Returns true if the unattendedmode property is not found.
    /// </summary>
    public bool InteractiveMode
    {
      get
      {
        bool interactiveMode = true;
        IGTApplication app = GTClassFactory.Create<IGTApplication>();

        foreach(string key in app.Properties.Keys)
        {
          if(0 == string.Compare(key, "unattendedmode", true))
          {
            interactiveMode = false;
            break;
          }
        }

        return interactiveMode;
      }
    }
  }
}
