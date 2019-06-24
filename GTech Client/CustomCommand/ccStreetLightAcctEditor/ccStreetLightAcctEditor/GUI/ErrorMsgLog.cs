using GTechnology.Oncor.CustomAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI.GUI
{
    public partial class ErrorMsgLog : Form
    {
        public ErrorMsgLog()
        {
            InitializeComponent();
            dtGridViewErrLog.Columns["Tab"].DataPropertyName = "ErrorIn";
            dtGridViewErrLog.Columns["ErrorMsg"].DataPropertyName = "ErrorMessage";
          //  dtGridViewErrLog.DataSource = GetErrors();
        }

        public List<ErrorLog> ErrorLog { set => dtGridViewErrLog.DataSource = value; }
        private BindingList<ErrorLog> GetErrors()
        {

            BindingList<ErrorLog> errs = new BindingList<ErrorLog>();
            errs.Add(new ErrorLog
            {
                ErrorIn="Street Light Account",
                ErrorMessage="ESI_Location - xxx duplicate Valu exists"
            });

            errs.Add(new ErrorLog
            {
                ErrorIn = "Street Light Account",
                ErrorMessage = "ESI_Location - xxx assigned to Street Light Hence cannot be delete"
            });

            errs.Add(new ErrorLog
            {
                ErrorIn = "Street Light Account",
                ErrorMessage = "ESI_Location - YYY not exists in CC&B "
            });

            errs.Add(new ErrorLog
            {
                ErrorIn = "Street Light Account",
                ErrorMessage = "For ESI_Location - YYY Lamp _Type is required "
            });

            errs.Add(new ErrorLog
            {
                ErrorIn = "Description-Value Lists",
                ErrorMessage = "Street Light Description xxx -already exists"
            });

            errs.Add(new ErrorLog
            {
                ErrorIn = "Description-Value Lists",
                ErrorMessage = "Description xxx -cannot be deleted"
            });

            errs.Add(new ErrorLog
            {
                ErrorIn = "Owner Code- Value Lists",
                ErrorMessage = "Owner Code xxx -cannot be deleted"
            });
            errs.Add(new ErrorLog
            {
                ErrorIn = "Owner Code- Value Lists",
                ErrorMessage = "Owner Code xxx -cannot be Updated"
            });
            errs.Add(new ErrorLog
            {
                ErrorIn = "Rate Code- Value Lists",
                ErrorMessage = "Rate Code xxx -cannot be Updated"
            });

            errs.Add(new ErrorLog
            {
                ErrorIn = "Rate Schedule- Value Lists",
                ErrorMessage = "Rate Scehdule xxx -cannot be Updated"
            });

            return errs;
        }
    }
}
