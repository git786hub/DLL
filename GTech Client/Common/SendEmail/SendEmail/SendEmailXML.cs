﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class eMailRequest {
    
    private string[] toAddrsField;
    
    private string fromAddrField;
    
    private string subjectField;
    
    private string messageField;
    
    private string attachmentsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("ToAddr", IsNullable=false)]
    public string[] ToAddrs {
        get {
            return this.toAddrsField;
        }
        set {
            this.toAddrsField = value;
        }
    }
    
    /// <remarks/>
    public string FromAddr {
        get {
            return this.fromAddrField;
        }
        set {
            this.fromAddrField = value;
        }
    }
    
    /// <remarks/>
    public string Subject {
        get {
            return this.subjectField;
        }
        set {
            this.subjectField = value;
        }
    }
    
    /// <remarks/>
    public string Message {
        get {
            return this.messageField;
        }
        set {
            this.messageField = value;
        }
    }
    
    /// <remarks/>
    public string Attachments {
        get {
            return this.attachmentsField;
        }
        set {
            this.attachmentsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class ToAddrs {
    
    private string[] toAddrField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ToAddr")]
    public string[] ToAddr {
        get {
            return this.toAddrField;
        }
        set {
            this.toAddrField = value;
        }
    }
}
