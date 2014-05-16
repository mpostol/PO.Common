/* ========================================================================
 * Copyright (c) 2005-2009 The OPC Foundation. All rights reserved.
 *
 * OPC Reciprocal Community Binary License ("RCBL") Version 1.00
 * 
 * Unless explicitly acquired and licensed from Licensor under another 
 * license, the contents of this file are subject to the Reciprocal 
 * Community Binary License ("RCBL") Version 1.00, or subsequent versions 
 * as allowed by the RCBL, and You may not copy or use this file in either 
 * source code or executable form, except in compliance with the terms and 
 * conditions of the RCBL.
 * 
 * All software distributed under the RCBL is provided strictly on an 
 * "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
 * AND LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT 
 * LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
 * PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RCBL for specific 
 * language governing rights and limitations under the RCBL.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/RCBL/1.00/
 * ======================================================================*/

namespace Opc.Ua.ModelDesigner {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd", IsNullable=false)]
    public partial class ModelDesign {
        
        private Namespace[] namespacesField;
        
        private NodeDesign[] itemsField;
        
        private string targetNamespaceField;
        
        private string targetXmlNamespaceField;
        
        private string defaultLocaleField;
        
        public ModelDesign() {
            this.defaultLocaleField = "en";
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public Namespace[] Namespaces {
            get {
                return this.namespacesField;
            }
            set {
                this.namespacesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DataType", typeof(DataTypeDesign))]
        [System.Xml.Serialization.XmlElementAttribute("Dictionary", typeof(DictionaryDesign))]
        [System.Xml.Serialization.XmlElementAttribute("EventType", typeof(EventTypeDesign))]
        [System.Xml.Serialization.XmlElementAttribute("Method", typeof(MethodDesign))]
        [System.Xml.Serialization.XmlElementAttribute("Object", typeof(ObjectDesign))]
        [System.Xml.Serialization.XmlElementAttribute("ObjectType", typeof(ObjectTypeDesign))]
        [System.Xml.Serialization.XmlElementAttribute("ReferenceType", typeof(ReferenceTypeDesign))]
        [System.Xml.Serialization.XmlElementAttribute("StateMachineType", typeof(StateMachineTypeDesign))]
        [System.Xml.Serialization.XmlElementAttribute("VariableType", typeof(VariableTypeDesign))]
        [System.Xml.Serialization.XmlElementAttribute("View", typeof(ViewDesign))]
        public NodeDesign[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TargetNamespace {
            get {
                return this.targetNamespaceField;
            }
            set {
                this.targetNamespaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TargetXmlNamespace {
            get {
                return this.targetXmlNamespaceField;
            }
            set {
                this.targetXmlNamespaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("en")]
        public string DefaultLocale {
            get {
                return this.defaultLocaleField;
            }
            set {
                this.defaultLocaleField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class Namespace {
        
        private string nameField;
        
        private string prefixField;
        
        private string internalPrefixField;
        
        private string xmlNamespaceField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Prefix {
            get {
                return this.prefixField;
            }
            set {
                this.prefixField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string InternalPrefix {
            get {
                return this.internalPrefixField;
            }
            set {
                this.internalPrefixField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XmlNamespace {
            get {
                return this.xmlNamespaceField;
            }
            set {
                this.xmlNamespaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class Reference {
        
        private System.Xml.XmlQualifiedName referenceTypeField;
        
        private System.Xml.XmlQualifiedName targetIdField;
        
        private bool isInverseField;
        
        private bool isOneWayField;
        
        public Reference() {
            this.isInverseField = false;
            this.isOneWayField = false;
        }
        
        /// <remarks/>
        public System.Xml.XmlQualifiedName ReferenceType {
            get {
                return this.referenceTypeField;
            }
            set {
                this.referenceTypeField = value;
            }
        }
        
        /// <remarks/>
        public System.Xml.XmlQualifiedName TargetId {
            get {
                return this.targetIdField;
            }
            set {
                this.targetIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsInverse {
            get {
                return this.isInverseField;
            }
            set {
                this.isInverseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsOneWay {
            get {
                return this.isOneWayField;
            }
            set {
                this.isOneWayField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class Parameter {
        
        private LocalizedText descriptionField;
        
        private string nameField;
        
        private int identifierField;
        
        private bool identifierFieldSpecified;
        
        private System.Xml.XmlQualifiedName dataTypeField;
        
        private ValueRank valueRankField;
        
        public Parameter() {
            this.valueRankField = ValueRank.Scalar;
        }
        
        /// <remarks/>
        public LocalizedText Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Identifier {
            get {
                return this.identifierField;
            }
            set {
                this.identifierField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdentifierSpecified {
            get {
                return this.identifierFieldSpecified;
            }
            set {
                this.identifierFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName DataType {
            get {
                return this.dataTypeField;
            }
            set {
                this.dataTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ValueRank.Scalar)]
        public ValueRank ValueRank {
            get {
                return this.valueRankField;
            }
            set {
                this.valueRankField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class LocalizedText {
        
        private string keyField;
        
        private string valueField;
        
        public LocalizedText() {
            this.keyField = "";
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public enum ValueRank {
        
        /// <remarks/>
        Scalar,
        
        /// <remarks/>
        Array,
        
        /// <remarks/>
        ScalarOrArray,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class ListOfChildren {
        
        private InstanceDesign[] itemsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Method", typeof(MethodDesign))]
        [System.Xml.Serialization.XmlElementAttribute("Object", typeof(ObjectDesign))]
        [System.Xml.Serialization.XmlElementAttribute("Property", typeof(PropertyDesign))]
        [System.Xml.Serialization.XmlElementAttribute("Variable", typeof(VariableDesign))]
        public InstanceDesign[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class MethodDesign : InstanceDesign {
        
        private Parameter[] inputArgumentsField;
        
        private Parameter[] outputArgumentsField;
        
        private bool nonExecutableField;
        
        public MethodDesign() {
            this.nonExecutableField = false;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Argument", IsNullable=false)]
        public Parameter[] InputArguments {
            get {
                return this.inputArgumentsField;
            }
            set {
                this.inputArgumentsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Argument", IsNullable=false)]
        public Parameter[] OutputArguments {
            get {
                return this.outputArgumentsField;
            }
            set {
                this.outputArgumentsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool NonExecutable {
            get {
                return this.nonExecutableField;
            }
            set {
                this.nonExecutableField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MethodDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VariableDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DictionaryDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PropertyDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ObjectDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EncodingDesign))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class InstanceDesign : NodeDesign {
        
        private System.Xml.XmlQualifiedName referenceTypeField;
        
        private System.Xml.XmlQualifiedName declarationField;
        
        private System.Xml.XmlQualifiedName typeDefinitionField;
        
        private ModellingRule modellingRuleField;
        
        private uint minCardinalityField;
        
        private uint maxCardinalityField;
        
        private bool preserveDefaultAttributesField;
        
        public InstanceDesign() {
            this.modellingRuleField = ModellingRule.Mandatory;
            this.minCardinalityField = ((uint)(0));
            this.maxCardinalityField = ((uint)(0));
            this.preserveDefaultAttributesField = false;
        }
        
        /// <remarks/>
        public System.Xml.XmlQualifiedName ReferenceType {
            get {
                return this.referenceTypeField;
            }
            set {
                this.referenceTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName Declaration {
            get {
                return this.declarationField;
            }
            set {
                this.declarationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName TypeDefinition {
            get {
                return this.typeDefinitionField;
            }
            set {
                this.typeDefinitionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ModellingRule.Mandatory)]
        public ModellingRule ModellingRule {
            get {
                return this.modellingRuleField;
            }
            set {
                this.modellingRuleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(typeof(uint), "0")]
        public uint MinCardinality {
            get {
                return this.minCardinalityField;
            }
            set {
                this.minCardinalityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(typeof(uint), "0")]
        public uint MaxCardinality {
            get {
                return this.maxCardinalityField;
            }
            set {
                this.maxCardinalityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool PreserveDefaultAttributes {
            get {
                return this.preserveDefaultAttributesField;
            }
            set {
                this.preserveDefaultAttributesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public enum ModellingRule {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        Mandatory,
        
        /// <remarks/>
        Optional,
        
        /// <remarks/>
        ExposesItsArray,
        
        /// <remarks/>
        CardinalityRestriction,
        
        /// <remarks/>
        MandatoryShared,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VariableTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EventTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ObjectTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StateMachineTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ViewDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InstanceDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MethodDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VariableDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DictionaryDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PropertyDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ObjectDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EncodingDesign))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class NodeDesign {
        
        private string browseNameField;
        
        private LocalizedText displayNameField;
        
        private LocalizedText descriptionField;
        
        private ListOfChildren childrenField;
        
        private Reference[] referencesField;
        
        private System.Xml.XmlQualifiedName symbolicNameField;
        
        private System.Xml.XmlQualifiedName symbolicIdField;
        
        private bool isDeclarationField;
        
        private uint numericIdField;
        
        private bool numericIdFieldSpecified;
        
        private string stringIdField;
        
        private uint writeAccessField;
        
        public NodeDesign() {
            this.isDeclarationField = false;
            this.writeAccessField = ((uint)(0));
        }
        
        /// <remarks/>
        public string BrowseName {
            get {
                return this.browseNameField;
            }
            set {
                this.browseNameField = value;
            }
        }
        
        /// <remarks/>
        public LocalizedText DisplayName {
            get {
                return this.displayNameField;
            }
            set {
                this.displayNameField = value;
            }
        }
        
        /// <remarks/>
        public LocalizedText Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        public ListOfChildren Children {
            get {
                return this.childrenField;
            }
            set {
                this.childrenField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public Reference[] References {
            get {
                return this.referencesField;
            }
            set {
                this.referencesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName SymbolicName {
            get {
                return this.symbolicNameField;
            }
            set {
                this.symbolicNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName SymbolicId {
            get {
                return this.symbolicIdField;
            }
            set {
                this.symbolicIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsDeclaration {
            get {
                return this.isDeclarationField;
            }
            set {
                this.isDeclarationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint NumericId {
            get {
                return this.numericIdField;
            }
            set {
                this.numericIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NumericIdSpecified {
            get {
                return this.numericIdFieldSpecified;
            }
            set {
                this.numericIdFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string StringId {
            get {
                return this.stringIdField;
            }
            set {
                this.stringIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(typeof(uint), "0")]
        public uint WriteAccess {
            get {
                return this.writeAccessField;
            }
            set {
                this.writeAccessField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VariableTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EventTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ObjectTypeDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StateMachineTypeDesign))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class TypeDesign : NodeDesign {
        
        private string classNameField;
        
        private System.Xml.XmlQualifiedName baseTypeField;
        
        private bool isAbstractField;
        
        private bool noClassGenerationField;
        
        public TypeDesign() {
            this.isAbstractField = false;
            this.noClassGenerationField = false;
        }
        
        /// <remarks/>
        public string ClassName {
            get {
                return this.classNameField;
            }
            set {
                this.classNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName BaseType {
            get {
                return this.baseTypeField;
            }
            set {
                this.baseTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsAbstract {
            get {
                return this.isAbstractField;
            }
            set {
                this.isAbstractField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool NoClassGeneration {
            get {
                return this.noClassGenerationField;
            }
            set {
                this.noClassGenerationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class ReferenceTypeDesign : TypeDesign {
        
        private LocalizedText inverseNameField;
        
        private bool symmetricField;
        
        public ReferenceTypeDesign() {
            this.symmetricField = false;
        }
        
        /// <remarks/>
        public LocalizedText InverseName {
            get {
                return this.inverseNameField;
            }
            set {
                this.inverseNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Symmetric {
            get {
                return this.symmetricField;
            }
            set {
                this.symmetricField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class DataTypeDesign : TypeDesign {
        
        private Parameter[] fieldsField;
        
        private EncodingDesign[] encodingsField;
        
        private bool noArraysAllowedField;
        
        private bool notInAddressSpaceField;
        
        public DataTypeDesign() {
            this.noArraysAllowedField = false;
            this.notInAddressSpaceField = false;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Field", IsNullable=false)]
        public Parameter[] Fields {
            get {
                return this.fieldsField;
            }
            set {
                this.fieldsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Encoding", IsNullable=false)]
        public EncodingDesign[] Encodings {
            get {
                return this.encodingsField;
            }
            set {
                this.encodingsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool NoArraysAllowed {
            get {
                return this.noArraysAllowedField;
            }
            set {
                this.noArraysAllowedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool NotInAddressSpace {
            get {
                return this.notInAddressSpaceField;
            }
            set {
                this.notInAddressSpaceField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class EncodingDesign : ObjectDesign {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EncodingDesign))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class ObjectDesign : InstanceDesign {
        
        private bool supportsEventsField;
        
        public ObjectDesign() {
            this.supportsEventsField = false;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsEvents {
            get {
                return this.supportsEventsField;
            }
            set {
                this.supportsEventsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class VariableTypeDesign : TypeDesign {
        
        private System.Xml.XmlElement defaultValueField;
        
        private System.Xml.XmlQualifiedName dataTypeField;
        
        private ValueRank valueRankField;
        
        private AccessLevel accessLevelField;
        
        private bool exposesItsChildrenField;
        
        public VariableTypeDesign() {
            this.valueRankField = ValueRank.Scalar;
            this.accessLevelField = AccessLevel.Read;
            this.exposesItsChildrenField = false;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement DefaultValue {
            get {
                return this.defaultValueField;
            }
            set {
                this.defaultValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName DataType {
            get {
                return this.dataTypeField;
            }
            set {
                this.dataTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ValueRank.Scalar)]
        public ValueRank ValueRank {
            get {
                return this.valueRankField;
            }
            set {
                this.valueRankField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(AccessLevel.Read)]
        public AccessLevel AccessLevel {
            get {
                return this.accessLevelField;
            }
            set {
                this.accessLevelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ExposesItsChildren {
            get {
                return this.exposesItsChildrenField;
            }
            set {
                this.exposesItsChildrenField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public enum AccessLevel {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        Read,
        
        /// <remarks/>
        Write,
        
        /// <remarks/>
        ReadWrite,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class EventTypeDesign : TypeDesign {
        
        private System.Xml.XmlQualifiedName sourceObjectTypeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName SourceObjectType {
            get {
                return this.sourceObjectTypeField;
            }
            set {
                this.sourceObjectTypeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StateMachineTypeDesign))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class ObjectTypeDesign : TypeDesign {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class StateMachineTypeDesign : ObjectTypeDesign {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class ViewDesign : NodeDesign {
        
        private bool supportsEventsField;
        
        private bool containsNoLoopsField;
        
        public ViewDesign() {
            this.supportsEventsField = false;
            this.containsNoLoopsField = false;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool SupportsEvents {
            get {
                return this.supportsEventsField;
            }
            set {
                this.supportsEventsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ContainsNoLoops {
            get {
                return this.containsNoLoopsField;
            }
            set {
                this.containsNoLoopsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DictionaryDesign))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PropertyDesign))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class VariableDesign : InstanceDesign {
        
        private System.Xml.XmlElement defaultValueField;
        
        private System.Xml.XmlQualifiedName dataTypeField;
        
        private ValueRank valueRankField;
        
        private AccessLevel accessLevelField;
        
        private int minimumSamplingIntervalField;
        
        private bool historizingField;
        
        public VariableDesign() {
            this.valueRankField = ValueRank.Scalar;
            this.accessLevelField = AccessLevel.Read;
            this.minimumSamplingIntervalField = 0;
            this.historizingField = false;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement DefaultValue {
            get {
                return this.defaultValueField;
            }
            set {
                this.defaultValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName DataType {
            get {
                return this.dataTypeField;
            }
            set {
                this.dataTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ValueRank.Scalar)]
        public ValueRank ValueRank {
            get {
                return this.valueRankField;
            }
            set {
                this.valueRankField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(AccessLevel.Read)]
        public AccessLevel AccessLevel {
            get {
                return this.accessLevelField;
            }
            set {
                this.accessLevelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public int MinimumSamplingInterval {
            get {
                return this.minimumSamplingIntervalField;
            }
            set {
                this.minimumSamplingIntervalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Historizing {
            get {
                return this.historizingField;
            }
            set {
                this.historizingField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class DictionaryDesign : VariableDesign {
        
        private System.Xml.XmlQualifiedName encodingNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName EncodingName {
            get {
                return this.encodingNameField;
            }
            set {
                this.encodingNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://opcfoundation.org/UA/ModelDesign.xsd")]
    public partial class PropertyDesign : VariableDesign {
    }
}
