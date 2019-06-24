﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 
namespace OncorTicketCreation {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.oncor.com/DIS")]
    [System.Xml.Serialization.XmlRootAttribute("RequestHeader", Namespace="http://www.oncor.com/DIS", IsNullable=false)]
    public partial class RequestHeaderType {
        
        private string sourceSystemField;
        
        private string transactionIdField;
        
        private string transactionTypeField;
        
        private string requestorField;
        
        private System.DateTime timestampField;
        
        private bool timestampFieldSpecified;
        
        /// <remarks/>
        public string SourceSystem {
            get {
                return this.sourceSystemField;
            }
            set {
                this.sourceSystemField = value;
            }
        }
        
        /// <remarks/>
        public string TransactionId {
            get {
                return this.transactionIdField;
            }
            set {
                this.transactionIdField = value;
            }
        }
        
        /// <remarks/>
        public string TransactionType {
            get {
                return this.transactionTypeField;
            }
            set {
                this.transactionTypeField = value;
            }
        }
        
        /// <remarks/>
        public string Requestor {
            get {
                return this.requestorField;
            }
            set {
                this.requestorField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime Timestamp {
            get {
                return this.timestampField;
            }
            set {
                this.timestampField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TimestampSpecified {
            get {
                return this.timestampFieldSpecified;
            }
            set {
                this.timestampFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.oncor.com/DIS")]
    [System.Xml.Serialization.XmlRootAttribute("ResultStatus", Namespace="http://www.oncor.com/DIS", IsNullable=false)]
    public partial class ResponseType {
        
        private string statusField;
        
        private string resultCodeField;
        
        private string[] errorMsgField;
        
        private System.DateTime timestampField;
        
        private bool timestampFieldSpecified;
        
        /// <remarks/>
        public string Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        public string ResultCode {
            get {
                return this.resultCodeField;
            }
            set {
                this.resultCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ErrorMsg")]
        public string[] ErrorMsg {
            get {
                return this.errorMsgField;
            }
            set {
                this.errorMsgField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime Timestamp {
            get {
                return this.timestampField;
            }
            set {
                this.timestampField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TimestampSpecified {
            get {
                return this.timestampFieldSpecified;
            }
            set {
                this.timestampFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.oncor.com/Ticket_Creation")]
    [System.Xml.Serialization.XmlRootAttribute("TicketCreation", Namespace="http://www.oncor.com/Ticket_Creation", IsNullable=false)]
    public partial class TicketCreationType {
        
        private string njunsTicketIdField;
        
        private string ticketNumberField;
        
        private string ticketTypeField;
        
        private string poleNumberField;
        
        private string miscellaneousIdField;
        
        private string njunsMemberCodeField;
        
        private string poleOwnerField;
        
        private string startDateField;
        
        private string workRequestedDateField;
        
        private string contactNameField;
        
        private string contactPhoneField;
        
        private string stateField;
        
        private string countyField;
        
        private string placeField;
        
        private string latitudeField;
        
        private string longitudeField;
        
        private string houseNumberField;
        
        private string streetNameField;
        
        private string priorityCodeField;
        
        private string njunsMemberField;
        
        private string jobTypeField;
        
        private string numberOfPolesField;
        
        private string daysIntervalField;
        
        private string remarksField;
        
        private TicketStepType[] ticketStepsField;
        
        private FileAttachmentType[] fileAttachmentsField;
        
        /// <remarks/>
        public string NjunsTicketId {
            get {
                return this.njunsTicketIdField;
            }
            set {
                this.njunsTicketIdField = value;
            }
        }
        
        /// <remarks/>
        public string TicketNumber {
            get {
                return this.ticketNumberField;
            }
            set {
                this.ticketNumberField = value;
            }
        }
        
        /// <remarks/>
        public string TicketType {
            get {
                return this.ticketTypeField;
            }
            set {
                this.ticketTypeField = value;
            }
        }
        
        /// <remarks/>
        public string PoleNumber {
            get {
                return this.poleNumberField;
            }
            set {
                this.poleNumberField = value;
            }
        }
        
        /// <remarks/>
        public string MiscellaneousId {
            get {
                return this.miscellaneousIdField;
            }
            set {
                this.miscellaneousIdField = value;
            }
        }
        
        /// <remarks/>
        public string NjunsMemberCode {
            get {
                return this.njunsMemberCodeField;
            }
            set {
                this.njunsMemberCodeField = value;
            }
        }
        
        /// <remarks/>
        public string PoleOwner {
            get {
                return this.poleOwnerField;
            }
            set {
                this.poleOwnerField = value;
            }
        }
        
        /// <remarks/>
        public string StartDate {
            get {
                return this.startDateField;
            }
            set {
                this.startDateField = value;
            }
        }
        
        /// <remarks/>
        public string WorkRequestedDate {
            get {
                return this.workRequestedDateField;
            }
            set {
                this.workRequestedDateField = value;
            }
        }
        
        /// <remarks/>
        public string ContactName {
            get {
                return this.contactNameField;
            }
            set {
                this.contactNameField = value;
            }
        }
        
        /// <remarks/>
        public string ContactPhone {
            get {
                return this.contactPhoneField;
            }
            set {
                this.contactPhoneField = value;
            }
        }
        
        /// <remarks/>
        public string State {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        public string County {
            get {
                return this.countyField;
            }
            set {
                this.countyField = value;
            }
        }
        
        /// <remarks/>
        public string Place {
            get {
                return this.placeField;
            }
            set {
                this.placeField = value;
            }
        }
        
        /// <remarks/>
        public string Latitude {
            get {
                return this.latitudeField;
            }
            set {
                this.latitudeField = value;
            }
        }
        
        /// <remarks/>
        public string Longitude {
            get {
                return this.longitudeField;
            }
            set {
                this.longitudeField = value;
            }
        }
        
        /// <remarks/>
        public string HouseNumber {
            get {
                return this.houseNumberField;
            }
            set {
                this.houseNumberField = value;
            }
        }
        
        /// <remarks/>
        public string StreetName {
            get {
                return this.streetNameField;
            }
            set {
                this.streetNameField = value;
            }
        }
        
        /// <remarks/>
        public string PriorityCode {
            get {
                return this.priorityCodeField;
            }
            set {
                this.priorityCodeField = value;
            }
        }
        
        /// <remarks/>
        public string NjunsMember {
            get {
                return this.njunsMemberField;
            }
            set {
                this.njunsMemberField = value;
            }
        }
        
        /// <remarks/>
        public string JobType {
            get {
                return this.jobTypeField;
            }
            set {
                this.jobTypeField = value;
            }
        }
        
        /// <remarks/>
        public string NumberOfPoles {
            get {
                return this.numberOfPolesField;
            }
            set {
                this.numberOfPolesField = value;
            }
        }
        
        /// <remarks/>
        public string DaysInterval {
            get {
                return this.daysIntervalField;
            }
            set {
                this.daysIntervalField = value;
            }
        }
        
        /// <remarks/>
        public string Remarks {
            get {
                return this.remarksField;
            }
            set {
                this.remarksField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TicketStep", IsNullable=false)]
        public TicketStepType[] TicketSteps {
            get {
                return this.ticketStepsField;
            }
            set {
                this.ticketStepsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FileAttachment", IsNullable=false)]
        public FileAttachmentType[] FileAttachments {
            get {
                return this.fileAttachmentsField;
            }
            set {
                this.fileAttachmentsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.oncor.com/Ticket_Creation")]
    [System.Xml.Serialization.XmlRootAttribute("TicketStep", Namespace="http://www.oncor.com/Ticket_Creation", IsNullable=false)]
    public partial class TicketStepType {
        
        private string stepNumberField;
        
        private string njunsMemberField;
        
        private string jobTypeField;
        
        private string numberOfPolesField;
        
        private string daysIntervalField;
        
        private string remarksField;
        
        /// <remarks/>
        public string StepNumber {
            get {
                return this.stepNumberField;
            }
            set {
                this.stepNumberField = value;
            }
        }
        
        /// <remarks/>
        public string NjunsMember {
            get {
                return this.njunsMemberField;
            }
            set {
                this.njunsMemberField = value;
            }
        }
        
        /// <remarks/>
        public string JobType {
            get {
                return this.jobTypeField;
            }
            set {
                this.jobTypeField = value;
            }
        }
        
        /// <remarks/>
        public string NumberOfPoles {
            get {
                return this.numberOfPolesField;
            }
            set {
                this.numberOfPolesField = value;
            }
        }
        
        /// <remarks/>
        public string DaysInterval {
            get {
                return this.daysIntervalField;
            }
            set {
                this.daysIntervalField = value;
            }
        }
        
        /// <remarks/>
        public string Remarks {
            get {
                return this.remarksField;
            }
            set {
                this.remarksField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.oncor.com/Ticket_Creation")]
    [System.Xml.Serialization.XmlRootAttribute("FileAttachment", Namespace="http://www.oncor.com/Ticket_Creation", IsNullable=false)]
    public partial class FileAttachmentType {
        
        private string nameField;
        
        private byte[] contentField;
        
        private string commentField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        public string Comment {
            get {
                return this.commentField;
            }
            set {
                this.commentField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.oncor.com/Ticket_Creation")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.oncor.com/Ticket_Creation", IsNullable=false)]
    public partial class TicketSteps {
        
        private TicketStepType[] ticketStepField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TicketStep")]
        public TicketStepType[] TicketStep {
            get {
                return this.ticketStepField;
            }
            set {
                this.ticketStepField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.oncor.com/Ticket_Creation")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.oncor.com/Ticket_Creation", IsNullable=false)]
    public partial class FileAttachments {
        
        private FileAttachmentType[] fileAttachmentField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FileAttachment")]
        public FileAttachmentType[] FileAttachment {
            get {
                return this.fileAttachmentField;
            }
            set {
                this.fileAttachmentField = value;
            }
        }
    }
}
