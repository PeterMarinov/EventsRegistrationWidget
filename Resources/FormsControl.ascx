<%@ Control Language="C#" %>
<%@ Register TagPrefix="sfFields" Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI.Fields" %>
<%@ Register TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI" Assembly="Telerik.Sitefinity" %> 
<sfFields:FormManager id="formManager" runat="server" />
 
 
<asp:Panel ID="errorsPanel" runat="server" CssClass="sfErrorSummary" />
<sf:SitefinityLabel id="successMessage" runat="server" WrapperTagName="div" HideIfNoText="true" CssClass="sfSuccess" />
<asp:Panel ID="formControls" runat="server">
     
</asp:Panel>