#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;


public class Wedding
{
    [Key] //Primare key
    public int WeddingId { get; set; }



    [Required]
    [MinLength(2, ErrorMessage = "must be at least 2 characters")]
    [MaxLength(30, ErrorMessage = "can't be longer than characters.")]
    public string WedderOne { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "must be at least 2 characters")]
    public string WedderTwo  { get; set; }

    [Required]
    public DateTime WeddingDate { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "must be at least 2 characters")]
    public string Address  { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

/**********************************************************************
Relationship properties below

Foreign Keys: id of a different (foreign) model

Navigation props:
    Data type is a related model
    MUST use .Inlcude for the nav prop data to be included via a SQL JOIN.

*************************************************/

    public int UserID { get; set; } //this foreign HAS TO MACH the PK property name

    public User? Creator { get; set; }

    public List<RSVP> Guests { get; set; } = new List<RSVP>();
}