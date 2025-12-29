# 🚀 .NET Upgrade Analysis Report

**Analysis ID:** `108`

**Generated:** 2025-12-19 10:30:12

---

## 📋 Project Information

| Field | Value |
|-------|-------|
| **Project Path** | `/modernize-data/studio-data/TNT1001/APP1965/transformed-code/84/studio-workspace/backendcomp` |
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
| **Total Issues** | 47 |
| **Critical Issues** | 12 |
| **Deprecated APIs** | 18 |
| **Breaking Changes** | 12 |
| **Estimated Effort** | **320-480 hours** |
| **Upgrade Complexity** | **Complex** |

---

## ⚠️ Identified Issues

| Severity | Category | Title | File | Line | Effort |
|----------|----------|-------|------|------|--------|
| 🔴 **critical** | package-compatibility | Entity Framework 6.2.0 Not Compatible with .NET 8 | `src/eShopLegacyWebForms/packages.config` | 9 | 🔴 high |
| 🔴 **critical** | package-compatibility | log4net 2.0.10 Legacy Logging Framework | `src/eShopLegacyWebForms/packages.config` | 11 | 🟡 medium |
| 🔴 **critical** | package-compatibility | Autofac 4.9.1 Web Integration Not Compatible | `src/eShopLegacyWebForms/packages.config` | 7 | 🔴 high |
| 🔴 **critical** | webforms-migration | System.Web References Throughout Application | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 173 | 🔴 high |
| 🔴 **critical** | deprecated-api | HttpContext.Current Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 43 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Global.asax Application Events | `src/eShopLegacyWebForms/Global.asax.cs` | 29 | 🔴 high |
| 🔴 **critical** | webforms-migration | Web.config Configuration System | `src/eShopLegacyWebForms/Web.config` | 1 | 🟡 medium |
| 🔴 **critical** | deprecated-api | Session State with InProc Mode | `src/eShopLegacyWebForms/Web.config` | 29 | 🟡 medium |
| 🔴 **critical** | webforms-migration | HTTP Modules for Autofac Integration | `src/eShopLegacyWebForms/Web.config` | 38 | 🔴 high |
| 🔴 **critical** | webforms-migration | Entity Framework 6 DbContext with System.Data.Entity | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 8 | 🔴 high |
| 🔴 **critical** | deprecated-api | Database.SetInitializer for EF6 | `src/eShopLegacyWebForms/Global.asax.cs` | 65 | 🟡 medium |
| 🔴 **critical** | webforms-migration | System.Web.UI.Page Base Class | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 9 | 🔴 high |
| 🟠 **high** | webforms-migration | ASP.NET Server Controls (asp:TextBox, asp:DropDownList, etc.) | `src/eShopLegacyWebForms/Catalog/Create.aspx` | 12 | 🔴 high |
| 🟠 **high** | webforms-migration | Master Pages (.master files) | `src/eShopLegacyWebForms/Site.Master` | 1 | 🟡 medium |
| 🟠 **high** | webforms-migration | User Control (.ascx file) | `src/eShopLegacyWebForms/ViewSwitcher.ascx` | 1 | 🟢 low |
| 🟠 **high** | deprecated-api | Page Lifecycle Events (Page_Load) | `src/eShopLegacyWebForms/Default.aspx.cs` | 22 | 🟡 medium |
| 🟠 **high** | deprecated-api | Response.Redirect Usage | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 48 | 🟢 low |
| 🟠 **high** | webforms-migration | ModelState.IsValid from Web Forms | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 32 | 🟡 medium |
| 🟠 **high** | package-compatibility | Microsoft.AspNet.Web.Optimization for Bundling | `src/eShopLegacyWebForms/packages.config` | 25 | 🟡 medium |
| 🟠 **high** | deprecated-api | log4net ILog Static Logger | `src/eShopLegacyWebForms/Global.asax.cs` | 19 | 🟡 medium |
| 🟠 **high** | deprecated-api | RouteConfig.RegisterRoutes with RouteTable | `src/eShopLegacyWebForms/Global.asax.cs` | 32 | 🟡 medium |
| 🟠 **high** | webforms-migration | ScriptManager and Script Bundling | `src/eShopLegacyWebForms/Site.Master` | 21 | 🟡 medium |
| 🟠 **high** | deprecated-api | Property Injection via Autofac Web Integration | `src/eShopLegacyWebForms/Default.aspx.cs` | 18 | 🟡 medium |
| 🟠 **high** | deprecated-api | Entity Framework 6 Include() for Eager Loading | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 27 | 🟢 low |
| 🟠 **high** | deprecated-api | EntityState.Modified for Updates | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 61 | 🟢 low |
| 🟠 **high** | webforms-migration | Data Binding with DataSource and DataBind() | `src/eShopLegacyWebForms/Default.aspx.cs` | 37 | 🟡 medium |
| 🟠 **high** | deprecated-api | ConfigurationManager.AppSettings | `src/eShopLegacyWebForms/Global.asax.cs` | 53 | 🟢 low |
| 🟠 **high** | webforms-migration | GetRouteUrl for Pagination URLs | `src/eShopLegacyWebForms/Default.aspx.cs` | 50 | 🟢 low |
| 🟡 **medium** | package-compatibility | ApplicationInsights Legacy Package Versions | `src/eShopLegacyWebForms/packages.config` | 12 | 🟢 low |
| 🟡 **medium** | deprecated-api | EntityTypeConfiguration<T> Fluent API | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 29 | 🟡 medium |
| 🟡 **medium** | deprecated-api | HasRequired() Relationship Configuration | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 79 | 🟡 medium |
| 🟡 **medium** | webforms-migration | Page.RouteData.Values Access | `src/eShopLegacyWebForms/Default.aspx.cs` | 26 | 🟢 low |
| 🟡 **medium** | webforms-migration | Button Click Event Handlers | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 30 | 🟡 medium |
| 🟡 **medium** | webforms-migration | ASP.NET Validators (RequiredFieldValidator, RangeValidator) | `src/eShopLegacyWebForms/Catalog/Create.aspx` | 13 | 🟡 medium |
| 🟡 **medium** | deprecated-api | Synchronous Database Operations | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 32 | 🟡 medium |
| 🟡 **medium** | deprecated-api | Dispose Pattern in Service Classes | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 71 | 🟢 low |
| 🟡 **medium** | package-compatibility | Autofac 4.9.1 May Need Upgrade | `src/eShopLegacyWebForms/packages.config` | 6 | 🟢 low |
| 🟡 **medium** | webforms-migration | Target Framework v4.7.2 | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 19 | 🔴 high |
| 🟡 **medium** | webforms-migration | SelectMethod for Data Binding | `src/eShopLegacyWebForms/Catalog/Create.aspx` | 29 | 🟢 low |
| 🟡 **medium** | security | ValidateRequest="false" on Create Page | `src/eShopLegacyWebForms/Catalog/Create.aspx` | 1 | 🟢 low |
| 🟢 **low** | package-compatibility | Newtonsoft.Json 12.0.1 | `src/eShopLegacyWebForms/packages.config` | 31 | 🟢 low |
| 🟢 **low** | webforms-migration | Bundle.config File | `src/eShopLegacyWebForms/Bundle.config` | 1 | 🟢 low |
| 🟢 **low** | deprecated-api | LogicalThreadContext for Log4net | `src/eShopLegacyWebForms/Global.asax.cs` | 72 | 🟢 low |
| 🟢 **low** | webforms-migration | runat="server" Attribute Throughout | `Multiple .aspx files` | 0 | 🟢 low |
| 🟢 **low** | webforms-migration | ContentPlaceHolder for Master Page Sections | `src/eShopLegacyWebForms/Site.Master` | 57 | 🟢 low |
| 🟢 **low** | deprecated-api | Scripts.Render() Helper | `src/eShopLegacyWebForms/Site.Master` | 12 | 🟢 low |
| 🟢 **low** | webforms-migration | ListView Control for Product List | `src/eShopLegacyWebForms/Default.aspx` | 0 | 🟢 low |

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
