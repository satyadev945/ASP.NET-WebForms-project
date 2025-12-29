# 🚀 .NET Upgrade Analysis Report

**Analysis ID:** `61`

**Generated:** 2025-12-18 10:57:47

---

## 📋 Project Information

| Field | Value |
|-------|-------|
| **Project Path** | `/modernize-data/studio-data/TNT1001/APP1953/transformed-code/46/studio-workspace/dotnetbackend` |
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
| **Breaking Changes** | 11 |
| **Estimated Effort** | **320-400 hours** |
| **Upgrade Complexity** | **Complex** |

---

## ⚠️ Identified Issues

| Severity | Category | Title | File | Line | Effort |
|----------|----------|-------|------|------|--------|
| 🔴 **critical** | package-compatibility | Entity Framework 6.2.0 is incompatible with .NET 8 | `src/eShopLegacyWebForms/packages.config` | 9 | 🔴 high |
| 🔴 **critical** | package-compatibility | log4net 2.0.10 is legacy and not recommended for .NET 8 | `src/eShopLegacyWebForms/packages.config` | 11 | 🟡 medium |
| 🔴 **critical** | deprecated-api | System.Web reference is not compatible with .NET 8 | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 173 | 🔴 high |
| 🔴 **critical** | deprecated-api | System.Web.Optimization is not compatible with .NET 8 | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 175 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Global.asax application lifecycle not supported in .NET 8 | `src/eShopLegacyWebForms/Global.asax.cs` | 17 | 🔴 high |
| 🔴 **critical** | webforms-migration | Autofac.Integration.Web is Web Forms-specific | `src/eShopLegacyWebForms/Global.asax.cs` | 2 | 🟡 medium |
| 🔴 **critical** | deprecated-api | LogManager from log4net used throughout application | `src/eShopLegacyWebForms/Global.asax.cs` | 19 | 🔴 high |
| 🔴 **critical** | deprecated-api | HttpContext.Current usage is not supported in ASP.NET Core | `src/eShopLegacyWebForms/Global.asax.cs` | 43 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Session state management requires complete redesign | `src/eShopLegacyWebForms/Web.config` | 29 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Entity Framework 6 DbContext incompatible with EF Core | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 8 | 🔴 high |
| 🔴 **critical** | webforms-migration | EntityTypeConfiguration is EF6-specific | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 29 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Database.SetInitializer is EF6-specific | `src/eShopLegacyWebForms/Global.asax.cs` | 65 | 🟡 medium |
| 🟠 **high** | webforms-migration | ASPX pages must be converted to Razor Pages | `Multiple files: Default.aspx, About.aspx, Contact.aspx, Create.aspx, Delete.aspx, Details.aspx, Edit.aspx` | 1 | 🔴 high |
| 🟠 **high** | webforms-migration | Master pages must be converted to Layout pages | `src/eShopLegacyWebForms/Site.Master` | 1 | 🟡 medium |
| 🟠 **high** | webforms-migration | User control (.ascx) must be converted to Partial View or View Component | `src/eShopLegacyWebForms/ViewSwitcher.ascx` | 1 | 🟢 low |
| 🟠 **high** | webforms-migration | Page lifecycle events (Page_Load) not supported in Razor Pages | `src/eShopLegacyWebForms/Default.aspx.cs` | 22 | 🟡 medium |
| 🟠 **high** | webforms-migration | Server controls (ListView, DropDownList, etc.) not available | `src/eShopLegacyWebForms/Default.aspx` | 12 | 🔴 high |
| 🟠 **high** | webforms-migration | Web Forms validation controls must be replaced | `src/eShopLegacyWebForms/Catalog/Create.aspx` | 13 | 🟡 medium |
| 🟠 **high** | webforms-migration | Button click event handlers must be converted to POST handlers | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 30 | 🟡 medium |
| 🟠 **high** | webforms-migration | Direct control property access not available in Razor Pages | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 36 | 🟡 medium |
| 🟠 **high** | webforms-migration | SelectMethod pattern not supported in Razor Pages | `src/eShopLegacyWebForms/Catalog/Create.aspx` | 29 | 🟢 low |
| 🟠 **high** | webforms-migration | ModelState validation is different in ASP.NET Core | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 32 | 🟢 low |
| 🟠 **high** | webforms-migration | Response.Redirect must be replaced with RedirectToPage | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 48 | 🟢 low |
| 🟠 **high** | webforms-migration | RouteConfig and Web Forms routing must be converted | `src/eShopLegacyWebForms/Global.asax.cs` | 32 | 🟡 medium |
| 🟠 **high** | webforms-migration | GetRouteUrl method not available in Razor Pages | `src/eShopLegacyWebForms/Default.aspx` | 102 | 🟡 medium |
| 🟠 **high** | deprecated-api | System.Data.Entity namespace not available in EF Core | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 4 | 🟢 low |
| 🟠 **high** | webforms-migration | ScriptManager is Web Forms-specific | `src/eShopLegacyWebForms/Site.Master` | 21 | 🟡 medium |
| 🟠 **high** | configuration | Web.config must be converted to appsettings.json | `src/eShopLegacyWebForms/Web.config` | 1 | 🟡 medium |
| 🟡 **medium** | configuration | Connection string format may need adjustment for EF Core | `src/eShopLegacyWebForms/Web.config` | 12 | 🟢 low |
| 🟡 **medium** | configuration | ConfigurationManager.AppSettings not available | `src/eShopLegacyWebForms/Global.asax.cs` | 53 | 🟢 low |
| 🟡 **medium** | webforms-migration | HTTP Modules must be converted to Middleware | `src/eShopLegacyWebForms/Web.config` | 38 | 🟡 medium |
| 🟡 **medium** | package-compatibility | Autofac 4.9.1 needs upgrade for .NET 8 | `src/eShopLegacyWebForms/packages.config` | 6 | 🟡 medium |
| 🟡 **medium** | package-compatibility | Microsoft.AspNet.* packages are .NET Framework-specific | `src/eShopLegacyWebForms/packages.config` | 19 | 🟢 low |
| 🟡 **medium** | data-access | EF6 Include() pattern works differently in EF Core | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 27 | 🟢 low |
| 🟡 **medium** | data-access | EF6 EntityState pattern differs in EF Core | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 61 | 🟢 low |
| 🟡 **medium** | webforms-migration | Property injection pattern must change to constructor injection | `src/eShopLegacyWebForms/Default.aspx.cs` | 18 | 🟡 medium |
| 🟡 **medium** | webforms-migration | Page.RouteData access pattern differs in Razor Pages | `src/eShopLegacyWebForms/Default.aspx.cs` | 26 | 🟢 low |
| 🟡 **medium** | webforms-migration | DataBind() pattern not used in Razor Pages | `src/eShopLegacyWebForms/Default.aspx.cs` | 38 | 🟢 low |
| 🟡 **medium** | architecture | Synchronous data access should be converted to async | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 22 | 🟡 medium |
| 🟢 **low** | package-compatibility | Application Insights packages are outdated | `src/eShopLegacyWebForms/packages.config` | 12 | 🟢 low |
| 🟢 **low** | package-compatibility | Newtonsoft.Json should be evaluated for System.Text.Json | `src/eShopLegacyWebForms/packages.config` | 31 | 🟢 low |
| 🟢 **low** | project-structure | Old-style .csproj format must be converted to SDK-style | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 2 | 🟢 low |
| 🟢 **low** | configuration | Bundle.config not used in ASP.NET Core | `src/eShopLegacyWebForms/Bundle.config` | 1 | 🟢 low |
| 🟢 **low** | webforms-migration | IDisposable pattern on service may not be needed | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 71 | 🟢 low |
| 🟢 **low** | architecture | Consider separating ViewModels from Domain Models | `src/eShopLegacyWebForms/Default.aspx` | 12 | 🟡 medium |
| 🟢 **low** | security | ValidateRequest="false" should be reviewed | `src/eShopLegacyWebForms/Catalog/Create.aspx` | 1 | 🟢 low |
| 🟢 **low** | architecture | Consider implementing repository pattern properly | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 13 | 🟡 medium |

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
