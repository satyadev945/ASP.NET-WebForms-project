# 🚀 .NET Upgrade Analysis Report

**Analysis ID:** `55`

**Generated:** 2025-12-18 08:21:35

---

## 📋 Project Information

| Field | Value |
|-------|-------|
| **Project Path** | `/modernize-data/studio-data/TNT1001/APP1947/transformed-code/43/studio-workspace/dotnetCOmp` |
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
| **Estimated Effort** | **120-160 hours** |
| **Upgrade Complexity** | **Complex** |

---

## ⚠️ Identified Issues

| Severity | Category | Title | File | Line | Effort |
|----------|----------|-------|------|------|--------|
| 🔴 **critical** | package-compatibility | Entity Framework 6.2.0 Not Compatible with .NET 8 | `packages.config` | 9 | 🔴 high |
| 🔴 **critical** | package-compatibility | log4net 2.0.10 Legacy Logging Not Recommended for .NET 8 | `packages.config` | 11 | 🟡 medium |
| 🔴 **critical** | deprecated-api | System.Web Reference Not Available in .NET 8 | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 173 | 🔴 high |
| 🔴 **critical** | deprecated-api | HttpContext.Current Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 43 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Web Forms Page Lifecycle Events | `src/eShopLegacyWebForms/Default.aspx.cs` | 22 | 🔴 high |
| 🔴 **critical** | webforms-migration | ASP.NET Web Forms Server Controls | `src/eShopLegacyWebForms/Default.aspx` | 12 | 🔴 high |
| 🔴 **critical** | webforms-migration | Master Pages Not Supported | `src/eShopLegacyWebForms/Site.Master` | 1 | 🟡 medium |
| 🔴 **critical** | configuration | Web.config Not Used in .NET 8 | `src/eShopLegacyWebForms/Web.config` | 1 | 🟡 medium |
| 🔴 **critical** | application-lifecycle | Global.asax Application Events | `src/eShopLegacyWebForms/Global.asax.cs` | 29 | 🟡 medium |
| 🔴 **critical** | dependency-injection | Autofac Web Forms Integration | `src/eShopLegacyWebForms/Global.asax.cs` | 50 | 🟡 medium |
| 🔴 **critical** | data-access | Entity Framework 6 DbContext Implementation | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 8 | 🔴 high |
| 🔴 **critical** | data-access | Entity Framework 6 Configuration API | `src/eShopLegacyWebForms/Models/CatalogDBContext.cs` | 29 | 🔴 high |
| 🟠 **high** | package-compatibility | Autofac.Web Package Not Compatible | `packages.config` | 7 | 🟡 medium |
| 🟠 **high** | package-compatibility | Microsoft.AspNet Web Forms Packages | `packages.config` | 19 | 🟡 medium |
| 🟠 **high** | session-state | Session State Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 43 | 🟡 medium |
| 🟠 **high** | http-modules | HTTP Modules Configuration | `src/eShopLegacyWebForms/Web.config` | 38 | 🟡 medium |
| 🟠 **high** | data-access | Synchronous Database Operations | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 22 | 🟡 medium |
| 🟠 **high** | data-access | Database Initializer Pattern | `src/eShopLegacyWebForms/Global.asax.cs` | 65 | 🟡 medium |
| 🟠 **high** | routing | Web Forms Routing Configuration | `src/eShopLegacyWebForms/Global.asax.cs` | 32 | 🟡 medium |
| 🟠 **high** | bundling | System.Web.Optimization Bundling | `src/eShopLegacyWebForms/Global.asax.cs` | 33 | 🟢 low |
| 🟠 **high** | logging | log4net Static Logger Pattern | `src/eShopLegacyWebForms/Default.aspx.cs` | 13 | 🟡 medium |
| 🟠 **high** | project-structure | Old-Style Project File Format | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 2 | 🟡 medium |
| 🟠 **high** | target-framework | Target Framework .NET Framework 4.7.2 | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 19 | 🔴 high |
| 🟠 **high** | webforms-migration | ViewSwitcher User Control | `src/eShopLegacyWebForms/ViewSwitcher.ascx` | 1 | 🟢 low |
| 🟠 **high** | application-insights | Application Insights Web Forms Integration | `packages.config` | 14 | 🟢 low |
| 🟠 **high** | webforms-migration | Data Binding Expressions | `src/eShopLegacyWebForms/Default.aspx` | 58 | 🟡 medium |
| 🟠 **high** | webforms-migration | Page Property Injection | `src/eShopLegacyWebForms/Default.aspx.cs` | 18 | 🟢 low |
| 🟡 **medium** | configuration | ConfigurationManager Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 53 | 🟢 low |
| 🟡 **medium** | data-access | Include Navigation Property Loading | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 27 | 🟢 low |
| 🟡 **medium** | data-access | Entry State Modification | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 61 | 🟢 low |
| 🟡 **medium** | dependency-management | packages.config Format | `packages.config` | 1 | 🟢 low |
| 🟡 **medium** | page-lifecycle | Response.Redirect Usage | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 48 | 🟢 low |
| 🟡 **medium** | validation | ModelState Validation | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 32 | 🟢 low |
| 🟡 **medium** | webforms-migration | GetRouteUrl Pattern | `src/eShopLegacyWebForms/Default.aspx.cs` | 50 | 🟢 low |
| 🟡 **medium** | http-handlers | IHttpHandler Pattern | `N/A` | 0 | 🟡 medium |
| 🟡 **medium** | static-files | Static File Serving Configuration | `N/A` | 0 | 🟢 low |
| 🟡 **medium** | mobile-detection | Mobile Master Page | `src/eShopLegacyWebForms/Site.Mobile.Master` | 1 | 🟢 low |
| 🟢 **low** | naming-conventions | Underscore Prefix in Class Names | `src/eShopLegacyWebForms/Default.aspx.cs` | 11 | 🟢 low |
| 🟢 **low** | project-structure | Models in Wrong Location | `src/eShopLegacyWebForms/Models/` | 0 | 🟡 medium |
| 🟢 **low** | security | Input Validation on Parse Operations | `src/eShopLegacyWebForms/Catalog/Create.aspx.cs` | 38 | 🟢 low |
| 🟢 **low** | error-handling | Missing Error Handling in Services | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 22 | 🟢 low |
| 🟢 **low** | dispose-pattern | Manual Dispose Pattern | `src/eShopLegacyWebForms/Services/CatalogService.cs` | 71 | 🟢 low |
| 🟢 **low** | constants | Magic Numbers for Pagination | `src/eShopLegacyWebForms/Default.aspx.cs` | 15 | 🟢 low |
| 🟢 **low** | data-access | HiLo ID Generation Pattern | `src/eShopLegacyWebForms/Models/CatalogItemHiLoGenerator.cs` | 0 | 🟢 low |
| 🟢 **low** | telemetry | ActivityId Correlation Pattern | `src/eShopLegacyWebForms/Global.asax.cs` | 72 | 🟢 low |
| 🟢 **low** | testing | No Unit Tests Found | `N/A` | 0 | 🟡 medium |
| 🟢 **low** | documentation | Missing Documentation | `N/A` | 0 | 🟢 low |

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
