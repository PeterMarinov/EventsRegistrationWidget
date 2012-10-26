using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Logging;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Forms.Model;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Modules.Forms.Web.UI;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web.UI.Fields;
using System.Web;
using Telerik.Sitefinity.Modules.Events;
using Telerik.Sitefinity.Events.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Lists.Model;
using System.Web.UI;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog;
using Telerik.Sitefinity.Ecommerce.Catalog.Model;
using Telerik.Sitefinity.Modules.Ecommerce.Orders;
using Telerik.Sitefinity.Ecommerce.Orders.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Modules.Newsletters;
using Telerik.Sitefinity.Newsletters.Model;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Workflow;
using Telerik.Sitefinity.Data;


namespace EventsRegistration
{
    [Telerik.Sitefinity.Web.UI.ControlDesign.ControlDesigner(typeof(FormsControlCustomDesigner))]
    public class FormsControlCustom : FormsControl
    {

        #region Members

        private const string layoutTemplateName = "FormsNotification.Resources.FormsControl.ascx";

        private string eventUrlName;

        private string _clientEmailField;

        private const string clientEmailSubject = "Event registration";

        private const string limitReachedMsg = "Online registration for this event is not available";

        private const string errorMsg = "Oops! There is a problem with this event registration.";        

        private bool includeNotification = false;

        private bool includeCheckoutPage = false;

        private bool sendEmailToClient = false;

        private bool singleWidgetPerEachEvent = false;

        private bool terminate = false;

        private string productTypeWidget = string.Empty;

        private string eventsName = string.Empty;

        private string fieldMaxAttendees = string.Empty;

        private string fieldCurrAttendees = string.Empty;

        private string fieldProductName = string.Empty;

        private string formFieldForEvents = string.Empty;

        private string checkOutPageId = string.Empty;

        private string mailSubject = string.Empty;

        private string toAddresses = string.Empty;

        private string templateId = string.Empty;

        private int templateIndexDropDown;

        private int panelBarIndex = 0;

        private int maxAttendees = 0;

        private int currentAttendees = 0;

        private int maxAttendeesWidget = 0;

        private int currAttendees = 0;

        private Guid productIdWidget = Guid.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the layout template name for the control.
        /// </summary>
        protected override string LayoutTemplateName
        {
            get
            {
                return FormsControlCustom.layoutTemplateName;
            }
        }

        /// <summary>
        /// Checks whether to include email notification or not.
        /// </summary>

        public bool IncludeNotification
        {
            get
            {
                return this.includeNotification;
            }
            set
            {
                this.includeNotification = value;
            }
        }

        /// <summary>
        /// Used to define the scope of the widget (per one event or for all)
        /// </summary>
        public bool SingleWidgetPerEachEvent
        {
            get
            {
                return this.singleWidgetPerEachEvent;
            }
            set
            {
                this.singleWidgetPerEachEvent = value;
            }
        }

        /// <summary>
        /// Persists the name of the event in the case when a widget corresponds to single event.
        /// </summary>
        public string EventsName
        {
            get
            {
                return this.eventsName;
            }
            set
            {
                this.eventsName = value;
            }
        }

