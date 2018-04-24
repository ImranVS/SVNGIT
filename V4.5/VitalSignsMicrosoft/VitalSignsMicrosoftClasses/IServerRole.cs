using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VitalSignsMicrosoftClasses
{
    interface IServerRole
    {
		void CheckServer(MonitoredItems.ExchangeServer myServer,ReturnPowerShellObjects powerShellObjects,  ref TestResults AllTestResults);
    }
}
