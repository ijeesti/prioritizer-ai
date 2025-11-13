namespace Prioritizer.PolicyAgent.Policies;

/// <summary>
/// Defines the critical, dynamic policy constraints that the Policy Agent provides
/// for subsequent agents (like the Product Agent) to apply when making a final decision.
/// </summary>
public static class RuntimePolicies
{
    /// <summary>
    /// A dictionary where the key is the product name (e.g., "CoreSaaS", "PublicAPI")
    /// and the value is the comprehensive policy string for that product.
    /// </summary>
    public static readonly Dictionary<string, string> ProductPolicies = new(StringComparer.OrdinalIgnoreCase)
    {
        // ------------------------------------------------------------------------------------------------
        // DEMO POLICY FOR CORE SAAS PRODUCT (High-Risk, High-Compliance)
        // ------------------------------------------------------------------------------------------------
        {"CoreSaaSPlatform", """
            ## Core SaaS Platform Policy Guidelines (V3.0 - High Compliance)
            
            This policy governs the final decision based on risk, value, and effort for our primary, customer-facing platform.
            
            1.  **Security & Compliance (Highest Priority - Strict):**
                * **PII/Financial Data:** Any change involving PII or payment tokens MUST trigger a mandatory security review. If the Policy Agent risk score is 8/10 or higher, the decision must be **REJECT** or **DEFER**.
                * **Data Sovereignty:** All new data storage must default to the EU region for GDPR compliance.
            
            2.  **Service Level Agreements (SLAs) & Reliability (Criticality Assessment):**
                * **BUG FIX PRIORITY:** Any bug fix related to an outage, data corruption, or workflow failure impacting more than 5% of users (P1/P2 incidents) is considered **CRITICAL** and must be **APPROVED** regardless of cost, provided the implementation risk is low (<4/10).
                * **Latency Budget:** User-facing synchronous API calls must maintain a 99th percentile latency of under 500ms.
                * **Uptime Target:** Features critical to the core workflow require a design target of 99.99% uptime.
            
            3.  **Strategic & Financial Viability:**
                * **New Feature Cost Ceiling:** Monthly cloud infrastructure cost for *new features* MUST NOT exceed $5,000. If the Tech Agent estimates higher, the feature must be **DEFERRED**.
                * **Technical Debt Reduction (Enhancements):** Enhancements dedicated purely to refactoring are given a **10% capacity budget** each sprint and should be **APPROVED** if they fall within this budget.
            
            4.  **Operational Constraints:**
                * **Team Ownership:** The 'Billing' service is owned by the Account Team and has the highest change control requirements.
            """},

        // ------------------------------------------------------------------------------------------------
        // DEMO POLICY FOR PUBLIC API PRODUCT (Medium-Risk, External Facing)
        // ------------------------------------------------------------------------------------------------
        {"PublicAPIService", """
            ## Public API Service Policy Guidelines (V1.2 - External Stability)
            
            This policy prioritizes stability, external compatibility, and clear versioning over rapid internal feature velocity.
            
            1.  **Security & Compliance (High Priority - Focus on Availability):**
                * **Authentication:** All new endpoints MUST use OAuth 2.0 with scope checking.
                * **Breaking Changes:** Features or fixes that introduce a breaking change to a current API version must be **REJECTED**. New versions must be introduced instead.
            
            2.  **Service Level Agreements (SLAs) & Reliability:**
                * **BUG FIX PRIORITY:** Any fix impacting external developer functionality is **CRITICAL**. Implementation risk must be kept below 3/10.
                * **Latency Budget:** API endpoints must maintain a 99th percentile latency of under **200ms**. This is stricter than the Core SaaS Platform.
            
            3.  **Strategic & Financial Viability:**
                * **New Feature Cost Ceiling:** Monthly cloud infrastructure cost for *new features* MUST NOT exceed $1,500. This is a low-margin service.
                * **Documentation Mandate:** All new features must include 100% complete OpenAPI (Swagger) documentation. If not documented, the feature is **DEFERRED**.
            
            4.  **Operational Constraints:**
                * **Stack Mandate:** Implementation must use the standard C#/.NET Core stack.
                * **CTO Approval:** Use of any external C# NuGet packages not currently on the approved list must be flagged for CTO review.
            """}
    };

    /// <summary>
    /// Fallback policy if a product ID is not found.
    /// </summary>
    public const string DefaultPolicy = """
        ## Default Policy (Low Priority / Internal Tooling)
        
        Use this policy only for internal-facing tools or dashboards. High risk (7/10+) will be rejected. 
        Cost ceiling is $200/month. Uptime target is 99.5%.
        """;
}