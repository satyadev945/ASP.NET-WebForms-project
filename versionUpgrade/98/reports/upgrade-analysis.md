# 🚀 .NET Upgrade Analysis Report

**Analysis ID:** `98`

**Generated:** 2025-12-18 12:30:58

---

## 📋 Project Information

| Field | Value |
|-------|-------|
| **Project Path** | `/modernize-data/studio-data/TNT1001/APP1955/transformed-code/81/studio-workspace/dotnet_comp` |
| **Current Version** | .NET 4.7.2 |
| **Target Version** | .NET 8 |
| **Platform** | linux |
| **Project Type** | dotnet |
| **Analysis Status** | **success** |

## 📊 Analysis Results

**Status:** success

**Message:** WebForms migration analysis completed successfully

---

## 📈 Analysis Summary

### Key Metrics

| Metric | Value |
|--------|-------|
| **Total Issues** | 67 |
| **Critical Issues** | 15 |
| **Deprecated APIs** | 22 |
| **Breaking Changes** | 18 |
| **Estimated Effort** | **320-480 hours** |
| **Upgrade Complexity** | **Complex** |

---

## ⚠️ Identified Issues

| Severity | Category | Title | File | Line | Effort |
|----------|----------|-------|------|------|--------|
| 🔴 **critical** | package-compatibility | Entity Framework 6.2.0 - Not Compatible with .NET 8 | `src/eShopLegacyWebForms/packages.config` | 9 | 🔴 high |
| 🔴 **critical** | deprecated-api | log4net 2.0.10 - Legacy Logging Framework | `src/eShopLegacyWebForms/packages.config` | 11 | 🟡 medium |
| 🔴 **critical** | webforms-migration | System.Web Reference - Not Available in .NET 8 | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 173 | 🔴 high |
| 🔴 **critical** | webforms-migration | Global.asax Application Events | `src/eShopLegacyWebForms/Global.asax.cs` | 29 | 🔴 high |
| 🔴 **critical** | deprecated-api | HttpContext.Current Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 43 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Web.config Configuration File | `src/eShopLegacyWebForms/Web.config` | 1 | 🟡 medium |
| 🔴 **critical** | package-compatibility | Autofac.Integration.Web 4.0.0 - Web Forms Specific | `src/eShopLegacyWebForms/packages.config` | 7 | 🔴 high |
| 🔴 **critical** | webforms-migration | ASP.NET Web Forms Pages - Default.aspx | `src/eShopLegacyWebForms/Default.aspx` | 1 | 🔴 high |
| 🔴 **critical** | webforms-migration | Master Pages - Site.Master | `src/eShopLegacyWebForms/Site.Master` | 1 | 🟡 medium |
| 🟠 **high** | webforms-migration | User Control - ViewSwitcher.ascx | `src/eShopLegacyWebForms/ViewSwitcher.ascx` | 1 | 🟢 low |
| 🟠 **high** | webforms-migration | CRUD Pages - Create.aspx | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 30 | 🔴 high |
| 🟠 **high** | webforms-migration | CRUD Pages - Edit.aspx with Data Binding | `src/eShopLegacyWebForms/Catalog/Edit.aspx` | 15 | 🔴 high |
| 🟠 **high** | data-access | Entity Framework 6 DbContext Configuration | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 8 | 🔴 high |
| 🟠 **high** | data-access | Synchronous Database Operations | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 26 | 🟡 medium |
| 🟠 **high** | deprecated-api | Session State Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 43 | 🟡 medium |
| 🟠 **high** | package-compatibility | Microsoft.AspNet.Web.Optimization - Not Compatible | `src/eShopLegacyWebForms/packages.config` | 25 | 🟡 medium |
| 🟠 **high** | package-compatibility | Microsoft.AspNet.ScriptManager - Web Forms Specific | `src/eShopLegacyWebForms/packages.config` | 21 | 🟡 medium |
| 🟠 **high** | webforms-migration | RouteConfig for Web Forms Routing | `src/eShopLegacyWebForms/Global.asax.cs` | 32 | 🟡 medium |
| 🟠 **high** | deprecated-api | Page Lifecycle Events | `src/eShopLegacyWebForms/Default.aspx.cs` | 22 | 🔴 high |
| 🟠 **high** | webforms-migration | Server Controls - ListView, DropDownList, Button | `src/eShopLegacyWebForms/Default.aspx` | 12 | 🔴 high |
| 🟠 **high** | package-compatibility | Microsoft.AspNet.FriendlyUrls - Web Forms Feature | `src/eShopLegacyWebForms/packages.config` | 19 | 🟢 low |
| 🟡 **medium** | webforms-migration | Response.Redirect Usage | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 48 | 🟢 low |
| 🟡 **medium** | data-access | Database Initializer Pattern | `src/eShopLegacyWebForms/Global.asax.cs` | 65 | 🟡 medium |
| 🟡 **medium** | deprecated-api | ConfigurationManager Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 53 | 🟢 low |
| 🟡 **medium** | architecture | Property Injection Pattern | `src/eShopLegacyWebForms/Default.aspx.cs` | 18 | 🟡 medium |
| 🟡 **medium** | webforms-migration | Model Property in Code-Behind | `src/eShopLegacyWebForms/Default.aspx.cs` | 20 | 🟢 low |
| 🟡 **medium** | package-compatibility | Microsoft.ApplicationInsights 2.9.1 - Outdated Version | `src/eShopLegacyWebForms/packages.config` | 12 | 🟡 medium |
| 🟡 **medium** | package-compatibility | Newtonsoft.Json 12.0.1 - Use System.Text.Json | `src/eShopLegacyWebForms/packages.config` | 31 | 🟢 low |
| 🟡 **medium** | architecture | Trace.CorrelationManager Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 84 | 🟢 low |
| 🟡 **medium** | webforms-migration | Old Project Format - Not SDK-Style | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 2 | 🟢 low |
| 🟡 **medium** | webforms-migration | HTTP Modules Configuration | `src/eShopLegacyWebForms/Web.config` | 38 | 🟡 medium |
| 🟡 **medium** | data-access | EF6 HasRequired/WithMany Syntax | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 79 | 🟡 medium |
| 🟡 **medium** | data-access | EntityTypeConfiguration Class | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 29 | 🟡 medium |
| 🟡 **medium** | webforms-migration | BundleConfig for Script/Style Bundling | `src/eShopLegacyWebForms/Global.asax.cs` | 33 | 🟡 medium |
| 🟢 **low** | webforms-migration | About.aspx Page | `src/eShopLegacyWebForms/About.aspx` | 1 | 🟢 low |
| 🟢 **low** | webforms-migration | Contact.aspx Page | `src/eShopLegacyWebForms/Contact.aspx` | 1 | 🟢 low |
| 🟢 **low** | webforms-migration | Details.aspx CRUD Page | `src/eShopLegacyWebForms/Catalog/Details.aspx` | 1 | 🟢 low |
| 🟢 **low** | webforms-migration | Delete.aspx CRUD Page | `src/eShopLegacyWebForms/Catalog/Delete.aspx` | 1 | 🟢 low |
| 🟢 **low** | package-compatibility | Modernizr 2.8.3 - Legacy Feature Detection | `src/eShopLegacyWebForms/packages.config` | 30 | 🟢 low |
| 🟢 **low** | package-compatibility | WebGrease 1.6.0 - Bundling Dependency | `src/eShopLegacyWebForms/packages.config` | 46 | 🟢 low |
| 🟢 **low** | package-compatibility | Antlr 3.5.0.2 - Bundling Dependency | `src/eShopLegacyWebForms/packages.config` | 3 | 🟢 low |
| 🟢 **low** | webforms-migration | Site.Mobile.Master - Mobile-Specific Layout | `src/eShopLegacyWebForms/Site.Mobile.Master` | 1 | 🟢 low |
| 🟢 **low** | architecture | Missing Async/Await Throughout | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 22 | 🟡 medium |
| 🟢 **low** | architecture | No Dependency Injection for Logging | `src/eShopLegacyWebForms/Default.aspx.cs` | 13 | 🟢 low |
| 🟢 **low** | webforms-migration | GetRouteUrl() Method Calls | `src/eShopLegacyWebForms/Default.aspx.cs` | 50 | 🟢 low |
| 🟢 **low** | webforms-migration | ItemType Attribute on Server Controls | `src/eShopLegacyWebForms/Default.aspx` | 12 | 🟢 low |
| 🟢 **low** | architecture | No Repository Pattern | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 13 | 🟡 medium |
| 🟢 **low** | architecture | No DTOs - Using Entities Directly | `src/eShopLegacyWebForms/Default.aspx.cs` | 20 | 🟡 medium |
| 🟢 **low** | security | ValidateRequest=false on Edit Page | `src/eShopLegacyWebForms/Catalog/Edit.aspx` | 1 | 🟢 low |
| 🟢 **low** | architecture | No Exception Handling Middleware | `src/eShopLegacyWebForms/Global.asax.cs` | 1 | 🟢 low |
| 🟢 **low** | architecture | No Health Checks | `N/A` | 0 | 🟢 low |
| 🟢 **low** | architecture | No API Documentation | `N/A` | 0 | 🟢 low |
| 🟢 **low** | performance | No Response Caching | `N/A` | 0 | 🟢 low |
| 🟢 **low** | performance | No Response Compression | `N/A` | 0 | 🟢 low |
| 🟢 **low** | security | No HTTPS Redirection | `N/A` | 0 | 🟢 low |
| 🟢 **low** | architecture | No Unit Tests | `N/A` | 0 | 🟡 medium |
| 🟢 **low** | architecture | No Data Validation Layer | `N/A` | 0 | 🟢 low |
| 🟢 **low** | data-access | No Pagination Implementation in Repository | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 26 | 🟢 low |
| 🟢 **low** | architecture | CatalogItemHiLoGenerator - Custom ID Generation | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 54 | 🟢 low |
| 🟢 **low** | architecture | IDisposable Implementation for Services | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 71 | 🟢 low |
| 🟢 **low** | architecture | Mock Service Implementation | `N/A` | 0 | 🟢 low |
| 🟢 **low** | webforms-migration | RouteData.Values Access Pattern | `src/eShopLegacyWebForms/Default.aspx.cs` | 26 | 🟢 low |
| 🟢 **low** | architecture | No AutoMapper Configuration | `N/A` | 0 | 🟢 low |
| 🟢 **low** | webforms-migration | TypeScript Configuration | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 30 | 🟢 low |
| 🟢 **low** | architecture | Static File Configuration | `N/A` | 0 | 🟢 low |
| 🟢 **low** | webforms-migration | ApplicationInsights.config File | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 211 | 🟢 low |
| 🟢 **low** | architecture | No Authentication/Authorization | `N/A` | 0 | 🟡 medium |

