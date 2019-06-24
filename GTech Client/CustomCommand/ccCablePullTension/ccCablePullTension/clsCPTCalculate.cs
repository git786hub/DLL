using System;
using System.ComponentModel;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using System.Drawing;

namespace GTechnology.Oncor.CustomAPI
{
    public class clsCPTCalculate
    {
        private string m_CableConfiguration = string.Empty;
        private double m_CableDiameter = 0;
        private double m_LowCOF = 0;
        private double m_HighCOF = 0;
        private double m_DuctDiameter = 0;
        private double m_DuctDiameterNominal = 0;
        private double m_StandardBendRadius = 0;
        private double m_CableClearance = 0;
        private double m_MinimumClearance = 0;
        private string m_JamRatio = string.Empty;
        private string m_Phases = string.Empty;
        private double m_WeightPerFoot = 0;
        private double m_WeightCorrectionFactor = 0;
        private int m_MaxTension = 0;
        private int m_MaxLength = 0;
        private int m_MaxSWBP = 0;
        private double m_SwbpChangePoint = 0;
        private double m_TotalLength = 0;
        private double m_TotalTension = 0;
        private double m_TotalReverseTension = 0;
        private double m_TotalSWBP = 0;
        private double m_TotalReverseSWBP = 0;
        private double m_COF = 0;

        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();

        // Cable Configuration Types
        public const string M_CABLECONFIG_SINGLE = "Single";
        public const string M_CABLECONFIG_CRADLED = "Cradled";
        public const string M_CABLECONFIG_TRIANGULAR = "Triangular";
        public const string M_CABLECONFIG_DIAMOND = "Diamond";

        public const string M_SINGLE_PHASE = "1-Phase";
        public const string M_THREE_PHASE = "3-Phase";

        // Section Types
        public const string M_SECTIONTYPE_STRAIGHT = "Straight";
        public const string M_SECTIONTYPE_VERTICAL = "Vertical Bend";
        public const string M_SECTIONTYPE_HORIZONTAL = "Horizontal Bend";
        public const string M_SECTIONTYPE_RISER = "Riser w/Vertical Bend";
        public const string M_SECTIONTYPE_PULLEY = "Pulley";
        public const string M_SECTIONTYPE_DIP = "Dip";

        // Datagridview columns
        public const int M_GR_SECTIONTYPE = 0;
        public const int M_GR_LENGTH = 1;
        public const int M_GR_BENDANGLE = 2;
        public const int M_GR_BENDRADIUS = 3;
        public const int M_GR_DEPTH = 4;
        public const int M_GR_FWD_TENSION = 5;
        public const int M_GR_FWD_SWBP = 6;
        public const int M_GR_REV_TENSION = 7;
        public const int M_GR_REV_SWBP = 8;
        public const int M_GR_FWD_COF = 9;
        public const int M_GR_REV_COF = 10;
        public const int M_GR_ROW_INDEX = 11;
        public const int M_GR_STRUCTURE_FROM = 12;
        public const int M_GR_STRUCTURE_TO = 13;
        public const int M_GR_SEGMENT_INDEX = 14;

        #region clsCPTCalculate Properties
        public string CableConfiguration
        {
            get
            {
                return m_CableConfiguration;
            }
            set
            {
                m_CableConfiguration = value;
            }
        }

        public double CableDiameter
        {
            get
            {
                return m_CableDiameter;
            }
            set
            {
                m_CableDiameter = value;
            }
        }

        public double DuctDiameter
        {
            get
            {
                return m_DuctDiameter;
            }
            set
            {
                m_DuctDiameter = value;
            }
        }

        public double DuctDiameterNominal
        {
            get
            {
                return m_DuctDiameterNominal;
            }
            set
            {
                m_DuctDiameterNominal = value;
            }
        }

        public double StandardBendRadius
        {
            get
            {
                return m_StandardBendRadius;
            }
            set
            {
                m_StandardBendRadius = value;
            }
        }

        public double CableClearance
        {
            get
            {
                return m_CableClearance;
            }
            set
            {
                m_CableClearance = value;
            }
        }

        public double MinimumClearance
        {
            get
            {
                return m_MinimumClearance;
            }
            set
            {
                m_MinimumClearance = value;
            }
        }

        public string JamRatio
        {
            get
            {
                return m_JamRatio;
            }
            set
            {
                m_JamRatio = value;
            }
        }

