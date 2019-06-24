using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI
{
  public class StreetLightHistoryTrack
  {
    private List<AssetHistory> _structureTracks;
    public List<AssetHistory> OwnedByHistoryTracks
    {
      get { return _structureTracks; }
      set { _structureTracks = value; }
    }

    private List<AssetHistory> _tracks;
    public List<AssetHistory> TrackList
    {
      get { return _tracks; }
      set { _tracks = value; }
    }

    private List<AssetHistory> _locationsForGeometries;
    public List<AssetHistory> GeometryLocations
    {
      get { return _locationsForGeometries; }
      set { _locationsForGeometries = value; }
    }
    public StreetLightHistoryTrack()
    {
      _structureTracks = new List<AssetHistory>();
      _tracks = new List<AssetHistory>();
      _locationsForGeometries = new List<AssetHistory>();
    }
  }
}
