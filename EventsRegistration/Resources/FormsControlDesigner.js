Type.registerNamespace("EventsRegistration");

var fieldsArray = new Array();
var autoText = "";
var DropdownIndex = 0;
var host;
var formName;
var FormFieldGetUrl;
var formId;
var panelBarIndex;
var maxAtt;
var productType;
var productId;

EventsRegistration.FormsControlCustomDesigner = function (element) {
    this.testProperty = "I am property";

    EventsRegistration.FormsControlCustomDesigner.initializeBase(this, [element]);
    this._pageSelector = null;
    this._comboMaxAtt = null;
    this._comboCurrAtt = null;
    this._comboProductName = null;
    this._resizeControlDesignerDelegate = null;
    this._beforeSaveChangesDelegate = null;
}

EventsRegistration.FormsControlCustomDesigner.prototype = {
    /* --------------------------------- set up and tear down --------------------------------- */

    initialize: function () {
        EventsRegistration.FormsControlCustomDesigner.callBaseMethod(this, 'initialize');
        this._pageLoadDelegate = Function.createDelegate(this, this._pageLoadHandler);
        Sys.Application.add_load(this._pageLoadDelegate);

        this._beforeSaveChangesDelegate = Function.createDelegate(this, this._beforeSaveChanges);
        this.get_propertyEditor().add_beforeSaveChanges(this._beforeSaveChangesDelegate);
    },

    dispose: function () {
        EventsRegistration.FormsControlCustomDesigner.callBaseMethod(this, 'dispose');

         if (this._beforeSaveChangesDelegate) {
            this.get_propertyEditor().remove_beforeSaveChanges(this._beforeSaveChangesDelegate);
            delete this._beforeSaveChangesDelegate;
        }
    },

    /* --------------------------------- public methods --------------------------------- */
    // implementation of IControlDesigner: Forces the control to refersh from the control Data
    refreshUI: function () {
        var data = this.get_controlData();
        host = data.Host;        
        $("#storage li.TemplateDropDownIndex").html(data.TemplateIndexDropDown);
        autoText = data.FormFieldForEvents;
        formId = data.FormId;
        panelBarIndex = data.PanelBarIndex;
        maxAtt = data.MaxAttendeesWidget;
        productType = data.ProductTypeWidget;
        productId = data.ProductIdWidget;

        this.resizeEvents();
        // populate the controls
        $("#checkBoxMail").attr("checked", data.IncludeNotification);
        if (($("#checkBoxMail").is(':checked') == true && $("div#EmailOptions").hasClass("hidden") == true) || ($("#checkBoxMail").is(':checked') == false && $("div#EmailOptions").hasClass("hidden") == false)) $("div#EmailOptions").toggleClass("hidden");
        $("#checkBoxProduct").attr("checked", data.IncludeCheckoutPage);
        if (($("#checkBoxProduct").is(':checked') == true && $("div#allEcommerceOptions").hasClass("hidden") == true) || ($("#checkBoxProduct").is(':checked') == false && $("div#allEcommerceOptions").hasClass("hidden") == false)) $("div#allEcommerceOptions").toggleClass("hidden");
        $("#checkBoxNotifySender").attr("checked", data.SendEmailToClient);
         if (($("#checkBoxNotifySender").is(':checked') == true && $("div#attendeesNotification").hasClass("hidden") == true) || ($("#checkBoxNotifySender").is(':checked') == false && $("div#attendeesNotification").hasClass("hidden") == false)) $("div#attendeesNotification").toggleClass("hidden");

        // handle the check box for the scope of the widget and hide show elements based on this
        this._handleWidgetScope(data.SingleWidgetPerEachEvent);

        var comboBoxMax = this.get_comboMaxAtt();
        if (data.FieldMaxAttendees != null) {
            comboBoxMax.set_value(data.FieldMaxAttendees);
            comboBoxMax.set_text(data.FieldMaxAttendees);
        }

        var comboBoxCurr = this.get_comboCurrAtt();
        if (data.FieldCurrAttendees != null) {
            comboBoxCurr.set_value(data.FieldCurrAttendees);
            comboBoxCurr.set_text(data.FieldCurrAttendees);
        }

        var comboBoxProductName = this.get_comboProductName();
        if (data.FieldProductName != null) {
        	comboBoxProductName.set_value(data.FieldProductName);
            comboBoxProductName.set_text(data.FieldProductName);
        }

        $("#subjectMail").val(data.MailSubject);
        $("#senderMail").val(data.ToAddresses);
        $("#eventNameInput").val(data.EventsName);

        // load selected checkout page
        var pageSelector = this.get_pageSelector();
        var pageid = data.CheckOutPageId;
        if (pageid) pageSelector.set_value(pageid);
    },

    // implementation of IControlDesigner: forces the designer view to apply the changes on UI to the control Data
    applyChanges: function () {
        // save selected page
        var controlData = this._propertyEditor.get_control();
        // start to save the data
        controlData.IncludeNotification = $("#checkBoxMail").is(':checked');
        controlData.IncludeCheckoutPage = $("#checkBoxProduct").is(":checked");
        controlData.SendEmailToClient = $("#checkBoxNotifySender").is(":checked");
        controlData.PanelBarIndex = $("#panelBar").data("kendoPanelBar").element.children("li.k-state-active").index();
        //controlData.MaxAttendeesWidget = $("#numAttendees").data("kendoNumericTextBox").value();
        controlData.MaxAttendeesWidget = $("#numAttendees").val();
        controlData.EventsName  = $("#eventNameInput").val();

        var comboMaxSelected = this.get_comboMaxAtt().get_selectedItem();
        if (comboMaxSelected) controlData.FieldMaxAttendees = comboMaxSelected.get_text();

        var comboCurrSelected = this.get_comboCurrAtt().get_selectedItem();
        if (comboCurrSelected) controlData.FieldCurrAttendees = comboCurrSelected.get_text();

        var comboProductNameSelected = this.get_comboProductName().get_selectedItem();
        if (comboProductNameSelected) controlData.FieldProductName = comboProductNameSelected.get_text();

        var selectedPageId = this.get_pageSelector().get_value();
        if (selectedPageId) controlData.CheckOutPageId = selectedPageId;

        if (formId != "") {
            controlData.FormId = formId;
            parseInt($("#storage li.GridIndex").html());
        }

        var autoComplete = $("#autoComplete").data("kendoAutoComplete").value();
        if (autoComplete != "") controlData.FormFieldForEvents = autoComplete;

        controlData.MailSubject = $("#subjectMail").val();
        controlData.ToAddresses = $("#senderMail").val();

        var templateDropDown = $("#titles").data("kendoDropDownList");
        if (templateDropDown.current() != null) {
            controlData.TemplateIndexDropDown = templateDropDown.current().index();
            controlData.TemplateId = templateDropDown.value();
        }

        controlData.ProductTypeWidget = $("#productTypeSelector").data("kendoDropDownList").value();
        if ($("#productSelector").data("kendoDropDownList").text() == "") controlData.ProductIdWidget = "";
        else controlData.ProductIdWidget = $("#productSelector").data("kendoDropDownList").value();

        var widgetScope = $("input[name='widgetScope']:checked").val();
        if (widgetScope == "ForOne") controlData.SingleWidgetPerEachEvent = true;
        else controlData.SingleWidgetPerEachEvent = false;
    },

    /* ------------------------------------- events ------------------------------------------- */
    // add resize events
    resizeEvents: function () {
        this._resizeControlDesignerDelegate = Function.createDelegate(this, this._resizeControlDesigner);

        var s = this.get_pageSelector();
        s.add_selectorOpened(this._resizeControlDesigner);
        s.add_selectorClosed(this._resizeControlDesigner);
    },

    _pageLoadHandler: function () {
        // handle the checkbox for the notifications
        $("#checkBoxMail").click(function () {
            if ($(this).is(':checked') == false) {
                $("div#EmailOptions").addClass("hidden");
            } else {
                $("div#EmailOptions").removeClass("hidden");
            }
            setTimeout("dialogBase.resizeToContent()", 100);
        });

        // handle the check box for the page
        $("#checkBoxProduct").click(function () {
            if ($(this).is(':checked') == false) {
                $("div#allEcommerceOptions").addClass("hidden");
            } else {
                $("div#allEcommerceOptions").removeClass("hidden");
            }
            setTimeout("dialogBase.resizeToContent()", 100);
        });

        // handle the check box attendees Notification
        $("#checkBoxNotifySender").click(function () {
            if ($(this).is(':checked') == false) {
                $("div#attendeesNotification").addClass("hidden");
            } else {
                $("div#attendeesNotification").removeClass("hidden");
            }
            setTimeout("dialogBase.resizeToContent()", 100);
        });

        // hide or show sections of the e-comerce section
        $('input[name=widgetScope]:radio').change(function(){
            if ($("input[name='widgetScope']:checked").val() == 'ForMany') {
                $("#attendeesSelection, #eventName, #productSection").addClass("hidden");
                $("div#productFieldEvents").removeClass("hidden");
            }
            else {
            	$("#attendeesSelection, #eventName, #productSection").removeClass("hidden");
                $("div#productFieldEvents").addClass("hidden");
            }
            setTimeout("dialogBase.resizeToContent()", 50);
         });

        $("#panelBar").kendoPanelBar({
            expandMode: "single",
            select: this._panelBarOnSelect
        });

        var dropDownIndex = parseInt($("#storage li.TemplateDropDownIndex").html());
        $("#titles").kendoDropDownList({
            dataTextField: "Name",
            dataValueField: "Id",
            template: $("#dropDownTemplate").html(),
            index: dropDownIndex,
            change: this._dropDownOnChange,
            dataSource: {
                type: "json",
                transport: {
                    read: host + "/Sitefinity/Services/Newsletters/MessageTemplate.svc/",
                    dataType: "json"
                },
                schema: {
                    data: function (data) {
                        return data.Items;
                    }
                }
            }
        });
        
        // activate a panel harcoded, but I can automate it later on to persist the clietn's activity
        $("#panelBar").data("kendoPanelBar").select(this._getItem(panelBarIndex)).expand(this._getItem(panelBarIndex));

        $("#grid").kendoGrid({
            dataSource: {
                type: "json",
                transport: {
                    read: host + "/Sitefinity/Services/Forms/FormsService.svc/",
                    dataType: "json"
                },
                schema: {
                    data: function (data) {
                        return data.Items;
                    },
                    model: {
                        fields: {
                            Name: { type: "string" },
                            Owner: { type: "string" },
                            Title: { type: "string" },
                            ViewUrl: { type: "string" }
                        }
                    }
                },
                serverPaging: false,
            },
            selectable: "row",
            pageable: false,
            scrollable: {
                virtual: true
            },
            columns: [
                 {
                     template: kendo.template($("#gridLeftTemplate").html())
                 },
                 {
                     template: '<div class="gridRightColumn"><span class="gridSpan"><span class="spanImage">&nbsp &nbsp &nbsp &nbsp</span>&nbsp &nbsp<a href="' + host + '${ ViewUrl }" target="_blank">preview</a></span></div>'
                 }
             ],
            change: this._changeGridSelection,
            dataBound: this._gridDataBound
        });

        var templatePreview = $("#templatePreview"),
                        undo = $("#undo").bind("click", this._onTemplateClick);

        var onClose = function () {
            undo.show();
        };

        if (!templatePreview.data("kendoWindow")) {
            templatePreview.kendoWindow({
                actions: ["Close"],
                resizable: false,
                draggable: false,
                modal: false,
                width: "max",
                title: "Template Preview",
                close: onClose
            });
        }
        // initialize the autoComplete
        $("#autoComplete").kendoAutoComplete({
            dataSource: fieldsArray,
            suggest: true,
            filter: 'contains'
        });

        $("#autoComplete").data("kendoAutoComplete").value(autoText); // set the value of the autoComplete

        $("#EmailOptions").kendoValidator({
            rules: {
                custom: function(input) {
                    var result = true;
                    if (input.is("[name=SenderMail]") == false ) return true;
                    else {
            		var valid = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
                    var entries = input.val().split(/,|;/);
                        $.each(entries, function () {
                            if (valid.test(this.trim()) == false) {
                            	result = false;
                            }
                        });
                    }
                    return result;
                }
            },
            messages: {
            	custom: "Please enter valid e-mail addresses" // defines message for the 'custom' validation rule
            }
        });

        $("#productTypeSelector").kendoDropDownList({
            dataTextField: "TitlePlural",
            dataValueField: "ClrType",
            change: this._productTypeChanged,
            dataSource: {
                type: "json",
                transport: {
                    read: host + "/Sitefinity/Services/Ecommerce/Catalog/ProductTypeService.svc/",
                    dataType: "json"
                },
                schema: {
                    data: function (data) {
                        return data.Items;
                    }
                }
            }
        });

        $("#productSelector").kendoDropDownList({
            dataTextField: "Item.Title.Value",
            dataValueField: "Id",
            autoBind: false,
            template: $("#dropDownProductTemplate").html(),
            dataSource: {
                type: "json",
                transport: {
                    read: host + "/Sitefinity/Services/Ecommerce/Catalog/ProductService.svc/?sortExpression=Title%20ASC&skip=0&take=1",
                    dataType: "json"
                },
                schema: {
                    data: function (data) {
                        return data.Items;
                    }
                }
            }
        });

        $("#numAttendees").kendoNumericTextBox({
            format: "# Attendees",
            upArrowText: "More attendees",
            downArrowText: "Less attendees"
        });
        
        $("#numAttendees").data("kendoNumericTextBox").value(maxAtt);

        var data = this.get_propertyEditor().get_control();
        if (data.ProductTypeWidget != "") {
            $("#productTypeSelector").val(data.ProductTypeWidget);
            var productSelector = $("#productSelector").data("kendoDropDownList")
        var serviceUrlSyntetic = host + "/Sitefinity/Services/Ecommerce/Catalog/ProductService.svc/?specificProductType=" + $("#productTypeSelector").data("kendoDropDownList").value ();
        productSelector.dataSource.transport.options.read.url = serviceUrlSyntetic;
        productSelector.dataSource.read();
        $("#productSelector").val(productId);
        }

        setTimeout("dialogBase.resizeToContent()", 150);
    },

     // validator logic here:
     _beforeSaveChanges: function (sender, args) {
        var errorMessage = "";
        var toCancel = false;
        if ($("#checkBoxProduct").is(':checked') == true && this.get_pageSelector().get_value() == null) {
     	    errorMessage += "Please, select a checkout page if you want to have one \n";
            toCancel = true;
        }

        if ($("#autoComplete").data("kendoAutoComplete").value() == "") {
            	errorMessage += "Please enter the form field name that will store the event's title value in the responses \n";
                toCancel = true;
        }

        if (formId == null) {
        	errorMessage += "Please select a form from the table on the left \n";
            toCancel = true;
        }

            var currAtt = this.get_comboCurrAtt().get_selectedItem();
            var maxAtt = this.get_comboMaxAtt().get_selectedItem();
            if (maxAtt == null) {
            	errorMessage += "Please, select the field for the maximum atendees from the drop down \n";
                toCancel = true;
            }
            else {
                if (currAtt) {
                    if (currAtt.get_text() == maxAtt.get_text()) {
                        errorMessage += "The current attendees and the maximum atendees fields must be different \n";
                        toCancel = true;
                    }
                }
            }

            if (currAtt == null) {
                errorMessage += "Please, select the field for the current atendees from the drop down \n";
                toCancel = true;
            }

        if ($("input[name='widgetScope']:checked").val() == 'ForMany') {
        	// the validator's logic when the widget if for all events

            if ($("#checkBoxProduct").is(':checked') == true && this.get_comboProductName().get_selectedItem() == null) {
            	errorMessage += "Please, select the custom events field that holds the events' name \n";
                toCancel = true;
            }
            
        }
        else {
        	// the validator's logic when we have different widgets for the different events
            if ($("#numAttendees").data("kendoNumericTextBox").value() <= 0) {
            	errorMessage += "Please select a positive number for the maximum attendees for this event \n";
                toCancel = true;
            }

            if ($("#eventNameInput").val() == ""){
                errorMessage += "Please enter the name for the event you want to associate this widget with \n"
                toCancel = true;
            }

            var kendoList = $("#productSelector").data("kendoDropDownList");
            if (kendoList.value() == null || kendoList.value() == "") {
            	rrorMessage += "Please also select an associated product from the e-commerce section drop-down  \n"
                toCancel = true;
            }
        }
        
        if ($("#checkBoxMail").is(':checked') == true &&  $("#senderMail").val() == "") {
        	errorMessage += "If you want to receive notifications, please select the e-mails of the eligible persons \n";
            toCancel = true;
        }

        if ( $("#checkBoxNotifySender").is(":checked") == true)  {
        	if ($("#subjectMail").val() == "") {
                errorMessage += "If you want to send e-mails to subscribers, please provide a subject for the mail \n";
                toCancel = true;
            }
            if ($("#titles").data("kendoDropDownList").value() == "")
            {
            	errorMessage += "If you want to send e-mails to subscribers, please select a valid e-mail template in the drop down \n";
                toCancel = true;
            }
        }
        
        args.set_cancel(toCancel);
        if (errorMessage != "") alert(errorMessage);
    },

    /* --------------------------------- event handlers --------------------------------- */

    _commandHandler: function (sender, args) {
        this._radBox = sender;
        var jake = args.get_item().get_value();
        jQuery("#LabelRadListBox").text(jake);
    },

    _resizeControlDesigner: function () {
        setTimeout("dialogBase.resizeToContent()", 100);
    },

    _dropDownOnChange: function () {
        $("#storage li.TemplateDropDownIndex").html($("#titles").data("kendoDropDownList").current().index());
    },

    _productTypeChanged: function() {
        var productType = this.value();
        var productSelector =  $("#productSelector").data("kendoDropDownList");
        productSelector.text("");
        productSelector.dataSource.transport.options.read.url = host + "/Sitefinity/Services/Ecommerce/Catalog/ProductService.svc/?specificProductType=" + productType;
        productSelector.dataSource.read();
    },

    _onTemplateClick: function () {
        var rootItem = $("#titles").data("kendoDropDownList");
        var undo = $("#undo");
        var DropDownIndex = parseInt($("#storage li.TemplateDropDownIndex").html());
        var txt = rootItem.ul.children("li").eq(DropDownIndex).children("div.hidden").children().html();
        var content = txt.replace(/&lt;/g, '<');
        content = content.replace(/&gt;/g, '>');
        $("#templatePreview").empty();
        $("#templatePreview").append(content);
        $("#templatePreview").data("kendoWindow").open();
        undo.hide();
    },

    /* --------------------------------- private methods --------------------------------- */

    _test: function (message) {
        alert(message.toString());
    },

    _handleWidgetScope: function(widgetScope) {
        if (widgetScope == true) {
            $("input[name='widgetScope']:eq(1)").attr('checked', 'checked');
             $("#attendeesSelection, #eventName, #productSection").removeClass("hidden");
        }
        else {
        	$("input[name='widgetScope']:eq(0)").attr('checked', 'checked');
            $("#attendeesSelection, #eventName, #productSection").addClass("hidden");
        }
    },

    _gridDataBound: function () {
    	var objectsArray = this._data;
        var arrayIndex = 0;
        var count = 0
         $.each(objectsArray, function () {
            if (this.Id.toString() == formId.toString()) {
                    arrayIndex = count;
            }
            count ++;
                });
        var grid = $("#grid").data("kendoGrid");
        grid.select(grid.tbody.children("tr").eq(arrayIndex));
    },

    _changeGridSelection: function () {
        var grid = $("#grid").data("kendoGrid");
        var data = grid.dataItem(grid.select());
        formName = data.Name;
        FormFieldGetUrl = host + "/Sitefinity/Services/Forms/FormsService.svc/entries/" + formName + "/?managerType=&providerName=&itemType=Telerik.Sitefinity.DynamicTypes.Model." + formName + "&provider=&sortExpression=Title%20ASC&skip=0&take=50";

        $.ajax({
            url: FormFieldGetUrl,
            dataType: 'json',
            success: function (data) {
                fieldsArray.splice(0, fieldsArray.length);
                var items = data.Items[0];
                $.each(items, function (key, val) {
                    var thekey = new String(key);
                    if (thekey.indexOf("FormTextBox") > -1)
                        fieldsArray.push(key.toString());
                });
                
                var autoComplete = $("#autoComplete").data("kendoAutoComplete");
                if (autoComplete)
                $("#autoComplete").data("kendoAutoComplete").dataSource._data = fieldsArray;
            }
        });
        //console.log(fieldsArray.toString());
        formId = data.Id;
    },

    _getItem: function (target) {
        var panelBar = $("#panelBar").data("kendoPanelBar");
        var rootItem = panelBar.element.children("li").eq(target);
        return rootItem;
    },

    _panelBarOnSelect: function () {
        setTimeout("dialogBase.resizeToContent()", 200);
    },



    /* --------------------------------- properties --------------------------------- */

    // gets the reference to the propertyEditor control
    get_propertyEditor: function () {
        return this._propertyEditor;
    },
    // sets the reference fo the propertyEditor control
    set_propertyEditor: function (value) {
        this._propertyEditor = value;
    },

    get_comboMaxAtt: function () {
        return this._comboMaxAtt;
    },
    set_comboMaxAtt: function (value) {
        this._comboMaxAtt = value;
    },

    get_comboCurrAtt: function () {
        return this._comboCurrAtt;
    },
    set_comboCurrAtt: function (value) {
        this._comboCurrAtt = value;
    },

    get_comboProductName: function() {
    	return this._comboProductName
    },
    set_comboProductName: function (value) {
        this._comboProductName = value;
    },

    // Page Selector
    get_pageSelector: function () {
        return this._pageSelector;
    },
    set_pageSelector: function (value) {
        this._pageSelector = value;
    }
}


EventsRegistration.FormsControlCustomDesigner.registerClass('EventsRegistration.FormsControlCustomDesigner', Telerik.Sitefinity.Web.UI.ControlDesign.ControlDesignerBase);
if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();