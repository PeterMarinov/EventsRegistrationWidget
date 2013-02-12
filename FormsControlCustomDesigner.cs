using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity.Web.UI.ControlDesign;
using System.Web.UI.HtmlControls;
using Telerik.Sitefinity.Web.UI;
using System.Web.UI;
using System.ComponentModel;
using Telerik.Sitefinity;
using Telerik.Web.UI;
using Telerik.Sitefinity.Web.UI.Fields;
using Telerik.Sitefinity.Events.Model;

namespace EventsRegistration
{
    class FormsControlCustomDesigner : ControlDesignerBase
    {

        private const string layoutTemplateName = "EventsRegistration.Resources.FormsControlDesigner.ascx";

        /// <summary>
        /// This is the embedded control template for the custom Forms Control Designer.
        /// </summary>
        protected override string LayoutTemplateName
        {
            get
            {
                return FormsControlCustomDesigner.layoutTemplateName;
            }
        }





        /// <summary>
        /// Gets a type from the resource assembly.
        /// Resource assembly is an assembly that contains embedded resources such as templates, images, CSS files and etc.
        /// By default this is Telerik.Sitefinity.Resources.dll.
        /// </summary>
        /// <value>The resources assembly info.</value>
        protected override Type ResourcesAssemblyInfo
        {
            get
            {
                return this.GetType();
            }
        }





        protected override void InitializeControls(Telerik.Sitefinity.Web.UI.GenericContainer container)
        {
            this.DesignerMode = ControlDesignerModes.Simple;

            //var kendoTheme = new HtmlLink();
            //kendoTheme.Attributes["href"] = ResolveClientUrl("EventsRegistration.Resources.KendoWidgetStyle.css");
            //kendoTheme.Attributes["rel"] = "stylesheet";
            //kendoTheme.Attributes["type"] = "text/css";
            //this.Page.Header.Controls.Add(kendoTheme);     

            var listOne = getCustomFields(typeof(Event), typeof(Decimal));
            var listTwo = getCustomFields(typeof(Event));
            this.comboMaxAtt.DataSource = listOne;
            this.comboCurrAtt.DataSource = listOne;
            this.comboProductName.DataSource = listTwo;
            this.comboMaxAtt.DataBind();
            this.comboCurrAtt.DataBind();
            this.comboProductName.DataBind();

            var item = new DataMemberInfo
            {
                Name = "Title",
                HeaderText = "Title",
                ColumnTemplate = "<span>{{Title}}</span>",
                IsSearchField = true
            };

            // set root node for page selector
            PageSelector.RootNodeID = Telerik.Sitefinity.Abstractions.SiteInitializer.FrontendRootNodeId;
        }



        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            var res = new List<ScriptReference>(base.GetScriptReferences());
            var assemblyName = this.GetType().Assembly.GetName().ToString();
            res.Add(new ScriptReference("EventsRegistration.Resources.FormsControlDesigner.js", assemblyName));
            return res.ToArray();
        }

        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var descriptors = new List<ScriptDescriptor>(base.GetScriptDescriptors());
            var descriptor = (ScriptControlDescriptor)descriptors.Last();
            descriptor.AddComponentProperty("comboMaxAtt", this.comboMaxAtt.ClientID);
            descriptor.AddComponentProperty("comboCurrAtt", this.comboCurrAtt.ClientID);
            descriptor.AddComponentProperty("comboProductName", this.comboProductName.ClientID);
            descriptor.AddComponentProperty("pageSelector", this.PageSelector.ClientID);
            return descriptors;
        }

        protected List<String> getCustomFields(Type type, Type filter = null)
        {
            // we are geting the metafield associated with the product we have created and check the name of the field
            var props = TypeDescriptor.GetProperties(type);
            List<String> theList = new List<String>();
            foreach (var singleProp in props)
            {
                if (singleProp.GetType() == typeof(MetafieldPropertyDescriptor))
                {
                    MetafieldPropertyDescriptor customField = singleProp as MetafieldPropertyDescriptor;
                    if (filter != null)
                    {
                        if (customField.PropertyType.Equals(filter))
                        {
                            theList.Add(customField.Name);
                        }
                    }
                    else
                    {
                        theList.Add(customField.Name);
                    }
                }
            }
            return theList;
        }


        public RadComboBox comboMaxAtt
        {
            get { return Container.GetControl<RadComboBox>("RadComboBoxMaxAtt", true); }
        }

        public RadComboBox comboCurrAtt
        {
            get { return Container.GetControl<RadComboBox>("RadComboBoxCurrAtt", true); }
        }

        public RadComboBox comboProductName
        {
            get { return Container.GetControl<RadComboBox>("RadComboBoxProductName", true); }
        }

        protected PageField PageSelector
        {
            get { return Container.GetControl<PageField>("PageSelector", true); }
        }
    }
}
