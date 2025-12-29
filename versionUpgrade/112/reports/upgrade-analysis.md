# 🚀 .NET Upgrade Analysis Report

**Analysis ID:** `112`

**Generated:** 2025-12-19 06:44:30

---

## 📋 Project Information

| Field | Value |
|-------|-------|
| **Project Path** | `/modernize-data/studio-data/TNT1001/APP1967/transformed-code/86/studio-workspace/backendDotnet` |
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
| **Total Issues** | 23 |
| **Critical Issues** | 8 |
| **Deprecated APIs** | 7 |
| **Breaking Changes** | 6 |
| **Estimated Effort** | **120-160 hours** |
| **Upgrade Complexity** | **Complex** |

---

## ⚠️ Identified Issues

| Severity | Category | Title | File | Line | Effort |
|----------|----------|-------|------|------|--------|
| 🔴 **critical** | deprecated-api | Entity Framework 6.2.0 Not Compatible with .NET 8 | `src/eShopLegacyWebForms/packages.config` | 9 | 🔴 high |
| 🔴 **critical** | deprecated-api | log4net Not Recommended for .NET 8 | `src/eShopLegacyWebForms/packages.config` | 11 | 🟡 medium |
| 🔴 **critical** | webforms-migration | System.Web Dependencies Throughout Application | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 173 | 🔴 high |
| 🔴 **critical** | webforms-migration | ASP.NET Web Forms Pages Need Complete Rewrite | `src/eShopLegacyWebForms/Default.aspx` | 1 | 🔴 high |
| 🔴 **critical** | webforms-migration | Master Pages Not Supported in .NET 8 | `src/eShopLegacyWebForms/Site.Master` | 1 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Page Lifecycle Events Not Available | `src/eShopLegacyWebForms/Default.aspx.cs` | 22 | 🟡 medium |
| 🔴 **critical** | webforms-migration | Web Forms Server Controls Not Supported | `src/eShopLegacyWebForms/Default.aspx` | 12 | 🔴 high |
| 🔴 **critical** | configuration | Web.config Not Used in .NET 8 | `src/eShopLegacyWebForms/Web.config` | 1 | 🟡 medium |
| 🟠 **high** | dependency-injection | Autofac Integration Needs Update | `src/eShopLegacyWebForms/Global.asax.cs` | 2 | 🟡 medium |
| 🟠 **high** | webforms-migration | Global.asax Application Events Not Available | `src/eShopLegacyWebForms/Global.asax.cs` | 29 | 🟡 medium |
| 🟠 **high** | webforms-migration | HttpContext.Current Usage | `src/eShopLegacyWebForms/Global.asax.cs` | 43 | 🟡 medium |
| 🟠 **high** | webforms-migration | Session State Configuration Incompatible | `src/eShopLegacyWebForms/Web.config` | 29 | 🟡 medium |
| 🟠 **high** | webforms-migration | HTTP Modules Need Migration to Middleware | `src/eShopLegacyWebForms/Web.config` | 38 | 🟡 medium |
| 🟠 **high** | webforms-migration | Web Optimization Bundle Configuration | `src/eShopLegacyWebForms/App_Start/BundleConfig.cs` | 1 | 🟡 medium |
| 🟠 **high** | webforms-migration | Routing Configuration Incompatible | `src/eShopLegacyWebForms/App_Start/RouteConfig.cs` | 1 | 🟡 medium |
| 🟡 **medium** | package-compatibility | Microsoft.AspNet Packages Not Compatible | `src/eShopLegacyWebForms/packages.config` | 19 | 🟢 low |
| 🟡 **medium** | package-compatibility | Application Insights Legacy Packages | `src/eShopLegacyWebForms/packages.config` | 12 | 🟢 low |
| 🟡 **medium** | webforms-migration | ViewState Dependencies | `src/eShopLegacyWebForms/Default.aspx` | 12 | 🟡 medium |
| 🟡 **medium** | webforms-migration | Postback Event Model | `src/eShopLegacyWebForms/Default.aspx.cs` | 37 | 🟡 medium |
| 🟡 **medium** | webforms-migration | Data Binding Expressions | `src/eShopLegacyWebForms/Default.aspx` | 58 | 🟡 medium |
| 🟡 **medium** | webforms-migration | User Control (.ascx) Migration | `src/eShopLegacyWebForms/ViewSwitcher.ascx` | 1 | 🟢 low |
| 🟢 **low** | package-compatibility | Older jQuery and Bootstrap Versions | `src/eShopLegacyWebForms/packages.config` | 10 | 🟢 low |
| 🟢 **low** | package-compatibility | Target Framework Version Update Required | `src/eShopLegacyWebForms/eShopLegacyWebForms.csproj` | 19 | 🟢 low |

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
