using GTechnology.Oncor.CustomAPI.Model;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class DataGridViewColumnSelector
    {
        // the DataGridView to which the DataGridViewColumnSelector is attached

        // a CheckedListBox containing the column header text and checkboxes
        public CheckedListBox mCheckedListBoxStage;
        public CheckedListBox mCheckedListBoxPrimFID;
        public CheckedListBox mCheckedListBoxSecFID;
        public CheckedListBox mCheckedListBoxLocation;
        public CheckedListBox mCheckedListBoxAbstract;
        public CheckedListBox mCheckedListBoxSectionDescription;
        public CheckedListBox mCheckedListBoxAdditionName;
        public CheckedListBox mCheckedListBoxBlockNumber;
        public CheckedListBox mCheckedListBoxLotNumber;
        public CheckedListBox mCheckedListBoxPrefix;
        public CheckedListBox mCheckedListBoxSuffix;
        public CheckedListBox mCheckedListBoxPretype;
        public CheckedListBox mCheckedListBoxType;
        public CheckedListBox mCheckedListBoxStrtCtrName;
        public CheckedListBox mCheckedListBoxOwner;
        public CheckedListBox mCheckedListBoxStrtCtrType;
        public CheckedListBox mCheckedListBoxMaterial;
        public CheckedListBox mCheckedListBoxWidth;
        public CheckedListBox mCheckedListBoxPipelnMlName;
        public CheckedListBox mCheckedListBoxName;
        public CheckedListBox mCheckedListBoxAddress;

        public CheckedListBox mCheckedListBoxAddressRange;

        // a ToolStripDropDown object used to show the popup
        public ToolStripDropDown mPopupStage;
        public ToolStripDropDown mPopupPrimFID;
        public ToolStripDropDown mPopupSecFID;
        public ToolStripDropDown mPopupName;
        public ToolStripDropDown mPopupAddress;

        public ToolStripDropDown mPopupLocation;
        public ToolStripDropDown mPopupAbstract;
        public ToolStripDropDown mPopupSectionDescription;
        public ToolStripDropDown mPopupAdditionName;
        public ToolStripDropDown mPopupBlockNumber;
        public ToolStripDropDown mPopupLotNumber;
        public ToolStripDropDown mPopupPrefix;
        public ToolStripDropDown mPopupSuffix;
        public ToolStripDropDown mPopupPretype;
        public ToolStripDropDown mPopupType;
        public ToolStripDropDown mPopupStrtCtrName;
        public ToolStripDropDown mPopupOwner;
        public ToolStripDropDown mPopupStrtCtrType;
        public ToolStripDropDown mPopupMaterial;
        public ToolStripDropDown mPopupWidth;
        public ToolStripDropDown mPopupPipelnMlName;

        public ToolStripDropDown mPopupAddressRange;

        public void LoadBldgFilterList(List<BuildingManual> Buildings)
        {
            if (Buildings != null)
            {
                mCheckedListBoxPrimFID.Items.Clear();
                mCheckedListBoxStage.Items.Clear();
                mCheckedListBoxName.Items.Clear();
                mCheckedListBoxLocation.Items.Clear();

                foreach (BuildingManual bldg in Buildings)
                {
                    if (!mCheckedListBoxPrimFID.Items.Contains(bldg.G3eFid)) { mCheckedListBoxPrimFID.Items.Add(bldg.G3eFid); }
                    if (!mCheckedListBoxStage.Items.Contains(bldg.Stage))
                    { mCheckedListBoxStage.Items.Add(bldg.Stage); }
                    if (!string.IsNullOrEmpty(bldg.Name) && !mCheckedListBoxName.Items.Contains(bldg.Name)) { mCheckedListBoxName.Items.Add(bldg.Name); }
                    if (!string.IsNullOrEmpty(bldg.Location) && !mCheckedListBoxLocation.Items.Contains(bldg.Location)) { mCheckedListBoxLocation.Items.Add(bldg.Location); }
                }
            }
        }
        public void LoadParcelFilterList(List<ParcelManual> Parcels)
        {
            if (Parcels != null)
            {

                mCheckedListBoxPrimFID.Items.Clear();
                mCheckedListBoxStage.Items.Clear();
                mCheckedListBoxName.Items.Clear();
                mCheckedListBoxAbstract.Items.Clear();
                mCheckedListBoxSectionDescription.Items.Clear();
                mCheckedListBoxLotNumber.Items.Clear();
                mCheckedListBoxAdditionName.Items.Clear();
                mCheckedListBoxAddress.Items.Clear();
                mCheckedListBoxBlockNumber.Items.Clear();

                foreach (ParcelManual prcl in Parcels)
                {
                    if (!mCheckedListBoxPrimFID.Items.Contains(prcl.G3eFid))
                    { mCheckedListBoxPrimFID.Items.Add(prcl.G3eFid); }
                    if (!mCheckedListBoxStage.Items.Contains(prcl.Stage))
                    {
                        mCheckedListBoxStage.Items.Add(prcl.Stage);
                    }
                    if (!string.IsNullOrEmpty(prcl.Name) && !mCheckedListBoxName.Items.Contains(prcl.Name))
                    {
                        mCheckedListBoxName.Items.Add(prcl.Name);
                    }
                    if (!string.IsNullOrEmpty(prcl.ParcelMLAbstract) && !mCheckedListBoxAbstract.Items.Contains(prcl.ParcelMLAbstract))
                    {
                        mCheckedListBoxAbstract.Items.Add(prcl.ParcelMLAbstract);
                    }
                    if (!string.IsNullOrEmpty(prcl.SectionDescription) && !mCheckedListBoxSectionDescription.Items.Contains(prcl.SectionDescription))
                    { mCheckedListBoxSectionDescription.Items.Add(prcl.SectionDescription); }
                    if (!string.IsNullOrEmpty(prcl.LotNumber) && !mCheckedListBoxLotNumber.Items.Contains(prcl.LotNumber))
                    { mCheckedListBoxLotNumber.Items.Add(prcl.LotNumber); }

                    if (!string.IsNullOrEmpty(prcl.AdditionName) && !mCheckedListBoxAdditionName.Items.Contains(prcl.AdditionName))
                    { mCheckedListBoxAdditionName.Items.Add(prcl.AdditionName); }
                    if (!string.IsNullOrEmpty(prcl.Address) && !mCheckedListBoxAddress.Items.Contains(prcl.Address))
                    { mCheckedListBoxAddress.Items.Add(prcl.Address); }

                    if (!string.IsNullOrEmpty(prcl.BlockNumber) && !mCheckedListBoxBlockNumber.Items.Contains(prcl.BlockNumber))
                    { mCheckedListBoxBlockNumber.Items.Add(prcl.BlockNumber); }
                }
            }
        }

        public void LoadStreetCntFilterLst(List<StreetCenterLine> StltCntrLines)
        {

            if (StltCntrLines != null)
            {
                mCheckedListBoxPrimFID.Items.Clear();
                mCheckedListBoxStage.Items.Clear();
                mCheckedListBoxStrtCtrName.Items.Clear();
                mCheckedListBoxMaterial.Items.Clear();
                mCheckedListBoxOwner.Items.Clear();
                mCheckedListBoxStrtCtrType.Items.Clear();
                mCheckedListBoxWidth.Items.Clear();

                foreach (StreetCenterLine stlt in StltCntrLines)
                {
                    if (!mCheckedListBoxPrimFID.Items.Contains(stlt.G3eFid))
                    { mCheckedListBoxPrimFID.Items.Add(stlt.G3eFid); }
                    if (!mCheckedListBoxStage.Items.Contains(stlt.Stage))
                    {
                        mCheckedListBoxStage.Items.Add(stlt.Stage);
                    }
                    if (!string.IsNullOrEmpty(stlt.Name) && !mCheckedListBoxStrtCtrName.Items.Contains(stlt.Name))
                    {
                        mCheckedListBoxStrtCtrName.Items.Add(stlt.Name);
                    }
                    if (!string.IsNullOrEmpty(stlt.Material) && !mCheckedListBoxMaterial.Items.Contains(stlt.Material))
                    { mCheckedListBoxMaterial.Items.Add(stlt.Material); }

                    if (!string.IsNullOrEmpty(stlt.Owner) && !mCheckedListBoxOwner.Items.Contains(stlt.Owner))
                    { mCheckedListBoxOwner.Items.Add(stlt.Owner); }
                    if (!string.IsNullOrEmpty(stlt.Type) && !mCheckedListBoxStrtCtrType.Items.Contains(stlt.Type))
                    { mCheckedListBoxStrtCtrType.Items.Add(stlt.Type); }

                    if (!string.IsNullOrEmpty(stlt.Width) && !mCheckedListBoxWidth.Items.Contains(stlt.Width))
                    { mCheckedListBoxWidth.Items.Add(stlt.Width); }
                }
            }
        }
        public void LoadPipeLineFilterLst(List<PipeLine> PipeLines)
        {
            if (PipeLines != null)
            {
                mCheckedListBoxPrimFID.Items.Clear();
                mCheckedListBoxStage.Items.Clear();
                mCheckedListBoxPipelnMlName.Items.Clear();
                mCheckedListBoxAddressRange.Items.Clear();
                mCheckedListBoxPrefix.Items.Clear();
                mCheckedListBoxPretype.Items.Clear();
                mCheckedListBoxType.Items.Clear();
                mCheckedListBoxSuffix.Items.Clear();

                foreach (PipeLine ppln in PipeLines)
                {
                    if (!mCheckedListBoxPrimFID.Items.Contains(ppln.G3eFid))
                    { mCheckedListBoxPrimFID.Items.Add(ppln.G3eFid); }
                    if (!mCheckedListBoxStage.Items.Contains(ppln.Stage))
                    {
                        mCheckedListBoxStage.Items.Add(ppln.Stage);
                    }
                    if (!string.IsNullOrEmpty(ppln.Name) && !mCheckedListBoxPipelnMlName.Items.Contains(ppln.Name))
                    {
                        mCheckedListBoxPipelnMlName.Items.Add(ppln.Name);
                    }

                    if (!string.IsNullOrEmpty(ppln.AddressRange) && !mCheckedListBoxAddressRange.Items.Contains(ppln.AddressRange))
                    { mCheckedListBoxAddressRange.Items.Add(ppln.AddressRange); }

                    if (!string.IsNullOrEmpty(ppln.Prefix) && !mCheckedListBoxPrefix.Items.Contains(ppln.Prefix))
                    { mCheckedListBoxPrefix.Items.Add(ppln.Prefix); }

                    if (!string.IsNullOrEmpty(ppln.PreType) && !mCheckedListBoxPretype.Items.Contains(ppln.PreType))
                    { mCheckedListBoxPretype.Items.Add(ppln.PreType); }
                    if (!string.IsNullOrEmpty(ppln.Type) && !mCheckedListBoxType.Items.Contains(ppln.Type))
                    { mCheckedListBoxType.Items.Add(ppln.Type); }

                    if (!string.IsNullOrEmpty(ppln.Suffix) && !mCheckedListBoxSuffix.Items.Contains(ppln.Suffix))
                    { mCheckedListBoxSuffix.Items.Add(ppln.Suffix); }
                }
            }
        }

        
        public DataGridViewColumnSelector()
        {


            mCheckedListBoxSecFID = new CheckedListBox();
            mCheckedListBoxSecFID.CheckOnClick = true;


            ToolStripControlHost mControlHostSecFID = new ToolStripControlHost(mCheckedListBoxSecFID);
            mControlHostSecFID.Padding = Padding.Empty;
            mControlHostSecFID.Margin = Padding.Empty;
            mControlHostSecFID.AutoSize = true;

            mPopupSecFID = new ToolStripDropDown();
            mPopupSecFID.Padding = Padding.Empty;
            mPopupSecFID.Items.Add(mControlHostSecFID);

            //-------------

            mCheckedListBoxStage = new CheckedListBox();
            mCheckedListBoxStage.CheckOnClick = true;

            ToolStripControlHost mControlHostStage = new ToolStripControlHost(mCheckedListBoxStage);
            mControlHostStage.Padding = Padding.Empty;
            mControlHostStage.Margin = Padding.Empty;
            mControlHostStage.AutoSize = true;

            mPopupStage = new ToolStripDropDown();
            mPopupStage.Padding = Padding.Empty;
            mPopupStage.Items.Add(mControlHostStage);

            //-------------
            mCheckedListBoxPrimFID = new CheckedListBox();
            mCheckedListBoxPrimFID.CheckOnClick = true;

            ToolStripControlHost mControlHostPrimFID = new ToolStripControlHost(mCheckedListBoxPrimFID);
            mControlHostPrimFID.Padding = Padding.Empty;
            mControlHostPrimFID.Margin = Padding.Empty;
            mControlHostPrimFID.AutoSize = true;

            mPopupPrimFID = new ToolStripDropDown();
            mPopupPrimFID.Padding = Padding.Empty;
            mPopupPrimFID.Items.Add(mControlHostPrimFID);

            //-------------

            mCheckedListBoxName = new CheckedListBox();
            mCheckedListBoxName.CheckOnClick = true;


            ToolStripControlHost mControlHostName = new ToolStripControlHost(mCheckedListBoxName);
            mControlHostName.Padding = Padding.Empty;
            mControlHostName.Margin = Padding.Empty;
            mControlHostName.AutoSize = true;

            mPopupName = new ToolStripDropDown();
            mPopupName.Padding = Padding.Empty;
            mPopupName.Items.Add(mControlHostName);

            //-------------

            mCheckedListBoxLocation = new CheckedListBox();
            mCheckedListBoxLocation.CheckOnClick = true;


            ToolStripControlHost mControlHostLocation = new ToolStripControlHost(mCheckedListBoxLocation);
            mControlHostLocation.Padding = Padding.Empty;
            mControlHostLocation.Margin = Padding.Empty;
            mControlHostLocation.AutoSize = true;

            mPopupLocation = new ToolStripDropDown();
            mPopupLocation.Padding = Padding.Empty;
            mPopupLocation.Items.Add(mControlHostLocation);

            //-------------

            mCheckedListBoxAddress = new CheckedListBox();
            mCheckedListBoxAddress.CheckOnClick = true;



            ToolStripControlHost mControlHostAddress = new ToolStripControlHost(mCheckedListBoxAddress);
            mControlHostAddress.Padding = Padding.Empty;
            mControlHostAddress.Margin = Padding.Empty;
            mControlHostAddress.AutoSize = true;

            mPopupAddress = new ToolStripDropDown();
            mPopupAddress.Padding = Padding.Empty;
            mPopupAddress.Items.Add(mControlHostAddress);
            //
            mCheckedListBoxAbstract = new CheckedListBox();
            mCheckedListBoxAbstract.CheckOnClick = true;



            ToolStripControlHost mControlHostAbstract = new ToolStripControlHost(mCheckedListBoxAbstract);
            mControlHostAbstract.Padding = Padding.Empty;
            mControlHostAbstract.Margin = Padding.Empty;
            mControlHostAbstract.AutoSize = true;

            mPopupAbstract = new ToolStripDropDown();
            mPopupAbstract.Padding = Padding.Empty;
            mPopupAbstract.Items.Add(mControlHostAbstract);

            //

            mCheckedListBoxSectionDescription = new CheckedListBox();
            mCheckedListBoxSectionDescription.CheckOnClick = true;

            ToolStripControlHost mControlHostSectionDescription = new ToolStripControlHost(mCheckedListBoxSectionDescription);
            mControlHostSectionDescription.Padding = Padding.Empty;
            mControlHostSectionDescription.Margin = Padding.Empty;
            mControlHostSectionDescription.AutoSize = true;

            mPopupSectionDescription = new ToolStripDropDown();
            mPopupSectionDescription.Padding = Padding.Empty;
            mPopupSectionDescription.Items.Add(mControlHostSectionDescription);


            //

            mCheckedListBoxAdditionName = new CheckedListBox();
            mCheckedListBoxAdditionName.CheckOnClick = true;



            ToolStripControlHost mControlHostAdditionName = new ToolStripControlHost(mCheckedListBoxAdditionName);
            mControlHostAdditionName.Padding = Padding.Empty;
            mControlHostAdditionName.Margin = Padding.Empty;
            mControlHostAdditionName.AutoSize = true;

            mPopupAdditionName = new ToolStripDropDown();
            mPopupAdditionName.Padding = Padding.Empty;
            mPopupAdditionName.Items.Add(mControlHostAdditionName);

            //

            mCheckedListBoxBlockNumber = new CheckedListBox();
            mCheckedListBoxBlockNumber.CheckOnClick = true;


            ToolStripControlHost mControlHostBlockNumber = new ToolStripControlHost(mCheckedListBoxBlockNumber);
            mControlHostBlockNumber.Padding = Padding.Empty;
            mControlHostBlockNumber.Margin = Padding.Empty;
            mControlHostBlockNumber.AutoSize = true;

            mPopupBlockNumber = new ToolStripDropDown();
            mPopupBlockNumber.Padding = Padding.Empty;
            mPopupBlockNumber.Items.Add(mControlHostBlockNumber);

            //

            mCheckedListBoxLotNumber = new CheckedListBox();
            mCheckedListBoxLotNumber.CheckOnClick = true;




            ToolStripControlHost mControlHostLotNumber = new ToolStripControlHost(mCheckedListBoxLotNumber);
            mControlHostLotNumber.Padding = Padding.Empty;
            mControlHostLotNumber.Margin = Padding.Empty;
            mControlHostLotNumber.AutoSize = true;

            mPopupLotNumber = new ToolStripDropDown();
            mPopupLotNumber.Padding = Padding.Empty;
            mPopupLotNumber.Items.Add(mControlHostLotNumber);

            //

            mCheckedListBoxPrefix = new CheckedListBox();
            mCheckedListBoxPrefix.CheckOnClick = true;



            ToolStripControlHost mControlHostPrefix = new ToolStripControlHost(mCheckedListBoxPrefix);
            mControlHostPrefix.Padding = Padding.Empty;
            mControlHostPrefix.Margin = Padding.Empty;
            mControlHostPrefix.AutoSize = true;

            mPopupPrefix = new ToolStripDropDown();
            mPopupPrefix.Padding = Padding.Empty;
            mPopupPrefix.Items.Add(mControlHostPrefix);

            //

            mCheckedListBoxSuffix = new CheckedListBox();
            mCheckedListBoxSuffix.CheckOnClick = true;



            ToolStripControlHost mControlHostSuffix = new ToolStripControlHost(mCheckedListBoxSuffix);
            mControlHostSuffix.Padding = Padding.Empty;
            mControlHostSuffix.Margin = Padding.Empty;
            mControlHostSuffix.AutoSize = true;

            mPopupSuffix = new ToolStripDropDown();
            mPopupSuffix.Padding = Padding.Empty;
            mPopupSuffix.Items.Add(mControlHostSuffix);


            //

            mCheckedListBoxPretype = new CheckedListBox();
            mCheckedListBoxPretype.CheckOnClick = true;


            ToolStripControlHost mControlHostPretype = new ToolStripControlHost(mCheckedListBoxPretype);
            mControlHostPretype.Padding = Padding.Empty;
            mControlHostPretype.Margin = Padding.Empty;
            mControlHostPretype.AutoSize = true;

            mPopupPretype = new ToolStripDropDown();
            mPopupPretype.Padding = Padding.Empty;
            mPopupPretype.Items.Add(mControlHostPretype);

            //

            mCheckedListBoxType = new CheckedListBox();
            mCheckedListBoxType.CheckOnClick = true;



            ToolStripControlHost mControlHostType = new ToolStripControlHost(mCheckedListBoxType);
            mControlHostType.Padding = Padding.Empty;
            mControlHostType.Margin = Padding.Empty;
            mControlHostType.AutoSize = true;

            mPopupType = new ToolStripDropDown();
            mPopupType.Padding = Padding.Empty;
            mPopupType.Items.Add(mControlHostType);

            //

            mCheckedListBoxStrtCtrName = new CheckedListBox();
            mCheckedListBoxStrtCtrName.CheckOnClick = true;



            ToolStripControlHost mControlHostStrtCtrName = new ToolStripControlHost(mCheckedListBoxStrtCtrName);
            mControlHostStrtCtrName.Padding = Padding.Empty;
            mControlHostStrtCtrName.Margin = Padding.Empty;
            mControlHostStrtCtrName.AutoSize = true;

            mPopupStrtCtrName = new ToolStripDropDown();
            mPopupStrtCtrName.Padding = Padding.Empty;
            mPopupStrtCtrName.Items.Add(mControlHostStrtCtrName);

            //

            mCheckedListBoxOwner = new CheckedListBox();
            mCheckedListBoxOwner.CheckOnClick = true;


            ToolStripControlHost mControlHostOwner = new ToolStripControlHost(mCheckedListBoxOwner);
            mControlHostOwner.Padding = Padding.Empty;
            mControlHostOwner.Margin = Padding.Empty;
            mControlHostOwner.AutoSize = true;

            mPopupOwner = new ToolStripDropDown();
            mPopupOwner.Padding = Padding.Empty;
            mPopupOwner.Items.Add(mControlHostOwner);

            //

            mCheckedListBoxStrtCtrType = new CheckedListBox();
            mCheckedListBoxStrtCtrType.CheckOnClick = true;



            ToolStripControlHost mControlHostStrtCtrType = new ToolStripControlHost(mCheckedListBoxStrtCtrType);
            mControlHostStrtCtrType.Padding = Padding.Empty;
            mControlHostStrtCtrType.Margin = Padding.Empty;
            mControlHostStrtCtrType.AutoSize = true;

            mPopupStrtCtrType = new ToolStripDropDown();
            mPopupStrtCtrType.Padding = Padding.Empty;
            mPopupStrtCtrType.Items.Add(mControlHostStrtCtrType);

            //

            mCheckedListBoxMaterial = new CheckedListBox();
            mCheckedListBoxMaterial.CheckOnClick = true;

            ToolStripControlHost mControlHostMaterial = new ToolStripControlHost(mCheckedListBoxMaterial);
            mControlHostMaterial.Padding = Padding.Empty;
            mControlHostMaterial.Margin = Padding.Empty;
            mControlHostMaterial.AutoSize = true;

            mPopupMaterial = new ToolStripDropDown();
            mPopupMaterial.Padding = Padding.Empty;
            mPopupMaterial.Items.Add(mControlHostMaterial);

            //

            mCheckedListBoxWidth = new CheckedListBox();
            mCheckedListBoxWidth.CheckOnClick = true;


            ToolStripControlHost mControlHostWidth = new ToolStripControlHost(mCheckedListBoxWidth);
            mControlHostWidth.Padding = Padding.Empty;
            mControlHostWidth.Margin = Padding.Empty;
            mControlHostWidth.AutoSize = true;

            mPopupWidth = new ToolStripDropDown();
            mPopupWidth.Padding = Padding.Empty;
            mPopupWidth.Items.Add(mControlHostWidth);

            //

            mCheckedListBoxPipelnMlName = new CheckedListBox();
            mCheckedListBoxPipelnMlName.CheckOnClick = true;


            ToolStripControlHost mControlHostPipelnMlName = new ToolStripControlHost(mCheckedListBoxPipelnMlName);
            mControlHostPipelnMlName.Padding = Padding.Empty;
            mControlHostPipelnMlName.Margin = Padding.Empty;
            mControlHostPipelnMlName.AutoSize = true;

            mPopupPipelnMlName = new ToolStripDropDown();
            mPopupPipelnMlName.Padding = Padding.Empty;
            mPopupPipelnMlName.Items.Add(mControlHostPipelnMlName);

            //

            mCheckedListBoxAddressRange = new CheckedListBox();
            mCheckedListBoxAddressRange.CheckOnClick = true;


            ToolStripControlHost mControlHostAddressRange = new ToolStripControlHost(mCheckedListBoxAddressRange);
            mControlHostAddressRange.Padding = Padding.Empty;
            mControlHostAddressRange.Margin = Padding.Empty;
            mControlHostAddressRange.AutoSize = true;

            mPopupAddressRange = new ToolStripDropDown();
            mPopupAddressRange.Padding = Padding.Empty;
            mPopupAddressRange.Items.Add(mControlHostAddressRange);

        }
    }
}