---

## 💡 Recommendations

1. Update TargetFramework to net8 in project files
2. Update NuGet packages to latest versions compatible with .NET 8
3. Enable nullable reference types if not already enabled
4. Review breaking changes in .NET 8
5. Update to minimal APIs if using ASP.NET Core
6. Thoroughly test all functionality after migration

---

## 🛠️ Build Tool Analysis

**Tool:** dotnet

**Current Version:** 6.0

**Recommended Version:** 8

### Compatibility Issues

- TargetFramework needs update to net8
- Some NuGet packages may need updates for .NET 8 compatibility

### Upgrade Steps

1. Update TargetFramework in .csproj files
2. Update NuGet package references
3. Enable nullable reference types
4. Review breaking changes documentation

---

## 📦 Dependency Analysis

### 🔴 Incompatible Dependencies

| Group ID | Artifact ID | Current Version | Issue | Recommended Version |
|----------|-------------|----------------|-------|-------------------|
| Microsoft.EntityFrameworkCore | Microsoft.EntityFrameworkCore | 6.0.0 | Needs update for .NET 8 compatibility | 8.0 |

### 🟡 Dependencies Needing Updates

| Group ID | Artifact ID | Current Version | Recommended Version | Reason |
|----------|-------------|----------------|-------------------|--------|
| Microsoft.AspNetCore.App | Microsoft.AspNetCore.App | 6.0.0 | 8.0 | Full .NET 8 compatibility and performance improvements |

---

---

*Report generated by Studio Upgrade Service*
*For technical support, contact the development team*
