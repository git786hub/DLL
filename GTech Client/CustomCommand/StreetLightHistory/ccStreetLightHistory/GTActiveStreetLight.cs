using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI
{
  public class GTActiveStreetLight
  {
    #region Properties

    private int _fid;
    public int G3E_FID
    {
      get { return _fid; }
      set { _fid = value; }
    }

    private short _fno;
    public short G3E_FNO
    {
      get { return _fno; }
      set { _fno = value; }
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

    private int _repfid;
    public int REPLACED_FID
    {
      get { return _repfid; }
      set { _repfid = value; }
    }

    
    #region Structure Properties

    private int _structurefid;
    public int Structure_FID
    {
      get { return _structurefid; }
      set { _structurefid = value; }
    }

    private short _structurefno;
    public short StructureFNO
    {
      get { return _structurefno; }
      set { _structurefno = value; }
    }

    private string _structureID;
    public string StructureID
    {
      get { return _structureID; }
      set { _structureID = value; }
    }

    private double _structureoggX1;
    public double StructureOGG_X1
    {
      get { return _structureoggX1; }
      set { _structureoggX1 = value; }
    }

    private double _structureoggy1;
    public double StructureOGG_Y1
    {
      get { return _structureoggy1; }
      set { _structureoggy1 = value; }
    }

    private double _structureoggz1;
    public double StructureOGG_Z1
    {
      get { return _structureoggz1; }
      set { _structureoggz1 = value; }
    }

    #endregion Structure Properties


    #region HistoricalTracks Properties

    private List<AssetHistory> _geomSourceList;
    public List<AssetHistory> GeometrySource
    {
      get { return _geomSourceList; }
      set { _geomSourceList = value; }
    }

    private Dictionary<string, List<AssetHistory>> _sidHistory;
    public Dictionary<string, List<AssetHistory>> HistoryOfSID
    {
      get { return _sidHistory; }
      set { _sidHistory = value; }
    }

    #endregion HistoricalTracks Properties

    #endregion Properties
    public GTActiveStreetLight()
    {
      _geomSourceList = new List<AssetHistory>();
      _sidHistory = new Dictionary<string, List<AssetHistory>>();
    }
  }
}