        /// <summary>
        /// Gets or sets a string representation of the CLR of the product type.
        /// </summary>
        public string ProductTypeWidget 
        {
            get
            {
                return this.productTypeWidget;
            }
            set
            {
                this.productTypeWidget = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the event's data field that holds the max attendees number.
        /// </summary>
        public string FieldMaxAttendees
        {
            get
            {
                return this.fieldMaxAttendees;
            }
            set
            {
                this.fieldMaxAttendees = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the event's data field that holds the current attendees number.
        /// </summary>
        public string FieldCurrAttendees
        {
            get
            {
                return this.fieldCurrAttendees;
            }
            set
            {
                this.fieldCurrAttendees = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the event's custom field that holds the associated product's ID
        /// </summary>
        public string FieldProductName
        {
            get 
            { 
                return this.fieldProductName;
            }
            set
            { 
                this.fieldProductName = value;
            }
        }

        /// <summary>
        /// Gets or sets the current attendees for the event.
        /// </summary>
        public int CurrAttendees
        {
            get 
            {                                
                if (currentEvent != null)
                {
                    //Holds the number of the max attendees for the respective event
                    this.currAttendees = int.Parse(currentEvent.GetValue(FieldCurrAttendees).ToString());                    
                }
                return this.currAttendees;
            }
            set 
            {
                this.currAttendees = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum attendees set on a widget level. Currently, up to 10,000 maximum attendees
        /// </summary>
        public int MaxAttendeesWidget
        {
            get
            {                
                return this.maxAttendeesWidget;
            }
            set
            {
                this.maxAttendeesWidget = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum attendees for the event.
        /// </summary>
        public int MaxAttendees
        {
            get
            {
                if (currentEvent != null)
                {
                    //Holds the number of the max attendees for the respective event
                    this.maxAttendees = int.Parse(currentEvent.GetValue(FieldMaxAttendees).ToString());
                }
                return this.maxAttendees;
            }
            set
            {
                this.maxAttendees = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the event's data field that holds the associated product from the ecommerce module.
        /// </summary>
        public string FormFieldForEvents
        {
            get
            {
                return this.formFieldForEvents;
            }
            set
            {
                this.formFieldForEvents = value;
            }
        }

        /// <summary>
        /// Checks whether the limit for the respective event has been reached.
        /// </summary>
        public bool LimitForEventReached
        {
            get
            {
                if (CurrAttendees < MaxAttendees)
                    return false;
                else
                    return true;
            }            
        }

        /// <summary>
        /// Checks whether to include checkout page or not (in case the event is paid).
        /// </summary>
        public bool IncludeCheckoutPage
        {
            get
            {
                return this.includeCheckoutPage;
            }
            set
            {
                this.includeCheckoutPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the id of the checkout page (used for paid events)
        /// </summary>
        public string CheckOutPageId
        {
            get
            {
                return this.checkOutPageId;
            }
            set
            {
                this.checkOutPageId = value;
            }
        }

        /// <summary>
        /// Gets or sets the url of the checkout page.
        /// </summary>
        private string CheckoutPageUrl
        {
            get
            {
                Guid pageId = new Guid(CheckOutPageId);
                PageManager pageManager = PageManager.GetManager();
                PageNode node = pageManager.GetPageNodes().Where(n => n.Id == pageId).FirstOrDefault();
                return node.GetUrl();
            }
            set
            {
            }
        }

        /// <summary>
        /// Checks whether to send email confirmation to the person who fills out the form.
        /// </summary>
        public bool SendEmailToClient
        {
            get
            {
                return this.sendEmailToClient;
            }
            set
            {
                this.sendEmailToClient = value;
            }
        }

        /// <summary>
        /// Gets or sets the recipient's addresses for the email notifcation.
        /// </summary>
        public string ToAddresses
        {
            get
            {
                return this.toAddresses;
            }
            set
            {
                this.toAddresses = value;
            }
        }

        /// <summary>
        /// Gets or sets the sender address for the email notifcation.
        /// </summary>
        private string FromAddress
        {
            get
            {
                var propertyValue = string.Empty;
                var smtpSettings = Config.Get<SystemConfig>().SmtpSettings;

                if (!string.IsNullOrEmpty(smtpSettings.DefaultSenderEmailAddress))
                {
                    propertyValue = smtpSettings.DefaultSenderEmailAddress;
                }
                return propertyValue.Trim();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the id of the template from the email campaigns module.
        /// </summary>
        public string TemplateId
        {
            get
            {
                return this.templateId;
            }
            set
            {
                this.templateId = value;
            }
        }

        /// <summary>
        /// Gets or sets the subject of the mail.
        /// </summary>
        public string MailSubject
        {
            get
            {
                if (this.mailSubject != String.Empty)
                {
                    return this.mailSubject;
                }
                else if (IncludeNotification & TemplateId != String.Empty)
                {
                    Guid bodyId = new Guid(TemplateId);
                    NewslettersManager manager = NewslettersManager.GetManager();
                    MessageBody messageBody = manager.GetMessageBodies().Where(b => b.Id == bodyId).SingleOrDefault();
                    return messageBody.Name;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                this.mailSubject = value;
            }
        }

        /// <summary>
        /// Gets the body of the template from the emai notifications module..
        /// </summary>
        private string MailBody
        {
            get
            {
                Guid bodyId = new Guid(TemplateId);
                NewslettersManager manager = NewslettersManager.GetManager();
                MessageBody messageBody = manager.GetMessageBodies().Where(b => b.Id == bodyId).SingleOrDefault();
                return messageBody.BodyText;
            }
        }

        /// <summary>
        /// Gets the host component of the Url.
        /// </summary>
        public string Host
        {
            get
            {
                Uri uri = HttpContext.Current.Request.Url;
                return "http://" + uri.Host + ":" + uri.Port;
            }
        }

        /// <summary>
        /// Gets or sets the EventsManager.
        /// </summary>
        private static EventsManager eventsManager
        {
            get
            {
                return EventsManager.GetManager();
            }
            set
            {
                FormsControlCustom.eventsManager = value;
            }
        }

        /// <summary>
        /// Gets the submit button of the form.
        /// </summary>
        private FormSubmitButton SubmitButton
        {
            get
            {                
                ControlTraverser tr = new ControlTraverser(this, TraverseMethod.BreadthFirst);
                var button = (FormSubmitButton)tr.SingleOrDefault(c => typeof(FormSubmitButton).IsAssignableFrom(c.GetType()));                            
                return button;
            }
        }

        /// <summary>
        /// Gets or Sets the current event.
        /// </summary>
        private Event currentEvent { get; set; }

        /// <summary>
        /// Gets or Sets the FormTextBox that holds the title of the event.
        /// </summary>
        private FormTextBox textBox { get; set; }

        /// <summary>
        /// Gets or Sets the last selected templated index.
        /// </summary>
        public int TemplateIndexDropDown
        {
            get
            {
                return this.templateIndexDropDown;
            }
            set
            {
                this.templateIndexDropDown = value;
            }
        }

        /// <summary>
        /// Gets or Sets the last selected region on the right hand side of the designer.
        /// </summary>
        public int PanelBarIndex
        {
            get
            {
                return this.panelBarIndex;
            }
            set
            {
                this.panelBarIndex = value;
            }
        }

        /// <summary>
        /// Gets or Sets the last selected region on the right hand side of the designer.
        /// </summary>
        public Guid ProductIdWidget 
        {
            get
            {
                return this.productIdWidget;
            }
            set
            {
                this.productIdWidget = value;
            }
        }

        /// <summary>
        /// Gets or Sets the ProductID for the created product in the Ecommerce module.
        /// </summary>
        public Guid ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates, whether the event registration should be terminated
        /// </summary>
        public bool Terminate
        {
            get
            {
                return this.terminate;
            }
            set
            {
                this.terminate = value;
            }
        }



        #endregion

        protected override void InitializeControls(Telerik.Sitefinity.Web.UI.GenericContainer container)
        {
            base.InitializeControls(container);


            //Injects the the css class .hideTextBox into the head section of the current page. This css class is used to hide the
            //FormTextBox that holds the title of the event
            this.Page.Header.Controls.Add(new LiteralControl("<style type=\"text/css\"> .hideTextBox {display: none !important;} </style>"));

            //Get the event by the current url
            int index = HttpContext.Current.Request.Url.AbsolutePath.LastIndexOf('/') + 1;
            eventUrlName = HttpContext.Current.Request.Url.AbsolutePath.Substring(index);

            if (!SingleWidgetPerEachEvent)
            {
                currentEvent = GetEventByUrl(eventUrlName);

                //Check whether a custom field with the name Thumbnail exists
                var productField = App.WorkWith().DynamicData().Type(typeof(Event)).Fields().Where(f => f.FieldName == FieldProductName).Get().SingleOrDefault();

                if (currentEvent != null && productField != null && !currentEvent.GetValue(FieldProductName).ToString().IsNullOrEmpty())
                    ProductId = new Guid(currentEvent.GetValue(FieldProductName).ToString());

                if (currentEvent != null && productField != null && currentEvent.GetValue(FieldProductName).ToString().IsNullOrEmpty())
                {
                    this.Visible = false;
                }
            }
            
            //Hide the text field that holds the event name
            foreach (var fieldControl in this.FieldControls)
            {
                if (fieldControl is FormTextBox && ((FormTextBox)fieldControl).MetaField.FieldName == FormFieldForEvents)
                {
                    //Holds the FormTextBox with the event's title
                    textBox = ((FormTextBox)fieldControl);

                    //Hide the FormTextBox with CSS
                    textBox.CssClass = "hideTextBox";

                    //Sets the Value of the FormTextBox to the title of the event
                    if (currentEvent != null)
                        ((FormTextBox)textBox).DefaultStringValue = currentEvent.Title.ToString();
                    else
                        ((FormTextBox)textBox).DefaultStringValue = EventsName;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);           

            //If the capacity for the event has been reached, disable the submit button
            if (LimitForEventReached && currentEvent != null)
            {
                LiteralControl objPanelText = new LiteralControl();
                objPanelText.Text = limitReachedMsg;
                this.ErrorsPanel.Controls.Add(objPanelText);
                this.ErrorsPanel.Visible = true;
                this.FormControls.Visible = false;
                SubmitButton.Enabled = false;
            }

        }


        protected override void OnBeforeFormSave(CancelEventArgs args)
        {
            base.OnBeforeFormSave(args);

            if (LimitForEventReached)
            {                
                args.Cancel = true;
            }
            if(currentEvent == null && !SingleWidgetPerEachEvent && IncludeCheckoutPage)
            {                
                LiteralControl objPanelText = new LiteralControl();
                objPanelText.Text = errorMsg;
                this.ErrorsPanel.Controls.Add(objPanelText);
                this.ErrorsPanel.Visible = true;
                SubmitButton.Enabled = false;
                args.Cancel = true;

            }
            if (IncludeCheckoutPage && ProductId == Guid.Empty && ProductIdWidget == Guid.Empty)
            {
                LiteralControl objPanelText = new LiteralControl();
                objPanelText.Text = errorMsg;
                this.ErrorsPanel.Controls.Add(objPanelText);
                this.ErrorsPanel.Visible = true;
                SubmitButton.Enabled = false;
                args.Cancel = true;
            }
        }

        /// <summary>
        /// Handles the submit button click event.
        /// </summary>
        /// <param name="control">The submit control.</param>
        /// <param name="validationGroup">The validation group.</param>
        protected override void ConfigureSubmitButton(System.Web.UI.Control control, string validationGroup)
        {
            var submit = control as FormSubmitButton;
            submit.Click += new EventHandler(submit_Click);
            base.ConfigureSubmitButton(control, validationGroup);
        }


        /// <summary>
        /// The submit button click event handler.
        /// </summary>
        /// <param name="sender">Calling control.</param>
        /// <param name="e">Event arguments.</param>
        void submit_Click(object sender, EventArgs e)
        {
            
            if (LimitForEventReached && currentEvent != null)
            {
                Terminate = true;                
            }
            if (currentEvent == null && !SingleWidgetPerEachEvent && IncludeCheckoutPage)
            {
                Terminate = true;
            }
            if (IncludeCheckoutPage && ProductId == Guid.Empty && !SingleWidgetPerEachEvent)
            {
                Terminate = true;
            }

            if (!Terminate)
            {
                if (currentEvent == null && SingleWidgetPerEachEvent)
                {

                    //Check whether an Event with the given name already exists
                    Event tempEvent = eventsManager.GetEvents().Where(ev => (ev.Title == EventsName && ev.Status == ContentLifecycleStatus.Live)).FirstOrDefault();

                    ProductId = ProductIdWidget;

                    if (tempEvent != null)
                    {
                        currentEvent = tempEvent;


                        


                        //Check whether the max attendees number has been changed. If so, update it.
                        if (int.Parse(currentEvent.GetValue(FieldMaxAttendees).ToString()) != MaxAttendeesWidget && int.Parse(currentEvent.GetValue(FieldMaxAttendees).ToString()) != 0)
                        {
                            Event master = eventsManager.Lifecycle.GetMaster(currentEvent) as Event;

                            // We have to suppress security since the current request is not authenticated yet and won't be allowed to modify the event.                             
                            using (new ElevatedModeRegion(eventsManager))
                            {

                                // Check whether the item exists
                                if (master != null)
                                {
                                    // Use the singular facade to modify the master by the master ID
                                    App.WorkWith().Event(master.Id).CheckOut().Do(ev =>
                                    {
                                        ev.SetValue(FieldMaxAttendees, MaxAttendeesWidget);
                                    })
                                            .CheckIn().Publish().SaveChanges();
                                }
                            }                        
                        }

                    }
                    else
                    {
                        // We have to suppress security since the current request is not authenticated yet and won't be allowed to create an event.                         
                        using (new ElevatedModeRegion(eventsManager))
                        {
                            // Create the event
                            currentEvent = eventsManager.CreateEvent(Guid.NewGuid());

                            // Set the event properties
                            currentEvent.Title = EventsName;
                            //If MaxAttendeesWidget == 0, there are no limitations for this event.
                            if (MaxAttendeesWidget == 0)
                                MaxAttendeesWidget = 10000;
                            currentEvent.SetValue(FieldMaxAttendees, MaxAttendeesWidget);
                            currentEvent.SetValue(FieldCurrAttendees, currentAttendees);
                            currentEvent.UrlName = Regex.Replace(EventsName.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");

                            currentEvent.PublicationDate = DateTime.Today;
                            currentEvent.ExpirationDate = DateTime.Today.AddDays(365);
                            currentEvent.ApprovalWorkflowState = "Draft";
                            currentEvent.Status = ContentLifecycleStatus.Master;

                            // Save the changes
                            eventsManager.SaveChanges();
                        }
                        // Save as a Draft
                        //var bag = new Dictionary<string, string>();
                        //bag.Add("ContentType", typeof(Event).FullName);
                        //WorkflowManager.MessageWorkflow(currentEvent.Id, typeof(Event), null, "Draft", false, bag);
                    }

                }

                //Holds the message content
                var msgContent = new StringBuilder();

                if (IncludeNotification)
                {
                    //Reads the fields controls on the form and stores the information in the msgContent StringBuilder
                    foreach (var fieldControl in this.FieldControls)
                    {
                        if (fieldControl is FormTextBox)
                        {
                            msgContent.Append(String.Format("<p><strong>{0}:</strong> {1}</p>", ((FormTextBox)fieldControl).Title, ((FormTextBox)fieldControl).Value));

                            if (((FormTextBox)fieldControl).Title.ToLower().Contains("email") || ((FormTextBox)fieldControl).Title.ToLower().Contains("e-mail") || ((FormTextBox)fieldControl).Title.ToLower().Contains("e mail"))
                            {
                                _clientEmailField = ((FormTextBox)fieldControl).Value.ToString().Trim();
                            }
                        }
                        else if (fieldControl is FormParagraphTextBox)
                        {
                            msgContent.Append(String.Format("<p><strong>{0}:</strong> {1}</p>", ((FormParagraphTextBox)fieldControl).Title, ((FormParagraphTextBox)fieldControl).Value));
                        }
                        else if (fieldControl is FormCheckboxes)
                        {
                            string choices = ((List<String>)((FormCheckboxes)fieldControl).Value).Aggregate(string.Empty, (current, item) => current + String.Format("{0},", item));
                            msgContent.Append(String.Format("<p><strong>{0}:</strong> {1}</p>", ((FormCheckboxes)fieldControl).Title, choices));

                        }
                        else if (fieldControl is FormChoiceField)
                        {
                            msgContent.Append(String.Format("<p><strong>{0}:</strong> {1}</p>", ((FormChoiceField)fieldControl).Title, ((FormChoiceField)fieldControl).Value));
                        }
                        else if (fieldControl is FieldControl)
                        {
                            var fldControl = fieldControl as FieldControl;

                            msgContent.Append(String.Format("<p><strong>{0}:</strong> {1}</p>", fldControl.Title, fldControl.Value));
                        }
                    }
                }

                if (!LimitForEventReached)
                {
                    try
                    {
                        //Send the email(s)
                        var isMsgSent = SendEmailMsg(FromAddress, ToAddresses, MailSubject, msgContent.ToString());
                    }
                    catch (Exception ex)
                    {
                    }

                    int currAttendees = int.Parse(currentEvent.GetValue(FieldCurrAttendees).ToString());
                    IncreaseAttendeesNumber(++currAttendees);

                    //If the event is paid, add the event to the shopping cart
                    if (IncludeCheckoutPage && ProductId != Guid.Empty)
                    {
                        AddEventToCart(currentEvent);
                    }
                }
            }
        }

        protected override void OnFormSaved(EventArgs args = null)
        {
            base.OnFormSaved(args);            

            if (IncludeCheckoutPage && !LimitForEventReached)
            {
                HttpContext.Current.Response.Redirect(CheckoutPageUrl);
            }
        }

        public Event GetEventByUrl(string urlName)
        {
            Event eventItem = eventsManager.GetEvents().Where(e => (e.UrlName == urlName && e.Status == ContentLifecycleStatus.Live))
                .FirstOrDefault();
            return eventItem;
        }

        private void IncreaseAttendeesNumber(int attendees)
        {
            Event master = eventsManager.Lifecycle.GetMaster(currentEvent) as Event;

            using (new ElevatedModeRegion(eventsManager))
            {
                // Check whether the item exists
                if (master != null)
                {
                    // Use the singular facade to modify the master by the master ID
                    App.WorkWith().Event(master.Id).CheckOut().Do(ev =>
                    {
                        ev.SetValue(FieldCurrAttendees, attendees);
                    })
                            .CheckIn().Publish().SaveChanges();
                }
            }
        }


        private void AddEventToCart(Event currentEvent)
        {
            CatalogManager catalog = new CatalogManager();            
            Product product = catalog.GetProduct(ProductId);

            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("shoppingCartId");
            OrdersManager orderm = new OrdersManager();
            OptionsDetails optionDetails = new OptionsDetails();

            // Check if shopping cart cookie exists in the current request.
            if (cookie == null) //if it does not exist...
            {
                CartOrder cartItem = orderm.CreateCartOrder(); //create a new cart order
                var shoppingcartid = cartItem.Id;  // that id is equal to the cookie value
                HttpCookie Cookie = new HttpCookie("shoppingCartId");  //create a new shopping cart cookie
                DateTime now = DateTime.Now; // Set the cookie value.
                Cookie.Value = shoppingcartid.ToString(); // Set the cookie expiration date.
                Cookie.Expires = now.AddYears(1);// Add the cookie.
                HttpContext.Current.Response.Cookies.Add(Cookie);  //give cart item currency of USD because it cannot be null
                cartItem.Currency = "USD"; //add the product to the cart
                orderm.AddToCart(cartItem, product, optionDetails, 1); //save all changes
                orderm.SaveChanges();
            }
            else //if the cookie does exist
            {
                Guid guid = new Guid(cookie.Value.ToString()); //get the cookie value as the guid
                CartOrder cartItem = orderm.GetCartOrder(guid); //get the cart based on the cookie value
                orderm.AddToCart(cartItem, product, optionDetails, 1); // add the item to the cart
                orderm.SaveChanges(); //save changes
            }
        }

        /// <summary>
        /// Sends the email message
        /// </summary>
        /// <param name="fromAddr">Sender</param>
        /// <param name="toAddr">Who the email is to (comma separate if more than one)</param>
        /// <param name="ccAddr">Who should be cc'ed (comma separate if more than one)</param>
        /// <param name="subject">Subject line of the email</param>
        /// <param name="msgBody">content of the email</param>
        /// <returns></returns>
        private bool SendEmailMsg(string fromAddr, string toAddr, string subject, string msgBody)
        {
            if (!string.IsNullOrEmpty(fromAddr) && !string.IsNullOrEmpty(toAddr))
            {
                var mailServer = new SmtpClient();
                var mailMsg = new MailMessage();

                var clientMailMsg = new MailMessage();

                var smtpSettings = Config.Get<SystemConfig>().SmtpSettings;

                if (!smtpSettings.Host.IsNullOrWhitespace())
                {
                    mailServer.Host = smtpSettings.Host;
                    mailServer.Port = smtpSettings.Port;
                    mailServer.EnableSsl = smtpSettings.EnableSSL;
                    mailServer.Timeout = smtpSettings.Timeout;

                    if (!string.IsNullOrEmpty(smtpSettings.UserName))
                    {
                        mailServer.UseDefaultCredentials = false;
                        mailServer.Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password);
                    }
                }

                if (IsValidEmail(fromAddr))
                    mailMsg.From = new MailAddress(fromAddr);


                var arrToAddr = toAddr.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var emailAddr in arrToAddr.Select(s => s.Trim()).Where(IsValidEmail))
                {
                    mailMsg.To.Add(new MailAddress(emailAddr));
                }

                mailMsg.IsBodyHtml = true;
                mailMsg.Subject = subject;
                mailMsg.Body = "<div>" + msgBody + "</div>";

                //If checked, send email confirmation to the client
                if (SendEmailToClient && IsValidEmail(_clientEmailField))
                {
                    clientMailMsg.To.Add(_clientEmailField);
                    clientMailMsg.From = new MailAddress(fromAddr);
                    clientMailMsg.IsBodyHtml = true;
                    clientMailMsg.Subject = MailSubject;
                    clientMailMsg.Body = MailBody;
                }

                try
                {
                    mailServer.Send(mailMsg);

                    if (SendEmailToClient && IsValidEmail(_clientEmailField))
                    {
                        mailServer.Send(clientMailMsg);
                    }

                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the email address.
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if valid, otherwise false</returns>
        private bool IsValidEmail(string email)
        {
            var pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
                            (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";
            var check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            bool valid = false;
            valid = !string.IsNullOrEmpty(email) && check.IsMatch(email);
            return valid;
        }
    }
}
