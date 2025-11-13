using System.ComponentModel.DataAnnotations;


namespace Prioritizer.Contracts.Models;

/// <summary>
/// The request body for starting a new prioritization process.
/// </summary>
public record PrioritizeRequest
{
    [Required]
    [MinLength(20, ErrorMessage = "The proposal must be detailed.")]
    public string Proposal { get; init; } = string.Empty;

    public string BusinessValue { get; set; } = string.Empty;

    public string RequestedBy { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty; //Enum ProductType?

    //add more fields like priority level, business value etc.
}