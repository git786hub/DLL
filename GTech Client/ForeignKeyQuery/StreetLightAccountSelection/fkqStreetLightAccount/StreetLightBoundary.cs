
namespace GTechnology.Oncor.CustomAPI
{
    public class StreetLightBoundary
    {
        int bnd_class;
        short bnd_fno;
        string g3eTable;
        string g3eField;
        string fieldValue;

        public int BoundaryClass
        {
            get { return bnd_class; }
            set { bnd_class = value; }
        }
        public short BoundaryFno
        {
            get { return bnd_fno; }
            set { bnd_fno = value; }
        }
        public string G3eTable
        {
            get { return g3eTable; }
            set { g3eTable = value; }
        }
        public string G3eField
        {
            get { return g3eField; }
            set { g3eField = value; }
        }
        public string FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
    }
}
