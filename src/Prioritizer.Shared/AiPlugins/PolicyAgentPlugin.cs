using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Prioritizer.Shared.AiPlugins;

/// <summary>
/// Tools available to the Policy Agent for gathering compliance data,
/// assessing policy adherence, and reading internal documentation.
/// </summary>
public class PolicyAgentPlugin
{
    /// <summary>
    /// Identifies the internal team responsible for a given service.
    /// Used to determine stakeholders for a policy review.
    /// </summary>
    [KernelFunction, Description("Provide detail of the internal team responsible for a given service name.")]
    public string GetTeamDetail(
        [Description("The name of the service or component (e.g., 'billing', 'document', 'payment').")] string serviceName)
    {

        if (string.IsNullOrEmpty(serviceName))
        {
            return "Error: Service name is required to find the responsible team.";
        }

        return serviceName.ToLower() switch
        {
            "billing" => "Account Management Team (Billing Policy Specialist)",
            "upload" or "document" => "Document Storage and Compliance Team",
            "payment" => "Payment and Checkout Compliance Team",
            _ => "General Policy Review Board",
        };
    }

    /// <summary>
    /// Mocks querying an internal policy database for rules related to a keyword.
    /// </summary>
    [KernelFunction, Description("Queries the Central Policy Database for rules related to the given keyword.")]
    public string CheckPolicyDatabase(
        [Description("The single keyword or concept to search the policy database for (e.g., 'GDPR', 'data retention', 'audit log').")] string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return "Please provide a specific keyword to search the policy database.";
        }

        return keyword.ToLower() switch
        {
            "gdpr" => "Rule 4.5: All European user data must be anonymized or pseudonymized after 90 days. Rule 8.2: Requires explicit consent for all personal data processing.",
            "data retention" => "Retention Policy 2.1: Financial transaction logs must be retained for 7 years. Non-essential user data may be deleted after 1 year of inactivity.",
            "audit log" => "Security Standard 1.0: All API changes and financial mutations must generate an immutable audit log entry. Logs must be stored in a write-once, read-many system.",
            _ => $"No specific policy rules found for '{keyword}'. Requires a manual review.",
        };
    }

    /// <summary>
    /// Mocks reading a large external document (like a PDF) and summarizing relevant sections.
    /// </summary>
    [KernelFunction, Description("Reads and summarizes the relevant section of a large compliance document based on the section title.")]
    public string ReadComplianceDocument(
        [Description("The title of the specific document section to summarize (e.g., 'Security Risk Matrix', 'API Standards').")] string sectionTitle)
    {
        if (string.IsNullOrEmpty(sectionTitle))
        {
            return "Please specify the exact section title of the compliance document to analyze.";
        }

        return sectionTitle.ToLower() switch
        {
            "security risk matrix" => "Summary: High-risk projects require 2-factor authentication on all administrative endpoints. Medium-risk projects need quarterly security audits. Low-risk is exempt from immediate review.",
            "api standards" => "Summary: All new public APIs must use OAuth 2.0 and follow RESTful principles. Deprecation of any API requires a 6-month notice period.",
            _ => $"Document section '{sectionTitle}' is unavailable or invalid.",
        };
    }

    /// <summary>
    /// Calculates a preliminary risk score based on the project's impact area.
    /// </summary>
    [KernelFunction, Description("Calculates a preliminary risk score (1-10, 10 being highest) based on the impact area of the project.")]
    public int CalculateRiskScore(
        [Description("The primary area of impact for the new feature (e.g., 'customer data', 'finance', 'marketing', 'infrastructure').")] string impactArea)
    {
        if (string.IsNullOrEmpty(impactArea))
        {
            return 5; // Neutral score if area is unknown
        }

        return impactArea.ToLower() switch
        {
            "finance" => 9,         // High risk, impacts money
            "customer data" => 8,   // High risk, impacts privacy/GDPR
            "infrastructure" => 7,  // Medium risk, potential for downtime
            "marketing" => 3,       // Low risk
            _ => 5,
        };
    }
}
