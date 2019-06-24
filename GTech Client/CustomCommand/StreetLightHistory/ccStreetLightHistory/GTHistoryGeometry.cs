using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
  public class GTHistoryGeometry
  {
    public GTHistoryGeometry(double _x, double _y, double _z)
    {
      _point = GTClassFactory.Create<IGTOrientedPointGeometry>();
      IGTPoint apoint = GTClassFactory.Create<IGTPoint>();
      apoint.X = _x;
      apoint.Y = _y;
      apoint.Z = _z;

      _point.Origin = apoint;
    }

    private IGTOrientedPointGeometry _point;
    public IGTOrientedPointGeometry GeomPoint
    {
      get { return _point; }
      set { _point = value; }
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
  }
}
