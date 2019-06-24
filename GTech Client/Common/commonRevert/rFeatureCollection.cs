namespace GTechnology.Oncor.CustomAPI
{
	public class rFeatureCollection
	{
		short r_fno;
		int r_fid;
		string r_fState;

		short r_wpfno;
		int r_wpfid;

		public short RFno
		{
			get { return r_fno; }
			set { r_fno = value; }
		}

		public int RFid
		{
			get { return r_fid; }
			set { r_fid = value; }
		}

		public string RFstate
		{
			get { return r_fState; }
			set { r_fState = value; }
		}

		public short RWPFno
		{
			get { return r_wpfno; }
			set { r_wpfno = value; }
		}

		public int RWPFid
		{
			get { return r_wpfid; }
			set { r_wpfid = value; }
		}
	}
}
