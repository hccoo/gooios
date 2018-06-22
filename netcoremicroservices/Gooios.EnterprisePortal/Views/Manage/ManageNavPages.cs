using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gooios.EnterprisePortal.Views.Manage
{
    public static class ManageNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Index => "Index";

        public static string ChangePassword => "ChangePassword";

        public static string GoodsManagement => "GoodsManagement";

        public static string ServiceManagement => "ServiceManagement";

        public static string ServicerManagement => "ServicerManagement";

        public static string TopicManagement => "TopicManagement";

        public static string ServiceCommentsManagement => "ServiceCommentsManagement";

        public static string ServicerCommentsManagement => "ServicerCommentsManagement";

        public static string GoodsCommentsManagement => "GoodsCommentsManagement";

        public static string ReservationManagement => "ReservationManagement";

        public static string OrderManagement => "OrderManagement";

        public static string ExternalLogins => "ExternalLogins";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string GoodsManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, GoodsManagement);

        public static string ServiceManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, ServiceManagement);

        public static string ServicerManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, ServicerManagement);

        public static string ServiceCommentsManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, ServiceCommentsManagement);

        public static string ServicerCommentsManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, ServicerCommentsManagement);

        public static string TopicManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, TopicManagement);

        public static string GoodsCommentsManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, GoodsCommentsManagement);

        public static string ReservationManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, ReservationManagement);

        public static string OrderManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, OrderManagement);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);
    
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
