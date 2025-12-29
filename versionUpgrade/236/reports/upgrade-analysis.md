# 🚀 .NET Upgrade Analysis Report

**Analysis ID:** `236`

**Generated:** 2025-12-29 10:49:24

---

## 📋 Project Information

| Field | Value |
|-------|-------|
| **Project Path** | `/modernize-data/studio-data/TNT1001/APP2160/transformed-code/134/studio-workspace/dotnet_backend` |
| **Current Version** | .NET 4.6 |
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
| 🔴 **critical** | package-compatibility | Entity Framework 5.0 Incompatible with .NET 8 | `packages.config` | 12 | 🔴 high |
| 🔴 **critical** | framework-incompatibility | Target Framework .NET Framework 4.6 | `FIlms.csproj` | 16 | 🔴 high |
| 🔴 **critical** | system-web-dependency | System.Web References Throughout Codebase | `Multiple files` | 0 | 🔴 high |
| 🔴 **critical** | webforms-migration | Web Forms Pages Require Complete Rewrite | `Multiple .aspx files` | 0 | 🔴 high |
| 🔴 **critical** | webforms-migration | Master Pages Not Supported in .NET 8 | `Site.Master` | 1 | 🟡 medium |
| 🔴 **critical** | webforms-migration | User Control (ViewSwitcher.ascx) Requires Migration | `ViewSwitcher.ascx` | 0 | 🟡 medium |
| 🔴 **critical** | configuration-migration | Web.config Must Migrate to appsettings.json | `Web.config` | 44 | 🔴 high |
| 🔴 **critical** | authentication-migration | Forms Authentication Not Supported | `Web.config` | 44 | 🔴 high |
| 🔴 **critical** | application-startup | Global.asax Not Supported in .NET 8 | `Global.asax.cs` | 14 | 🟡 medium |
| 🔴 **critical** | data-access-migration | EDMX Model File Requires Migration | `FIlms.csproj` | 233 | 🔴 high |
| 🔴 **critical** | package-compatibility | Microsoft.AspNet.Web.Optimization Not Compatible | `packages.config` | 22 | 🟡 medium |
| 🔴 **critical** | package-compatibility | DotNetOpenAuth Not Compatible with .NET 8 | `packages.config` | 6 | 🔴 high |
| 🟠 **high** | webforms-controls | Server-Side Validation Controls | `LogIn.aspx` | 21 | 🟡 medium |
| 🟠 **high** | webforms-controls | Panel Controls Used for Conditional Display | `LogIn.aspx` | 13 | 🟢 low |
| 🟠 **high** | webforms-controls | LoginView and LoginStatus Controls | `Site.Master` | 48 | 🟡 medium |
| 🟠 **high** | webforms-lifecycle | Page Lifecycle Event Handlers | `Multiple .aspx.cs files` | 0 | 🟡 medium |
| 🟠 **high** | webforms-postback | Button Click Event Handlers (Postback Pattern) | `LogIn.aspx.cs` | 18 | 🟡 medium |
| 🟠 **high** | state-management | ViewState Usage Detected | `Site.Master` | 48 | 🟡 medium |
| 🟠 **high** | state-management | Session State Configuration | `Web.config` | 68 | 🟡 medium |
| 🟠 **high** | response-methods | Response.Redirect Usage | `Default.aspx.cs` | 19 | 🟢 low |
| 🟠 **high** | package-compatibility | AjaxControlToolkit Not Compatible | `packages.config` | 3 | 🔴 high |
| 🟠 **high** | package-compatibility | Microsoft.AspNet.ScriptManager Not Compatible | `packages.config` | 20 | 🟡 medium |
| 🟠 **high** | package-compatibility | Microsoft.AspNet.FriendlyUrls Not Compatible | `packages.config` | 15 | 🟡 medium |
| 🟠 **high** | package-compatibility | System.Web.Providers Not Compatible | `packages.config` | 18 | 🔴 high |
| 🟠 **high** | webforms-controls | ScriptManager Control in Master Page | `Site.Master` | 18 | 🟢 low |
| 🟠 **high** | connection-string | Entity Framework Connection String Format | `Web.config` | 14 | 🟢 low |
| 🟠 **high** | database-entities | Entity Framework Models Require Regeneration | `FIlms.csproj` | 418 | 🟡 medium |
| 🟠 **high** | webforms-controls | AutoPostBack Pattern Used | `LogIn.aspx` | 57 | 🟡 medium |
| 🟡 **medium** | project-structure | Old-Style Project File Format | `FIlms.csproj` | 2 | 🟡 medium |
| 🟡 **medium** | configuration | AppSettings and ConnectionStrings Migration | `Web.config` | 11 | 🟢 low |
| 🟡 **medium** | webforms-controls | ContentPlaceHolder Controls | `Site.Master` | 75 | 🟢 low |
| 🟡 **medium** | ui-controls | Image Control Usage | `Default.aspx` | 11 | 🟢 low |
| 🟡 **medium** | ui-controls | Button Server Controls | `LogIn.aspx` | 46 | 🟢 low |
| 🟡 **medium** | ui-controls | TextBox Server Controls | `LogIn.aspx` | 20 | 🟢 low |
| 🟡 **medium** | ui-controls | Label Server Controls | `Site.Master` | 43 | 🟢 low |
| 🟡 **medium** | routing | RouteConfig.RegisterRoutes Pattern | `Global.asax.cs` | 19 | 🟢 low |
| 🟡 **medium** | bundling | BundleConfig for Asset Management | `Global.asax.cs` | 17 | 🟡 medium |
| 🟡 **medium** | jquery-version | jQuery 1.8.2 Very Outdated | `packages.config` | 13 | 🟡 medium |
| 🟡 **medium** | modernizr | Modernizr 2.6.2 Outdated | `packages.config` | 25 | 🟢 low |
| 🟢 **low** | naming-convention | Project Name Has Inconsistent Casing | `FIlms.csproj` | 14 | 🟢 low |
| 🟢 **low** | localization | Bulgarian Text in UI | `Multiple files` | 0 | 🟡 medium |
| 🟢 **low** | static-files | jQuery UI Theme Files | `Content/themes/base/` | 0 | 🟢 low |
| 🟢 **low** | error-handling | Application_Error Event Handler | `Global.asax.cs` | 28 | 🟢 low |
| 🟡 **medium** | accessibility | Missing HTML5 Semantic Elements | `LogIn.aspx` | 14 | 🟡 medium |
| 🟢 **low** | commented-code | Commented Code and Settings | `Multiple files` | 0 | 🟢 low |
| 🟡 **medium** | architecture | No Separation of Concerns | `Multiple .aspx.cs files` | 0 | 🔴 high |
| 🟡 **medium** | testing | No Unit Tests Present | `Solution` | 0 | 🟡 medium |

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
