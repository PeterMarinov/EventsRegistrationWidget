<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI.ControlDesign"
    TagPrefix="designers" %>
<%@ Register Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI"
    TagPrefix="sitefinity" %>
<%@ Register Assembly="Telerik.Stefinity" Namespace="Telerik.Sitefinity.Web.UI.Fields"
    TagPrefix="sf" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<sitefinity:ResourceLinks ID="resourcesLinks" runat="server">
    <sitefinity:ResourceFile JavaScriptLibrary="JQuery" />
    <sitefinity:ResourceFile Name="Telerik.Sitefinity.Resources.Scripts.Kendo.styles.kendo_common_min.css"
        Static="True" />
    <sitefinity:ResourceFile Name="Telerik.Sitefinity.Resources.Scripts.Kendo.kendo.all.min.js" />
    <sitefinity:ResourceFile Name="EventsRegistration.Resources.KendoWidgetStyle.css"
        Static="true" AssemblyInfo="EventsRegistration.FormsControlCustom, EventsRegistration" />
</sitefinity:ResourceLinks>
<ul class="hidden" id="storage">
    <li class="TemplateDropDownIndex"></li>
    <li class="GridIndex"></li>
</ul>
<div class="sfContentViews">
    <div class="sfColWrapper sfEqualCols sfModeSelector sfClearfix sfNavDesignerCtrl sfNavDim">
        <div id="RotatorDesignChoice" class="sfLeftCol">
            <h2 class="sfStep1">
                Select a Form</h2>
            <br />
            <table id="grid">
                <thead>
                    <tr>
                        <th>
                            Form Title
                        </th>
                        <th>
                            &nbsp
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="RotatorOptions" class="sfRightCol">
            <h2 class="sfStep2">
                Tune your Widget</h2>
            <br />
            <div id="example" class="k-content">
                <div class="widgetOptionWrapper">
                    <ul>
                        <li class="widgetOption"><span>&nbsp>>>&nbsp&nbsp</span><input type="radio" name="widgetScope"
                            id="singleWidget" value="ForMany"><label for="singleWidget">&nbspOne widget for all
                                events</label></li>
                        <li class="widgetOption"><span>&nbsp>>>&nbsp&nbsp</span><input type="radio" name="widgetScope"
                            id="multipleWidgets" value="ForOne"><label for="multipleWidgets">&nbspEvery event is
                                with individual widget</label></li>
                    </ul>
                </div>
            </div>
            <ul id="panelBar">
                <li>The Forms Section
                    <br />
                    <div id="radStuff">
                        <div class="sfExample">
                            <span style="font-weight: bold">Select the field that holds the MAXIMUM attendees for
                                the event: </span>
                        </div>
                        <telerik:RadComboBox ID="RadComboBoxMaxAtt" runat="server" Width="100%" Skin="Metro"
                            EmptyMessage="Choose a Field..." CssClass="comboWrapper">
                        </telerik:RadComboBox>
                        <div class="sfExample">
                            <span style="font-weight: bold">Select the field that holds the CURRENT attendees for
                                the event: </span>
                        </div>
                        <div>
                            <telerik:RadComboBox ID="RadComboBoxCurrAtt" runat="server" Width="100%" Skin="Metro"
                                EmptyMessage="Choose a Field..." CssClass="comboWrapper">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div id="attendeesSelection">
                        <span style="font-weight: bold">Please enter the max number of the attendees:</span>
                        <br />
                        <input id="numAttendees" type="number" min="0" max="10000" step="10" />
                    </div>
                    <div id="eventName">
                        <span style="font-weight: bold">Now, type the name of the event for this widget:</span>
                        <br />
                        <span style="" class="k-widget k-autocomplete k-header k-state-default">
                            <input id="eventNameInput" name="eventNameInputName" style="width: 100%; height: auto;"
                                type="custom" placeholder="e.g. MyEvent" class="k-input" /></span>
                    </div>
                    <div class="sfExample">
                        <span style="font-weight: bold">Enter the name of the Form TextBox that holds the Events
                            NAME</span>
                        <input id="autoComplete" />
                    </div>
                </li>
                <li>E-Mail Configuration
                    <div>
                        <br />
                        <input type="checkbox" id="checkBoxMail" /><span style="font-weight: bold"> Do you want
                            to receive Email notifications?</span>
                        <br />
                    </div>
                    <br />
                    <div class="sfExample" id="EmailOptions">
                        <br />
                        <span style="font-weight: bold">Enter the Email address(es) where you want to receive
                            the notifications:</span>
                        <br />
                        <span style="" class="k-widget k-autocomplete k-header k-state-default">
                            <input id="senderMail" name="SenderMail" style="width: 100%; height: auto;" type="custom"
                                placeholder="e.g. myname@example.net" class="k-input" />
                        </span>
                        <br />
                        <br />
                        <input type="checkbox" id="checkBoxNotifySender" /><span style="font-weight: bold">
                            Do you want to send Email confirmation to the attendees?</span>
                        <br />
                        <div id="attendeesNotification">
                            <br />
                            <span style="font-weight: bold">Enter the SUBJECT of the Email: </span>
                            <br />
                            <span style="" class="k-widget k-autocomplete k-header k-state-default">
                                <input id="subjectMail" style="width: 100%; height: auto;" type="text" autocomplete="off"
                                    class="k-input">
                            </span>
                            <br />
                            <br />
                            <div class="sfExample">
                                <span style="font-weight: bold">Select a message template for the confirmation message
                                    to the attendees:</span>
                                <br />
                                <span>
                                    <input id="titles" />&nbsp&nbsp&nbsp<span id="undo" class="k-group">Preview the template</span><div
                                        id="templatePreview">
                                    </div>
                                </span>
                            </div>
                        </div>
                    </div>
                </li>
                <li>E-Commerce Configuration
                    <div>
                        <br />
                        <input type="checkbox" id="checkBoxProduct" /><span style="font-weight: bold">&nbspNavigate
                            to a CheckOut page?</span>
                        <br />
                    </div>
                    <br />
                    <div id="allEcommerceOptions">
                        <div class="sfExample" id="EcommerceOptions">
                            <span style="font-weight: bold">Select a checkout page:</span>
                            <sf:PageField ID="PageSelector" runat="server" WebServiceUrl="~/Sitefinity/Services/Pages/PagesService.svc/"
                                DisplayMode="Write" />
                        </div>
                        <div id="productFieldEvents">
                            <div class="sfExample">
                                <span style="font-weight: bold">Select the field that holds the associated product name
                                    for the event: </span>
                            </div>
                            <div>
                                <telerik:RadComboBox ID="RadComboBoxProductName" runat="server" Width="100%" Skin="Metro"
                                    EmptyMessage="Choose a Field..." CssClass="comboWrapper">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div id="productSection">
                            <span style="font-weight: bold">&nbsp 1.Select the product type:&nbsp</span>
                            <br />
                            <input id="productTypeSelector" />
                            <br />
                            <span style="font-weight: bold">&nbsp 2. Now, select the product:&nbsp</span>
                            <br />
                            <input id="productSelector" />
                        </div>
                    </div>
                </li>
            </ul>
            <div class="sfStep2Options">
            </div>
        </div>
    </div>
</div>
<!-- Template for the left column of the Grid -->
<script id="gridLeftTemplate" type="text/x-kendo-tmpl">
                <div class="gridLeftColumn">
                    <span>${ Title }</span>
                </div>
</script>
<!-- Template for the dropDown -->
<script id="dropDownTemplate" type="text/x-kendo-template">
        <span>${ data.Name }</span><div class="hidden"><p>${data.BodyText}</p></div>
</script>
<!-- Template for the products dropDown -->
<script id="dropDownProductTemplate" type="text/x-kendo-template">
        <div class="listItemProduct"><span><img src="${ data.Item.Thumbnail.Url }" alt="${ data.Item.Thumbnail.Title }" height="22px"/>&nbsp&nbsp&nbsp${data.Item.Title.Value}</span></div>           </script>