        public double LowCOF
        {
            get
            {
                return m_LowCOF;
            }
            set
            {
                m_LowCOF = value;
            }
        }

        public double HighCOF
        {
            get
            {
                return m_HighCOF;
            }
            set
            {
                m_HighCOF = value;
            }
        }

        public double SwbpChangePoint
        {
            get
            {
                return m_SwbpChangePoint;
            }
            set
            {
                m_SwbpChangePoint = value;
            }
        }

        public string Phases
        {
            get
            {
                return m_Phases;
            }
            set
            {
                m_Phases = value;
            }
        }

        public double WeightCorrectionFactor
        {
            get
            {
                return m_WeightCorrectionFactor;
            }
            set
            {
                m_WeightCorrectionFactor = value;
            }
        }

        public double WeightPerFoot
        {
            get
            {
                return m_WeightPerFoot;
            }
            set
            {
                m_WeightPerFoot = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return m_MaxLength;
            }
            set
            {
                m_MaxLength = value;
            }
        }

        public int MaxTension
        {
            get
            {
                return m_MaxTension;
            }
            set
            {
                m_MaxTension = value;
            }
        }

        public int MaxSWBP
        {
            get
            {
                return m_MaxSWBP;
            }
            set
            {
                m_MaxSWBP = value;
            }
        }

        public double TotalLength
        {
            get
            {
                return m_TotalLength;
            }
            set
            {
                m_TotalLength = value;
            }
        }

        public double TotalTension
        {
            get
            {
                return m_TotalTension;
            }
            set
            {
                m_TotalTension = value;
            }
        }

        public double TotalReverseTension
        {
            get
            {
                return m_TotalReverseTension;
            }
            set
            {
                m_TotalReverseTension = value;
            }
        }

        public double TotalSWBP
        {
            get
            {
                return m_TotalSWBP;
            }
            set
            {
                m_TotalSWBP = value;
            }
        }

        public double TotalReverseSWBP
        {
            get
            {
                return m_TotalReverseSWBP;
            }
            set
            {
                m_TotalReverseSWBP = value;
            }
        }
        #endregion

