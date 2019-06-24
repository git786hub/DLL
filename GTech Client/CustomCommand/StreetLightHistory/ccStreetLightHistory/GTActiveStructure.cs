using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
  public class GTActiveStructure
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

    private int _structurefid;
    public int Structure_FID
    {
      get { return _structurefid; }
      set { _structurefid = value; }
    }

    private List<IGTKeyObject> _koStreetlights;
    public List<IGTKeyObject> StreetlightKOList
    {
      get { return _koStreetlights; }
      set { _koStreetlights = value; }
    }
    #endregion Properties

    public GTActiveStructure()
    {
      _koStreetlights = new List<IGTKeyObject>();
    }
  }
}
