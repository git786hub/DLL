using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI
{
  public class AssetHistory
  {
    #region Properties

    #region Properties

    private int _g3eIdentifier;
    public int G3E_Identifier
    {
      get { return _g3eIdentifier; }
      set { _g3eIdentifier = value; }
    }

    private short _g3eFNO;
    public short G3E_FNO
    {
      get { return _g3eFNO; }
      set { _g3eFNO = value; }
    }

    private int _g3eFID;
    public int G3E_FID
    {
      get { return _g3eFID; }
      set { _g3eFID = value; }
    }

    private short _g3eCNO;
    public short G3E_CNO
    {
      get { return _g3eCNO; }
      set { _g3eCNO = value; }
    }

    private int _g3eCID;
    public int G3E_CID
    {
      get { return _g3eCID; }
      set { _g3eCID = value; }
    }

    private int _g3eANO;
    public int G3E_ANO
    {
      get { return _g3eANO; }
      set { _g3eANO = value; }
    }

    private string _changeOperation;
    public string ChangeOperation
    {
      get { return _changeOperation; }
      set { _changeOperation = value; }
    }

    private string _structureID1;
    public string StructureID_1
    {
      get { return _structureID1; }
      set { _structureID1 = value; }
    }

    private double _oggX1;
    public double OGG_X1
    {
      get { return _oggX1; }
      set { _oggX1 = value; }
    }

    private double _oggy1;
    public double OGG_Y1
    {
      get { return _oggy1; }
      set { _oggy1 = value; }
    }

    private double _oggz1;
    public double OGG_Z1
    {
      get { return _oggz1; }
      set { _oggz1 = value; }
    }

    private string _oldvalue;
    public string OLD_VALUE
    {
      get { return _oldvalue; }
      set { _oldvalue = value; }
    }

    private string _newvalue;
    public string NEW_VALUE
    {
      get { return _newvalue; }
      set { _newvalue = value; }
    }
    private DateTime _changeDate;
    public DateTime ChangeDate
    {
      get { return _changeDate; }
      set { _changeDate = value; }
    }
    private string _changeUID;
    public string ChangeUID
    {
      get { return _changeUID; }
      set { _changeUID = value; }
    }

    #endregion Properties

    #endregion Properties

    public AssetHistory()
    {

    }
  }
}