        // Entry procedure that calls the calculation procedures.
        public bool Calculate (ref DataGridView dgvSections)
        {
            bool bReturnValue = false;

            try
            {
                // Calculate the cable clearance
                if (!CalculateClearance())
                {
                    return false;                    
                }

                // Calculate the jam ratio
                if (!CalculateJamRatio())
                {
                    return false;
                }

                // Calculate the weight correction factor
                if (!CalculateWeightCorrectionFactor())
                {
                    return false;
                }

                // Calculate the tension and side wall bearing pressure
                if (!CalculateTension(ref dgvSections, M_GR_FWD_TENSION, M_GR_FWD_SWBP, M_GR_FWD_COF))
                {
                    return false;
                }

                ListSortDirection ReverseDirection = ListSortDirection.Descending;
                dgvSections.Sort(dgvSections.Columns[M_GR_ROW_INDEX], ReverseDirection);
                TotalTension = Convert.ToDouble(dgvSections.Rows[0].Cells[M_GR_FWD_TENSION].Value);

                // Calculate the reverse tension and side wall bearing pressure
                if (!CalculateTension(ref dgvSections, M_GR_REV_TENSION, M_GR_REV_SWBP, M_GR_REV_COF))
                {
                    return false;
                }

                ListSortDirection SortDirection = ListSortDirection.Ascending;
                dgvSections.Sort(dgvSections.Columns[M_GR_ROW_INDEX], SortDirection);
                TotalReverseTension = Convert.ToDouble(dgvSections.Rows[0].Cells[M_GR_REV_TENSION].Value);

                bReturnValue = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Calculate the cable clearance
        private bool CalculateClearance()
        {
            bool bReturnValue = false;

            try
            {
                double nominalCableDiameter = CableDiameter + 0.05;
                double clearance = 0;

                switch (CableConfiguration)
                {
                    case M_CABLECONFIG_SINGLE:
                        clearance = DuctDiameter - nominalCableDiameter;
                        bReturnValue = true;
                        break;
                    case M_CABLECONFIG_CRADLED:
                    case M_CABLECONFIG_TRIANGULAR:
                        clearance = DuctDiameter / 2 - 1.366 * nominalCableDiameter + 0.5 * (DuctDiameter - nominalCableDiameter)
                                    * Math.Sqrt(Math.Abs(1 - Math.Pow((nominalCableDiameter / (DuctDiameter - nominalCableDiameter)), 2)));
                        bReturnValue = true;
                        break;
                    case M_CABLECONFIG_DIAMOND:
                        clearance = DuctDiameter - nominalCableDiameter - 2 * Math.Pow(nominalCableDiameter, 2) / (DuctDiameter - nominalCableDiameter);
                        bReturnValue = true;
                        break;
                    default:
                        bReturnValue = false;
                        break;
                }

                CableClearance = Math.Round(clearance, 3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_CABLE_CLEARANCE + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }            

            return bReturnValue;
        }

        // Calculate jam ratio - the tendency of cables to jam in conduits, normally in a bend, when the conduit diameter is about 3 times the diameter of the cable
        private bool CalculateJamRatio()
        {
            bool bReturnValue = false;

            try
            {
                if (Phases == M_SINGLE_PHASE)
                {
                    JamRatio = "n/a";
                }
                else
                {
                    JamRatio = Math.Round(DuctDiameter / CableDiameter, 2).ToString();
                }
                
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_JAM_RATIO + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Calculate the weight correction factor - a factor multiplied by the cable weight based on the cable configuration
        private bool CalculateWeightCorrectionFactor()
        {
            bool bReturnValue = false;            

            try
            {
                double tempJamRatio = DuctDiameter / CableDiameter;
                double wcf = 0;

                switch (CableConfiguration)
                {
                    case M_CABLECONFIG_SINGLE:
                        wcf = 1.0;
                        bReturnValue = true;
                        break;
                    case M_CABLECONFIG_CRADLED:
                        wcf = 1 + 1.33333 * Math.Pow(1 / (tempJamRatio - 1), 2);
                        bReturnValue = true;
                        break;
                    case M_CABLECONFIG_TRIANGULAR:
                        wcf = 1 / Math.Sqrt(1 - Math.Pow(1 / (tempJamRatio - 1), 2));
                        bReturnValue = true;
                        break;
                    case M_CABLECONFIG_DIAMOND:
                        wcf = 1 + 2 * Math.Pow(1 / (tempJamRatio - 1), 2);
                        bReturnValue = true;
                        break;
                    default:
                        bReturnValue = false;
                        break;
                }

                WeightCorrectionFactor = Math.Round(wcf, 3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_WCF + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Loop through the records in the datagridview and calculate the tension for each section and also SWBP for bends.
        // Calculate the forward or reverse values based on the tensionField and swbpField.
        private bool CalculateTension(ref DataGridView dgvSections, int tensionField, int swbpField, int cofField)
        {
            bool bReturnValue = false;
            string segmentType = string.Empty;
            double entranceTension = 0;
            double exitTension = 0;
            double length = 0;
            double angle = 0;
            double radius = 0;
            double depth = 0;
            double swbp = 0;

            try
            {

                foreach (DataGridViewRow row in dgvSections.Rows)
                {
                    segmentType = string.Empty;
                    length = 0;
                    angle = 0;
                    radius = 0;
                    depth = 0;
                    swbp = 0;

                    m_COF = LowCOF;

                    if (Convert.ToString(row.Cells[M_GR_SECTIONTYPE].Value) != string.Empty)
                    {
                        segmentType = row.Cells[M_GR_SECTIONTYPE].Value.ToString();
                    }
                    else
                    {
                        continue;
                    }
                    
                    if (Convert.ToString(row.Cells[M_GR_LENGTH].Value) != string.Empty)
                    {
                        length = Convert.ToDouble(row.Cells[M_GR_LENGTH].Value);
                    }

                    if (Convert.ToString(row.Cells[M_GR_BENDANGLE].Value) != string.Empty)
                    {
                        angle = Convert.ToDouble(row.Cells[M_GR_BENDANGLE].Value);
                    }

                    if (Convert.ToString(row.Cells[M_GR_BENDRADIUS].Value) != string.Empty)
                    {
                        radius = Convert.ToDouble(row.Cells[M_GR_BENDRADIUS].Value);
                    }

                    if (Convert.ToString(row.Cells[M_GR_DEPTH].Value) != string.Empty)
                    {
                        depth = Convert.ToDouble(row.Cells[M_GR_DEPTH].Value);
                    }

                        switch (segmentType)
                    {
                        case M_SECTIONTYPE_STRAIGHT:
                            CalculateStraightTension(entranceTension, length, angle, radius, out exitTension);
                            row.Cells[tensionField].Value = Math.Round(exitTension, 0);
                            bReturnValue = true;
                            break;
                        case M_SECTIONTYPE_DIP:
                            CalculateDipTension(entranceTension, length, depth, out exitTension);
                            row.Cells[tensionField].Value = Math.Round(exitTension, 0);

                            radius = Math.Pow((length / 2), 2) / (4 * Math.Abs(depth));

                            CalculateSWBP(exitTension, length, angle, radius, out swbp);                            
                            row.Cells[swbpField].Value = Math.Round(swbp, 0);
                            
                            bReturnValue = true;
                            break;
                        case M_SECTIONTYPE_HORIZONTAL:
                            CalculateHorizontalBendTension(entranceTension, length, angle, radius, out exitTension);
                            CalculateSWBP(exitTension, length, angle, radius, out swbp);

                            if (swbp >= SwbpChangePoint)
                            {
                                // Recalculate using the high coefficient of friction
                                m_COF = HighCOF;
                                CalculateHorizontalBendTension(entranceTension, length, angle, radius, out exitTension);
                                CalculateSWBP(exitTension, length, angle, radius, out swbp);
                            }

                            row.Cells[tensionField].Value = Math.Round(exitTension, 0);
                            row.Cells[swbpField].Value = Math.Round(swbp, 0);

                            bReturnValue = true;
                            break;
                        case M_SECTIONTYPE_VERTICAL:
                            if (tensionField == M_GR_REV_TENSION)
                            {
                                // Calculating the reverse tension. The Vertical Bend type needs to be swapped.
                                // For example, Concave Up Pulling Down becomes Concave Up Pulling Up.
                                radius *= -1;
                            }
                            CalculateVerticalBendTension(entranceTension, length, angle, radius, out exitTension);
                            CalculateSWBP(exitTension, length, angle, radius, out swbp);

                            if (swbp >= SwbpChangePoint)
                            {
                                // Recalculate using the high coefficient of friction
                                m_COF = HighCOF;
                                CalculateVerticalBendTension(entranceTension, length, angle, radius, out exitTension);
                                CalculateSWBP(exitTension, length, angle, radius, out swbp);
                            }

                            row.Cells[tensionField].Value = Math.Round(exitTension, 0);
                            row.Cells[swbpField].Value = Math.Round(swbp, 0);

                            bReturnValue = true;
                            break;
                        case M_SECTIONTYPE_RISER:                            
                            CalculateRiserTension(entranceTension, length, angle, radius, tensionField, out exitTension);
                            CalculateSWBP(exitTension, length, angle, radius, out swbp);
                            
                            if (swbp >= SwbpChangePoint)
                            {
                                // Recalculate using the high coefficient of friction
                                m_COF = HighCOF;
                                CalculateRiserTension(entranceTension, length, angle, radius, tensionField, out exitTension);
                                CalculateSWBP(exitTension, length, angle, radius, out swbp);
                            }

                            row.Cells[tensionField].Value = Math.Round(exitTension, 0);
                            row.Cells[swbpField].Value = Math.Round(swbp, 0);

                            bReturnValue = true;
                            break;
                        case M_SECTIONTYPE_PULLEY:
                            CalculateSWBP(entranceTension, length, angle, radius, out swbp);

                            if (swbp >= SwbpChangePoint)
                            {
                                // Recalculate using the high coefficient of friction
                                m_COF = HighCOF;
                                CalculateSWBP(entranceTension, length, angle, radius, out swbp);
                            }

                            row.Cells[tensionField].Value = Math.Round(exitTension, 0);
                            row.Cells[swbpField].Value = Math.Round(swbp, 0);

                            bReturnValue = true;
                            break;
                        default:
                            bReturnValue = true;
                            break;
                    }

                    row.Cells[cofField].Value = m_COF;

                    if (tensionField == M_GR_FWD_TENSION)
                    {
                        TotalLength += length;
                        if (swbp > TotalSWBP)
                        {
                            TotalSWBP = swbp;
                        }
                    }
                    else
                    {
                        if (swbp > TotalReverseSWBP)
                        {
                            TotalReverseSWBP = swbp;
                        }
                    }

                    if (exitTension > MaxTension)
                    {
                        row.Cells[tensionField].Style.BackColor = Color.Red;
                    }
                    
                    if (swbp > MaxSWBP)
                    {
                        row.Cells[swbpField].Style.BackColor = Color.Red;
                    }      

                    entranceTension = exitTension;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_TENSION + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Calculate the tension for a Straight section type
        private bool CalculateStraightTension(double initialTension, double length, double angle, double radius, out double tension)
        {
            bool bReturnValue = false;

            tension = 0;

            try
            {
                double degreesToRadians = Math.PI / 180;

                if (angle == 0)
                {
                    if (radius == 0)
                    {
                        tension = initialTension + WeightPerFoot * m_COF * WeightCorrectionFactor * length;
                    }
                    else if (radius > 0)
                    {
                        tension = initialTension + length * WeightPerFoot * (Math.Sin(Math.Abs(Math.Atan(radius / length))) + m_COF
                                  * WeightCorrectionFactor * Math.Cos(Math.Abs(Math.Atan(radius / length))));
                    }
                    else
                    {
                        tension = initialTension - length * WeightPerFoot * (Math.Sin(Math.Abs(Math.Atan(radius / length))) 
                                  - m_COF * WeightCorrectionFactor * Math.Cos(Math.Abs(Math.Atan(radius / length))));
                    }
                }
                else if (angle > 0)
                {
                    tension = initialTension + length * WeightPerFoot * (Math.Sin(degreesToRadians * (Math.Abs(angle)))
                              + m_COF * WeightCorrectionFactor * Math.Cos(degreesToRadians * (Math.Abs(angle))));
                }
                else
                {
                    tension = initialTension - length * WeightPerFoot * (Math.Sin(degreesToRadians * (Math.Abs(angle))) - m_COF
                              * WeightCorrectionFactor * Math.Cos(degreesToRadians * (Math.Abs(angle))));
                }                

                bReturnValue = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_STRAIGHT_TENSION + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Calculate the tension for a Dip section type
        private bool CalculateDipTension(double initialTension, double length, double depth, out double tension)
        {
            bool bReturnValue = false;

            tension = 0;

            try
            {
                double angle = 2 * Math.Abs(depth / (length / 2));
                double x = m_COF * WeightCorrectionFactor * angle;

                if (initialTension > 0)
                {
                    tension = initialTension * Math.Exp(4 * x) + Math.Exp(4 * x) - 2 * Math.Exp(3 * x) + 2 * Math.Exp(x) - 1;
                }
                else
                {
                    tension = initialTension + WeightPerFoot * WeightCorrectionFactor * m_COF * length;
                }

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_DIP_TENSION + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Calculate the tension for a Horizontal Bend section type
        private bool CalculateHorizontalBendTension(double initialTension, double length, double angle, double radius, out double tension)
        {
            bool bReturnValue = false;

            tension = 0;

            try
            {
                double degreesToRadians = Math.PI / 180;
                double x = m_COF * WeightCorrectionFactor * angle * degreesToRadians;

                tension = initialTension * Math.Cosh(x) + Math.Sqrt(Math.Pow(initialTension, 2) + Math.Pow((WeightPerFoot * radius), 2)) * Math.Sinh(x);

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_HBEND_TENSION + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Calculate the tension for a Vertical Bend section type
        private bool CalculateVerticalBendTension(double initialTension, double length, double angle, double radius, out double tension)
        {
            bool bReturnValue = false;            

            tension = 0;

            try
            {
                double degreesToRadians = Math.PI / 180;
                double x = m_COF * WeightCorrectionFactor * degreesToRadians * Math.Abs(angle);

                if (initialTension > 0)
                {

                    double y = WeightPerFoot * Math.Abs(radius) / (1 + Math.Pow((m_COF * WeightCorrectionFactor), 2));
                    if (angle > 0)
                    {
                        y *= -1;
                    }

                    double z = 0;
                    if (radius >= 0)
                    {
                        z = (2 * m_COF * WeightCorrectionFactor * Math.Sin(degreesToRadians * (Math.Abs(angle)))
                             - (1 - Math.Pow((m_COF * WeightCorrectionFactor), 2)) * (Math.Exp(x) - (Math.Cos(degreesToRadians * (Math.Abs(angle))))));
                    }
                    else
                    {
                        z = (2 * m_COF * WeightCorrectionFactor * Math.Exp(m_COF * WeightCorrectionFactor * degreesToRadians * (Math.Abs(angle)))
                            * Math.Sin(degreesToRadians * (Math.Abs(angle))) + (1 - Math.Pow((m_COF * WeightCorrectionFactor), 2))
                            * (1 - Math.Exp(m_COF * WeightCorrectionFactor * degreesToRadians * (Math.Abs(angle)))
                            * Math.Cos(degreesToRadians * (Math.Abs(angle)))));
                    }

                    tension = initialTension * Math.Exp(x) + y * z;
                }
                else
                {
                    tension = initialTension + x * WeightPerFoot * Math.Abs(radius);
                }

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_VBEND_TENSION + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Calculate the tension for a Riser w/Vertical Bend section type
        private bool CalculateRiserTension(double initialTension, double length, double angle, double radius, int direction, out double tension)
        {
            bool bReturnValue = false;            

            tension = 0;

            try
            {
                double degreesToRadians = Math.PI / 180;

                if (initialTension > 0)
                {
                    double x = initialTension * Math.Exp(m_COF * WeightCorrectionFactor * degreesToRadians * Math.Abs(angle));
                    double y = 0;

                    if (length > 0)
                    {
                        y = WeightPerFoot * Math.Abs(length);
                    }

                    //if (beginRiser)
                    //{
                    //    tension = x - WeightPerFoot * Math.Abs(radius) / (1 + Math.Pow((m_COF * WeightCorrectionFactor), 2))
                    //              * (2 * m_COF * WeightCorrectionFactor * Math.Exp(m_COF * WeightCorrectionFactor * degreesToRadians 
                    //              * (Math.Abs(angle))) * Math.Sin(degreesToRadians * (Math.Abs(angle))) 
                    //              + (1 - Math.Pow((m_COF * WeightCorrectionFactor), 2)) * (1 - Math.Exp(m_COF * WeightCorrectionFactor 
                    //              * degreesToRadians * (Math.Abs(angle))) * Math.Cos(degreesToRadians * (Math.Abs(angle)))));
                    //}
                    //else
                    //{
                    tension = x - WeightPerFoot * Math.Abs(radius) / (1 + Math.Pow((m_COF * WeightCorrectionFactor), 2))
                          * (2 * m_COF * WeightCorrectionFactor * Math.Sin(degreesToRadians * (Math.Abs(angle)))
                          - (1 - Math.Pow((m_COF * WeightCorrectionFactor), 2)) * (Math.Exp(m_COF * WeightCorrectionFactor * degreesToRadians * Math.Abs(angle))
                          - Math.Cos(degreesToRadians * Math.Abs(angle)))) + y;
                    //}
                }
                else
                {
                    tension = initialTension + m_COF * WeightCorrectionFactor * WeightPerFoot * Math.Abs(radius) * degreesToRadians * Math.Abs(angle);

                    if (direction == M_GR_REV_TENSION && (WeightPerFoot * Math.Abs(length)) > 0)
                    {
                        tension += WeightPerFoot * Math.Abs(length);
                    }
                }

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_RISER_TENSION + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }

        // Calculate the Sidewall Bearing Pressure (SWBP) - the pressure that the inside of a bend exerts on the cables during a pull.
        private bool CalculateSWBP(double tension, double length, double angle, double radius, out double swbp)
        {
            bool bReturnValue = false;

            swbp = 0;

            try
            {
                switch (CableConfiguration)
                {
                    case M_CABLECONFIG_SINGLE:
                        swbp = Math.Abs(tension / radius);
                        bReturnValue = true;
                        break;
                    case M_CABLECONFIG_CRADLED:
                        swbp = Math.Abs((3 * WeightCorrectionFactor - 2) * tension / (3 * radius));
                        bReturnValue = true;
                        break;
                    case M_CABLECONFIG_TRIANGULAR:
                        swbp = Math.Abs(WeightCorrectionFactor * tension / (2 * radius));
                        bReturnValue = true;
                        break;
                    case M_CABLECONFIG_DIAMOND:
                        swbp = Math.Abs((WeightCorrectionFactor - 1) * tension / radius);
                        bReturnValue = true;
                        break;
                    default:
                        bReturnValue = false;
                        break;
                }

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_CPT_CALCULATE_SWBP + ": " + ex.Message, ConstantsDT.COMMAND_NAME_CABLE_PULL_TENSION, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            return bReturnValue;
        }
    }
}
