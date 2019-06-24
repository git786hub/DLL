using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI
{
  public class GTSysGenParam
  {
    public GTSysGenParam()
    {
      iMovementThresholdGP = 0;
      iHistoricalSymbolGP = 0;
      iHistoricalLineGP = 0;
      iHistoricalStackingGP = 0;
    }

    #region Properties

    private int iMovementThresholdGP;
    public int MovementThreshold
    {
      get { return iMovementThresholdGP; }
      set { iMovementThresholdGP = value; }
    }

    private int iHistoricalSymbolGP;
    public int HistoricalSymbol
    {
      get { return iHistoricalSymbolGP; }
      set { iHistoricalSymbolGP = value; }
    }

    private int iHistoricalLineGP;
    public int HistoricalLine
    {
      get { return iHistoricalLineGP; }
      set { iHistoricalLineGP = value; }
    }

    private int iHistoricalStackingGP;
    public int HistoricalStacking
    {
      get { return iHistoricalStackingGP; }
      set { iHistoricalStackingGP = value; }
    }

    private int _anoX;
    public int ANO_X
    {
      get { return _anoX; }
      set { _anoX = value; }
    }
    private int _anoY;
    public int ANO_Y
    {
      get { return _anoY; }
      set { _anoY = value; }
    }

    private int _anoZ;
    public int ANO_Z
    {
      get { return _anoZ; }
      set { _anoZ = value; }
    }

    #endregion Properties
  }
}
