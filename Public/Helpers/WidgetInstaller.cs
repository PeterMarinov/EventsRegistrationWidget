using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Modules.Events;
using Telerik.Sitefinity.Modules.Events.Configuration;

namespace EventsRegistration.Public.Helpers
{
    public class WidgetInstaller
    {
        public static void PreApplicationStart()
        {
            Bootstrapper.Initialized += (new EventHandler<ExecutedEventArgs>(WidgetInstaller.Bootstrapper_Initialized));
        }

        private static void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName != "RegisterRoutes" || !Bootstrapper.IsDataInitialized)
            {
                return;
            }

            IntallWidget();
        }

        public static void IntallWidget()
        {
            // define content view control
            var eventsConfig = Config.Get<EventsConfig>().ContentViewControls;
            var EventsFrontEndDefinition = eventsConfig.Elements.Where(el => el.GetKey().Equals(EventsDefinitions.FrontendDefinitionName)).SingleOrDefault();
            if (EventsFrontEndDefinition != null)
            {
                //Regestering the widget
                App.WorkWith().Module(EventsModule.ModuleName).Install().PageToolbox()
                        .ContentSection()
                            .LoadOrAddWidget<FormsControlCustom>("EventsRegistration")
                                .SetTitle("Events Registration")
                                .SetDescription("This widget is used to enable users to register for events")
                                .SetCssClass("sfLanguageSelectorIcn")
                                .Done();
            }
        }

    }
}
