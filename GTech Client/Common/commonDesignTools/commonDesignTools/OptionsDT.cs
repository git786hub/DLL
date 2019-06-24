using System;
using System.Drawing;
using System.Windows.Forms;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class OptionsDT : Form
    {
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private bool m_OutputToMapWindow;
        private bool m_ActiveMapWindowOnly;
        private bool m_OverrideStyle;
        private double m_LineWeight;
        private Color m_TraceColor;
        private Color m_FillColor;
        private Color m_ProblemColor;

        private bool m_OverrideMapWindowSettings;
        private short m_ZoomOption;
        private double m_ZoomFactor;
        private bool m_NotifyChanges;

        const double M_WEIGHT_TO_PT = 35.2778;

        const string M_PROP_OUTPUT_TO_MAP_WINDOW = "DesignToolOptions_OutputToMapWindow";
        const string M_PROP_ACTIVE_WINDOW_ONLY = "DesignToolOptions_ActiveWindowOnly";
        const string M_PROP_OVERRIDE_STYLE = "DesignToolOptions_OverrideStyle";
        const string M_PROP_LINE_WEIGHT = "DesignToolOptions_LineWeight";
        const string M_PROP_TRACE_COLOR = "DesignToolOptions_TraceColor";
        const string M_PROP_FILL_COLOR = "DesignToolOptions_FillColor";
        const string M_PROP_PROBLEM_COLOR = "DesignToolOptions_ProblemColor";

        const string M_PROP_OVERRIDE_MAP_SETTINGS = "DesignToolOptions_OverrideMapSettings";
        const string M_PROP_ZOOM_OPTION = "DesignToolOptions_ZoomOption";
        const string M_PROP_ZOOM_FACTOR = "DesignToolOptions_ZoomFactor";
        const string M_PROP_NOTIFY_CHANGES = "DesignToolOptions_NotifyChanges";

        public bool OutputToMapWindow
        {
            get
            {
                return m_OutputToMapWindow;
            }
            set
            {
                m_OutputToMapWindow = value;
            }
        }

        public bool ActiveWindowOnly
        {
            get
            {
                return m_ActiveMapWindowOnly;
            }
            set
            {
                m_ActiveMapWindowOnly = value;
            }
        }

        public bool OverrideStyle
        {
            get
            {
                return m_OverrideStyle;
            }
            set
            {
                m_OverrideStyle = value;
            }
        }

        public double LineWeight
        {
            get
            {
                return m_LineWeight;
            }
            set
            {
                m_LineWeight = value;
            }
        }

        public Color TraceColor
        {
            get
            {
                return m_TraceColor;
            }
            set
            {
                m_TraceColor = value;
            }
        }

        public Color FillColor
        {
            get
            {
                return m_FillColor;
            }
            set
            {
                m_FillColor = value;
            }
        }

        public Color ProblemColor
        {
            get
            {
                return m_ProblemColor;
            }
            set
            {
                m_ProblemColor = value;
            }
        }

        public bool OverrideMapWindowSettings
        {
            get
            {
                return m_OverrideMapWindowSettings;
            }
            set
            {
                m_OverrideMapWindowSettings = value;
            }
        }

        public short ZoomOption
        {
            get
            {
                return m_ZoomOption;
            }
            set
            {
                m_ZoomOption = value;
            }
        }

        public double ZoomFactor
        {
            get
            {
                return m_ZoomFactor;
            }
            set
            {
                m_ZoomFactor = value;
            }
        }

        public bool NotifyChanges
        {
            get
            {
                return m_NotifyChanges;
            }
            set
            {
                m_NotifyChanges = value;
            }
        }

        public OptionsDT()
        {
            InitializeComponent();

            // Check if dialog has previously been loaded in the G/Tech session.
            // If so, then use previous settings.
            InitializeForm();
        }

        private bool InitializeForm()
        {
            object propertyValue;
            bool returnValue = false;
            CommonDT commonDT = new CommonDT();

            try
            {
                if (commonDT.CheckIfPropertyExists(M_PROP_OUTPUT_TO_MAP_WINDOW, out propertyValue))
                {
                    chkOutputMapWindow.Checked = Convert.ToBoolean(propertyValue);
                    m_OutputToMapWindow = chkOutputMapWindow.Checked;
                }
                else
                {
                    m_OutputToMapWindow = chkOutputMapWindow.Checked;
                    commonDT.AddProperty(M_PROP_OUTPUT_TO_MAP_WINDOW, m_OutputToMapWindow);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_ACTIVE_WINDOW_ONLY, out propertyValue))
                {
                    optActiveWindow.Checked = Convert.ToBoolean(propertyValue);
                    optAllWindows.Checked = !Convert.ToBoolean(propertyValue);
                    m_ActiveMapWindowOnly = optActiveWindow.Checked;
                }
                else
                {
                    m_ActiveMapWindowOnly = optActiveWindow.Checked;
                    commonDT.AddProperty(M_PROP_ACTIVE_WINDOW_ONLY, m_ActiveMapWindowOnly);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_OVERRIDE_STYLE, out propertyValue))
                {
                    chkOverrideStyle.Checked = Convert.ToBoolean(propertyValue);
                    m_OverrideStyle = chkOverrideStyle.Checked;
                }
                else
                {
                    m_OverrideStyle = chkOverrideStyle.Checked;
                    commonDT.AddProperty(M_PROP_OVERRIDE_STYLE, m_OverrideStyle);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_LINE_WEIGHT, out propertyValue))
                {
                    cboLineweight.Weight = Convert.ToDouble(propertyValue) * M_WEIGHT_TO_PT;
                    m_LineWeight = cboLineweight.Weight / M_WEIGHT_TO_PT;
                }
                else
                {
                    cboLineweight.Weight = M_WEIGHT_TO_PT;
                    m_LineWeight = cboLineweight.Weight / M_WEIGHT_TO_PT;
                    commonDT.AddProperty(M_PROP_LINE_WEIGHT, m_LineWeight);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_TRACE_COLOR, out propertyValue))
                {
                    int [] rgb = (int[])propertyValue;
                    GMTraceColorButton.Color = Color.FromArgb(rgb[0], rgb[1], rgb[2]);
                    m_TraceColor = GMTraceColorButton.Color;
                }
                else
                {
                    m_TraceColor = GMTraceColorButton.Color;
                    int[] rgb = { m_TraceColor.R, m_TraceColor.G, m_TraceColor.B };
                    commonDT.AddProperty(M_PROP_TRACE_COLOR, rgb);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_FILL_COLOR, out propertyValue))
                {
                    int[] rgb = (int[])propertyValue;
                    GMFillColorButton.Color = Color.FromArgb(rgb[0], rgb[1], rgb[2]);
                    m_FillColor = GMFillColorButton.Color;
                }
                else
                {
                    m_FillColor = GMFillColorButton.Color;
                    int[] rgb = { m_FillColor.R, m_FillColor.G, m_FillColor.B };
                    commonDT.AddProperty(M_PROP_FILL_COLOR, rgb);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_PROBLEM_COLOR, out propertyValue))
                {
                    int[] rgb = (int[])propertyValue;
                    GMProblemColorButton.Color = Color.FromArgb(rgb[0], rgb[1], rgb[2]);
                    m_ProblemColor = GMProblemColorButton.Color;
                }
                else
                {
                    m_ProblemColor = GMProblemColorButton.Color;
                    int[] rgb = { m_ProblemColor.R, m_ProblemColor.G, m_ProblemColor.B };
                    commonDT.AddProperty(M_PROP_PROBLEM_COLOR, rgb);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_OVERRIDE_MAP_SETTINGS, out propertyValue))
                {
                    chkOverrideMapWindowSettings.Checked = Convert.ToBoolean(propertyValue);
                    m_OverrideMapWindowSettings = chkOverrideMapWindowSettings.Checked;
                }
                else
                {
                    m_OverrideMapWindowSettings = chkOverrideMapWindowSettings.Checked;
                    commonDT.AddProperty(M_PROP_OVERRIDE_MAP_SETTINGS, m_OverrideMapWindowSettings);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_ZOOM_OPTION, out propertyValue))
                {
                    if (Convert.ToInt16(propertyValue) == 1)
                    {
                        optViewAtCurrentScale.Checked = true;
                        m_ZoomOption = 1;
                    }
                    else if(Convert.ToInt16(propertyValue) == 2)
                    {
                        optCenterAtCurrentScale.Checked = true;
                        m_ZoomOption = 2;
                    }
                    else
                    {
                        optFitAndZoomOut.Checked = true;
                        m_ZoomOption = 3;
                    }
                }
                else
                {
                    if (optViewAtCurrentScale.Checked)
                    {
                        m_ZoomOption = 1;
                    }
                    else if (optCenterAtCurrentScale.Checked)
                    {
                        m_ZoomOption = 2;
                    }
                    else
                    {
                        m_ZoomOption = 3;
                    }

                    commonDT.AddProperty(M_PROP_ZOOM_OPTION, m_ZoomOption);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_ZOOM_FACTOR, out propertyValue))
                {
                    txtZoom.Text = Convert.ToString(Convert.ToDouble(propertyValue) * 100);
                    m_ZoomFactor = Convert.ToDouble(txtZoom.Text);
                }
                else
                {
                    m_ZoomFactor = Convert.ToDouble(txtZoom.Text) / 100;
                    commonDT.AddProperty(M_PROP_ZOOM_FACTOR, m_ZoomFactor);
                }

                if (commonDT.CheckIfPropertyExists(M_PROP_NOTIFY_CHANGES, out propertyValue))
                {
                    chkNotifyChanges.Checked = Convert.ToBoolean(propertyValue);
                    m_NotifyChanges = chkNotifyChanges.Checked;
                }
                else
                {
                    m_NotifyChanges = chkNotifyChanges.Checked;
                    commonDT.AddProperty(M_PROP_NOTIFY_CHANGES, m_NotifyChanges);
                }

                cmdApply.Enabled = false;
            }
            catch
            {
                // Ignore error
            }
            finally
            {
                commonDT = null;
            }

            return returnValue;
        }        

        private void chkOutputMapWindow_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;

            if (!chkOutputMapWindow.Checked)
            {
                optActiveWindow.Enabled = false;
                optAllWindows.Enabled = false;
                chkOverrideStyle.Enabled = false;
                cboLineweight.Enabled = false;
                lblLineWeight.Enabled = false;
                lblTraceColor.Enabled = false;
                lblFillcolor.Enabled = false;
                lblProblemColor.Enabled = false;
                GMTraceColorButton.Enabled = false;
                GMFillColorButton.Enabled = false;
                GMProblemColorButton.Enabled = false;
            }
            else
            {
                optActiveWindow.Enabled = true;
                optAllWindows.Enabled = true;
                chkOverrideStyle.Enabled = true;

                if (chkOverrideStyle.Checked)
                {
                    cboLineweight.Enabled = true;
                    lblLineWeight.Enabled = true;
                    lblTraceColor.Enabled = true;
                    lblFillcolor.Enabled = true;
                    lblProblemColor.Enabled = true;
                    GMTraceColorButton.Enabled = true;
                    GMFillColorButton.Enabled = true;
                    GMProblemColorButton.Enabled = true;
                }
                else
                {
                    cboLineweight.Enabled = false;
                    lblLineWeight.Enabled = false;
                    lblTraceColor.Enabled = false;
                    lblFillcolor.Enabled = false;
                    lblProblemColor.Enabled = false;
                    GMTraceColorButton.Enabled = false;
                    GMFillColorButton.Enabled = false;
                    GMProblemColorButton.Enabled = false;
                }
            }
        }

        private void chkOverrideStyle_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;

            if (chkOverrideStyle.Checked == false)
            {
                cboLineweight.Enabled = false;
                lblLineWeight.Enabled = false;
                lblTraceColor.Enabled = false;
                lblFillcolor.Enabled = false;
                lblProblemColor.Enabled = false;
                GMTraceColorButton.Enabled = false;
                GMFillColorButton.Enabled = false;
                GMProblemColorButton.Enabled = false;
            }
            else
            {
                if (chkOutputMapWindow.Checked == true)
                {
                    cboLineweight.Enabled = true;
                    lblLineWeight.Enabled = true;
                    lblTraceColor.Enabled = true;
                    lblFillcolor.Enabled = true;
                    lblProblemColor.Enabled = true;
                    GMTraceColorButton.Enabled = true;
                    GMFillColorButton.Enabled = true;
                    GMProblemColorButton.Enabled = true;
                }
                else
                {
                    cboLineweight.Enabled = false;
                    lblLineWeight.Enabled = false;
                    lblTraceColor.Enabled = false;
                    lblFillcolor.Enabled = false;
                    lblProblemColor.Enabled = false;
                    GMTraceColorButton.Enabled = false;
                    GMFillColorButton.Enabled = false;
                    GMProblemColorButton.Enabled = false;
                }
            }
        }

        private void chkOverrideMapWindowSettings_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;

            if (!chkOverrideMapWindowSettings.Checked)
            {
                fraMapWindowSettings.Enabled = false;
                optViewAtCurrentScale.Enabled = false;
                optCenterAtCurrentScale.Enabled = false;
                optFitAndZoomOut.Enabled = false;
                txtZoom.Enabled = false;
                spnZoom.Enabled = false;
            }
            else
            {
                fraMapWindowSettings.Enabled = true;
                optViewAtCurrentScale.Enabled = true;
                optCenterAtCurrentScale.Enabled = true;
                optFitAndZoomOut.Enabled = true;
                if (optFitAndZoomOut.Checked == true)
                {
                    txtZoom.Enabled = true;
                    spnZoom.Enabled = true;
                }
                else
                {
                    txtZoom.Enabled = false;
                    spnZoom.Enabled = false;
                }
            }
        }

        private void optViewatcurrentscale_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;

            if (optViewAtCurrentScale.Checked)
            {
                txtZoom.Enabled = false;
                spnZoom.Enabled = false;
            }
        }

        private void optCentreatcurrentscale_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;

            if (optCenterAtCurrentScale.Checked)
            {
                txtZoom.Enabled = false;
                spnZoom.Enabled = false;
            }
        }

        private void optFitandzoomout_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;

            if (optFitAndZoomOut.Checked)
            {
                if (chkOverrideMapWindowSettings.Checked)
                {
                    txtZoom.Enabled = true;
                    spnZoom.Enabled = true;
                }
                else
                {
                    txtZoom.Enabled = false;
                    spnZoom.Enabled = false;
                }
            }
        }

        private void txtZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 48 | e.KeyChar > 57)
            {
                e.Handled = true;
            }
        }

        private void spnZoom_Scroll(object sender, ScrollEventArgs e)
        {
            cmdApply.Enabled = true;

            short zoomFactor = Convert.ToInt16(txtZoom.Text);

            if (e.Type == ScrollEventType.SmallIncrement)
            {
                zoomFactor++;
            }
            else if (e.Type == ScrollEventType.SmallDecrement)
            {
                zoomFactor--;
            }

            txtZoom.Text = Convert.ToString(zoomFactor);
            m_ZoomFactor = Convert.ToDouble(zoomFactor) / 100;
        }

        private void cmdApply_Click(object sender, EventArgs e)
        {
            // Save settings
            m_OutputToMapWindow = chkOutputMapWindow.Checked;
            m_ActiveMapWindowOnly = optActiveWindow.Checked;
            m_OverrideStyle = chkOverrideStyle.Checked;
            m_LineWeight = cboLineweight.Weight / M_WEIGHT_TO_PT;
            m_TraceColor = GMTraceColorButton.Color;
            m_FillColor = GMFillColorButton.Color;
            m_ProblemColor = GMProblemColorButton.Color;

            m_OverrideMapWindowSettings = chkOverrideMapWindowSettings.Checked;

            if (optViewAtCurrentScale.Checked)
            {
                m_ZoomOption = 1;
            }
            else if(optCenterAtCurrentScale.Checked)
            {
                m_ZoomOption = 2;
            }
            else
            {
                m_ZoomOption = 3;
            }

            if (!(txtZoom.Text.Trim().Length == 0))
            {
                m_ZoomFactor = Convert.ToDouble(txtZoom.Text) / 100;
            }

            m_NotifyChanges = chkNotifyChanges.Checked;

            CommonDT commonDT = new CommonDT();

            // Add Properties so settings will persist in current G/Tech session
            commonDT.AddProperty(M_PROP_OUTPUT_TO_MAP_WINDOW, m_OutputToMapWindow);
            commonDT.AddProperty(M_PROP_ACTIVE_WINDOW_ONLY, m_ActiveMapWindowOnly);
            commonDT.AddProperty(M_PROP_OVERRIDE_STYLE, m_OverrideStyle);
            commonDT.AddProperty(M_PROP_LINE_WEIGHT, m_LineWeight);
            int[] rgb = { m_TraceColor.R, m_TraceColor.G, m_TraceColor.B };
            commonDT.AddProperty(M_PROP_TRACE_COLOR, rgb);
            rgb[0] = m_FillColor.R;
            rgb[1] = m_FillColor.G;
            rgb[2] = m_FillColor.B;
            commonDT.AddProperty(M_PROP_FILL_COLOR, rgb);
            rgb[0] = m_ProblemColor.R;
            rgb[1] = m_ProblemColor.G;
            rgb[2] = m_ProblemColor.B;
            commonDT.AddProperty(M_PROP_PROBLEM_COLOR, rgb);

            commonDT.AddProperty(M_PROP_OVERRIDE_MAP_SETTINGS, m_OverrideMapWindowSettings);
            commonDT.AddProperty(M_PROP_ZOOM_OPTION, m_ZoomOption);
            commonDT.AddProperty(M_PROP_ZOOM_FACTOR, m_ZoomFactor);
            commonDT.AddProperty(M_PROP_NOTIFY_CHANGES, m_NotifyChanges);

            commonDT = null;

            cmdApply.Enabled = false;
        }        

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optActiveWindow_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;
        }

        private void optAllWindows_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;
        }

        private void cboLineweight_WeightChanged(object sender, AxSTYCTLLib._DWeightEvents_WeightChangedEvent e)
        {
            cmdApply.Enabled = true;
        }

        private void GMTraceColorButton_ColorChanged(object sender, AxSTYCTLLib._DColorBtnEvents_ColorChangedEvent e)
        {
            cmdApply.Enabled = true;
        }

        private void GMFillColorButton_ColorChanged(object sender, AxSTYCTLLib._DColorBtnEvents_ColorChangedEvent e)
        {
            cmdApply.Enabled = true;
        }

        private void GMProblemColorButton_ColorChanged(object sender, AxSTYCTLLib._DColorBtnEvents_ColorChangedEvent e)
        {
            cmdApply.Enabled = true;
        }

        private void chkNotifyChanges_CheckedChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;
        }

        private void txtZoom_TextChanged(object sender, EventArgs e)
        {
            cmdApply.Enabled = true;
        }
    }
}
